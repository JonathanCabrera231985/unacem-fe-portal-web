using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web;

namespace Control
{
    public class Encriptaciondes
    {

        public class ClaseEncriptacion
        {
            static byte[] key = { };
            static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
            static string sEncryptionKey = "!#$a54?3";

            public string Decrypt(string stringToDecrypt)
            {
                byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
                try
                {
                    key = System.Text.Encoding.UTF8.GetBytes(LeftRightMid.Left(sEncryptionKey, 8));
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    inputByteArray = Convert.FromBase64String(stringToDecrypt);
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                    return encoding.GetString(ms.ToArray());
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }

            public string Encrypt(string stringToEncrypt)
            {
                try
                {
                    key = System.Text.Encoding.UTF8.GetBytes(LeftRightMid.Left(sEncryptionKey, 8));
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }



            #region instance
            private static ClaseEncriptacion m_instance;
            // Properties
            public static ClaseEncriptacion Instance
            {
                get
                {
                    if (m_instance == null)
                    {
                        m_instance = new ClaseEncriptacion();
                    }
                    return m_instance;
                }
            }


            #endregion
        }

        public class LeftRightMid
        {
            /// <summary>
            /// The main entry point for the application.
            /// </summary>
            [STAThread]
            static void Main(string[] args)
            {
                //assign a value to our string
                string myString = "This is a string";
                //get 4 characters starting from the left
                Console.WriteLine(Left(myString, 4));
                //get 6 characters starting from the right
                Console.WriteLine(Right(myString, 6));
                //get 4 characters starting at index 5 of the string
                Console.WriteLine(Mid(myString, 5, 4));
                //get the characters from index 5 up to the end of the string
                Console.WriteLine(Mid(myString, 5));
                //display the result to the screen
                Console.ReadLine();
            }

            public static string Left(string param, int length)
            {
                //we start at 0 since we want to get the characters starting from the
                //left and with the specified lenght and assign it to a variable
                string result = param.Substring(0, length);
                //return the result of the operation
                return result;
            }

            public static string Right(string param, int length)
            {
                //start at the index based on the lenght of the sting minus
                //the specified lenght and assign it a variable
                int value = param.Length - length;
                string result = param.Substring(value, length);
                //return the result of the operation
                return result;
            }

            public static string Mid(string param, int startIndex, int length)
            {
                //start at the specified index in the string ang get N number of
                //characters depending on the lenght and assign it to a variable
                string result = param.Substring(startIndex, length);
                //return the result of the operation
                return result;
            }

            public static string Mid(string param, int startIndex)
            {
                //start at the specified index and return all characters after it
                //and assign it to a variable
                string result = param.Substring(startIndex);
                //return the result of the operation
                return result;
            }

            #region instance
            private static LeftRightMid m_instance;
            // Properties
            public static LeftRightMid Instance
            {
                get
                {
                    if (m_instance == null)
                    {
                        m_instance = new LeftRightMid();
                    }
                    return m_instance;
                }
            }


            #endregion

        }
    }

    public class QueryString : IHttpModule
    {
        ///
        /// variables const
        ///
        private const string nombreParametro = "q=";
        //private const string nombreParametro;
        private const string llaveEncriptacion = "key";

        ///
        /// Salt para "reforzar" encriptado
        ///
        private readonly static byte[] salt = Encoding.ASCII.GetBytes(llaveEncriptacion.Length.ToString());

        ///
        ///
        ///
        public QueryString()
        {
            //Algún día necesitaré esto lol
        }

        ///
        /// Implementamos Dispose de interfaz
        ///
        public void Dispose()
        {
            //Nothing
        }

        ///
        /// Implementamos Init de Interfaz
        /// Con esto construimos un nuevo evento para manejar la petición
        ///
        ///
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        ///
        /// Evento que maneja la petición
        ///
        ///
        ///
        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current; //Contexto http actual

            if (context.Request.Url.OriginalString.Contains("aspx") && context.Request.RawUrl.Contains("?")) //url contiene ".aspx" && "?" ?
            {
                string query = ExtraerCadena(context.Request.RawUrl);
                string ruta = ObtenerRutaVirtual();

                if (query.StartsWith(nombreParametro, StringComparison.OrdinalIgnoreCase))
                {
                    // Desencripta queryString y se vuelve a establecer la ruta
                    string rawQuery = query.Replace(nombreParametro, string.Empty);
                    string decryptedQuery = Desencriptar(rawQuery);
                    context.RewritePath(ruta, string.Empty, decryptedQuery);
                }
                else if (context.Request.HttpMethod == "GET")
                {
                    // Encripta queryString y reedirecciona a la url encriptada
                    //Encripta todas las queryString automáticamente. ****Eliminar esta parte si no se desea encriptar automáticamente y realizar otras acciones
                    string encryptedQuery = Encriptar(query);
                    context.Response.Redirect(ruta + encryptedQuery);
                }
            }
        }

        ///
        /// Analiza la url actual y extrae la ruta virtual sin usar la queryString
        ///
        /// Ruta virtual de la url actual
        private static string ObtenerRutaVirtual()
        {
            string ruta = HttpContext.Current.Request.RawUrl;
            ruta = ruta.Substring(0, ruta.IndexOf("?"));
            ruta = ruta.Substring(ruta.LastIndexOf("/") + 1);
            return ruta;
        }

        ///
        /// Analiza la url y regresa la queryString
        ///
        ///url a analizar
        /// QueryString sin signo "?"
        private static string ExtraerCadena(string url)
        {
            int indice = url.IndexOf("?") + 1;
            return url.Substring(indice);
        }

        ///
        /// Encripta cualquier cadena con el algoritmo Rijndael.
        ///
        ///Cadena a encriptar
        /// Cadena encriptada Base64
        public static string Encriptar(string cadenaEntrada)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] textoPlano = Encoding.Unicode.GetBytes(cadenaEntrada);
            PasswordDeriveBytes llave = new PasswordDeriveBytes(llaveEncriptacion, salt);

            using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(llave.GetBytes(32), llave.GetBytes(16)))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(textoPlano, 0, textoPlano.Length);
                        cryptoStream.FlushFinalBlock();
                        return "?" + nombreParametro + Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        ///
        /// Desencripta la cadena encriptada previamente
        ///
        ///Cadena a desencriptar
        /// Cadena desencriptada
        public static string Desencriptar(string cadenaEntrada)
        {
            try
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                byte[] datosCifrados = Convert.FromBase64String(cadenaEntrada);
                PasswordDeriveBytes llave = new PasswordDeriveBytes(llaveEncriptacion, salt);

                using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(llave.GetBytes(32), llave.GetBytes(16)))
                {
                    using (MemoryStream memoryStream = new MemoryStream(datosCifrados))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] textoPlano = new byte[datosCifrados.Length];
                            int contador = cryptoStream.Read(textoPlano, 0, textoPlano.Length);
                            return Encoding.Unicode.GetString(textoPlano, 0, contador);
                        }
                    }
                }
            }
            //Por implementación rápida, cualquier error o cualquier dato extra que introduzca el usuario en la url encriptada o algo por el estilo, lo enviaré a la misma //página de error personalizada
            catch (FormatException)
            {
                HttpContext context = HttpContext.Current;
                context.Response.Redirect("Default.aspx");
                return null;
            }
            catch (CryptographicException)
            {
                HttpContext context = HttpContext.Current;
                context.Response.Redirect("Default.aspx");
                return null;
            }
            catch (IndexOutOfRangeException)
            {
                HttpContext context = HttpContext.Current;
                context.Response.Redirect("Default.aspx");
                return null;
            }
            finally
            {
                //Destruimos o finalizamos lo que necesitemos. No olvidar que existe el método Dispose ;)
            }
        }
    }
}
