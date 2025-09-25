using ClibLogger;
using Microsoft.IdentityModel.Tokens;
using swConsultaDoc.Api.Services.Contracts;
using swConsultaDoc.Data.Interfaces;
using swConsultaDoc.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace swConsultaDoc.Api.Services
{
    public class AuthService : IAuthService
    {
        #region Atributos
        static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;
        string className = "";
        #endregion

        private readonly string? key;
        private readonly string? issuer;
        private readonly string? audience;
        private readonly IConsultaDoc usuarios;
        private readonly ILoggerManager _logger;

        public AuthService(IConfiguration config, IConsultaDoc usuarios, ILoggerManager loggerManager)
        {
            key = config.GetSection("AuthenticationSettings").GetValue("SigningKey", "");
            issuer = config.GetSection("AuthenticationSettings").GetValue("Issuer", "");
            audience = config.GetSection("AuthenticationSettings").GetValue("Audience", "");
            _logger = loggerManager;
            this.usuarios = usuarios;
        }
        public Object? ValidateLogin(UsuarioRequestModel Datos)
        {
            Datos.Password = this.ComputeSha256Hash(Datos.Password);
            string mensajeerror = "";
            Object? respuesta = new object();

            UsuarioModel? Usuario = usuarios.ValidaUsuario(Datos, ref mensajeerror);

            if (Usuario == null)
            {
                return null;
            }
            if (Datos.Password.ToUpper() == Usuario.Clave.ToString().ToUpper() && Usuario.Estado)
            {
                var fechaActual = DateTime.UtcNow;
                var validez = TimeSpan.FromHours(8);
                var fechaExpiracion = fechaActual.Add(validez);

                var token = this.GenerateToken(fechaActual, Usuario, validez);
                return new
                {
                    Token = token,
                    ExpireAt = fechaExpiracion
                };
            }
            //if (username.Equals(usr.Username) && password.Equals(usr.Password))
            //    return true;
            return null;
        }

        public string ComputeSha256Hash(string rawData)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public string GenerateToken(DateTime fechaActual, UsuarioModel usuario, TimeSpan tiempoValidez)
        {
            DateTime fechaExpiracion = fechaActual.Add(tiempoValidez);
            //Configuramos las claims
            Claim[] claims = new Claim[]
            {
            new(JwtRegisteredClaimNames.Sub,usuario.NombreUsuario),
            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                new DateTimeOffset(fechaActual).ToUniversalTime().ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64
            ),
            new(JwtRegisteredClaimNames.UniqueName, usuario.UsuarioId),
            };

            //Añadimos las credenciales
            SigningCredentials signingCredentials = new(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature
            );

            //Configuracion del jwt token
            JwtSecurityToken jwt = new(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: fechaActual,
                expires: fechaExpiracion,
                signingCredentials: signingCredentials
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        public string UsuarioId(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            SecurityToken validatedToken;
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var usernameClaim = principal?.Identity?.Name;
                if (usernameClaim != null)
                {
                    return usernameClaim;
                }
                return "";
            }
            catch (Exception)
            {
                // Manejo de error, el token no es válido o hubo un problema al validar
                return "";
            }
        }
        public string NombreUsuario(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            SecurityToken validatedToken;
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var usernameClaim = principal.FindFirst(ClaimTypes.NameIdentifier) ?? principal.FindFirst(JwtRegisteredClaimNames.Sub);
                if (usernameClaim != null)
                {
                    return usernameClaim.Value;
                }
                return "";
            }
            catch (Exception)
            {
                // Manejo de error, el token no es válido o hubo un problema al validar
                return "";
            }
        }


        public Response<JsonElement?> Valida(UsuarioRequestModel Datos)
        {
            #region LOG
            string _metodo = GetActualAsyncMethodName();
            _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> INICIA", className, _metodo));
            #endregion
            Response<JsonElement?> respuesta = new();
            string mensajeerror = "";
            try
            {
                Datos.Password = this.ComputeSha256Hash(Datos.Password);
                UsuarioModel? Usuario = new UsuarioModel();
                Usuario = usuarios.ValidaUsuario(Datos, ref mensajeerror);
               
                if ( !string.IsNullOrEmpty(mensajeerror))
                {
                    mensajeerror = string.IsNullOrEmpty(mensajeerror) ? "USUARIO NO AUNTENTICADO" : mensajeerror;
                    respuesta.Estado = "ERROR";
                    respuesta.Mensaje = mensajeerror;
                }

                if (Datos.Password.ToUpper() == Usuario.Clave.ToString().ToUpper() && Usuario.Estado)
                {
                    var fechaActual = DateTime.UtcNow;
                    var validez = TimeSpan.FromHours(8);
                    var fechaExpiracion = fechaActual.Add(validez);

                    var token = this.GenerateToken(fechaActual, Usuario, validez);

                    // Crea un objeto anónimo con los valores del token y la fecha de expiración
                    var tokenData = new
                    {
                        Token = token,
                        ExpireAt = fechaExpiracion
                    };

                    // Serializa el objeto anónimo a JSON
                    var jsonElement = JsonSerializer.SerializeToElement(tokenData);

                    // Llena la respuesta
                    respuesta = new Response<JsonElement?>
                    {
                        Estado = "OK",
                        Data = jsonElement,
                        Mensaje = "Token generado correctamente"
                    };

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(String.Format("ClassName: {0} - Metodo: {1} -- Error: {2}", className, _metodo, ex.Message));
                respuesta.Estado = "ERROR";
                respuesta.Mensaje = "Ocurrio un Error Interno";
            }

            _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> FIN", className, _metodo));
            return respuesta;

        }


    }


}
