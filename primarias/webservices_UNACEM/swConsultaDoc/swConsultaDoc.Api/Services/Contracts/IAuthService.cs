using swConsultaDoc.Domain;
using System.Text.Json;

namespace swConsultaDoc.Api.Services.Contracts
{
    public interface IAuthService
    {
        Response<JsonElement?> Valida(UsuarioRequestModel Datos);
        public Object? ValidateLogin(UsuarioRequestModel Datos);
        string GenerateToken(DateTime fechaActual, UsuarioModel usuario, TimeSpan tiempoValidez);
        string ComputeSha256Hash(string rawData);
        string NombreUsuario(string token);
        string UsuarioId(string token);

    }
}
