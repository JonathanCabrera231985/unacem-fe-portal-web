using System;  
    using System.Security.Cryptography;  
    using System.Text;  
    using System.IO; 

namespace CriptoSimetrica
{
    public class AES
    {
        private int keySize = 32;
        private int ivSize = 16;
        private RijndaelManaged rij = new RijndaelManaged();
        public string encriptar(string cadena, string clave)
        {
            // Convierto la cadena y la clave en arreglos de bytes
            // para poder usarlas en las funciones de encriptacion
            byte[] cadenaBytes = Encoding.UTF8.GetBytes(cadena);
            byte[] claveBytes = Encoding.UTF8.GetBytes(clave);
            byte[] iv  = rij.IV;  
            Array.Resize(ref claveBytes, keySize);
            Array.Resize(ref iv, ivSize); 
            // Creo un objeto de la clase Rijndael
            

            // Configuro para que utilice el modo ECB
            rij.Mode = CipherMode.ECB;

            // Configuro para que use encriptacion de 256 bits.
            rij.BlockSize = 256;

            // Declaro que si necesitara mas bytes agregue ceros.
           // rij.Padding = PaddingMode.Zeros;

            // Declaro un encriptador que use mi clave secreta y un vector
            // de inicializacion aleatorio
            ICryptoTransform encriptador;
            encriptador = rij.CreateEncryptor(claveBytes,iv );

            // Declaro un stream de memoria para que guarde los datos
            // encriptados a medida que se van calculando
            MemoryStream memStream = new MemoryStream();

            // Declaro un stream de cifrado para que pueda escribir aqui
            // la cadena a encriptar. Esta clase utiliza el encriptador
            // y el stream de memoria para realizar la encriptacion
            // y para almacenarla
            CryptoStream cifradoStream;
            cifradoStream = new CryptoStream(memStream, encriptador, CryptoStreamMode.Write);

            // Escribo los bytes a encriptar. A medida que se va escribiendo
            // se va encriptando la cadena
            cifradoStream.Write(cadenaBytes, 0, cadenaBytes.Length);

            // Aviso que la encriptación se terminó
            cifradoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memStream.ToArray();

            // Cierro los dos streams creados
            memStream.Close();
            cifradoStream.Close();

            // Convierto el resultado en base 64 para que sea legible
            // y devuelvo el resultado
            return Convert.ToBase64String(cipherTextBytes);
        }
        public string desencriptar(string cadena, string clave)
        {
         //   RijndaelManaged rij = new RijndaelManaged();
            // Convierto la cadena y la clave en arreglos de bytes
            // para poder usarlas en las funciones de encriptacion
            // En este caso la cadena la convierta usando base 64
            // que es la codificacion usada en el metodo encriptar
            byte[] cadenaBytes = Convert.FromBase64String(cadena);
            byte[] claveBytes = Encoding.UTF8.GetBytes(clave);
            byte[] iv = rij.IV;
            Array.Resize(ref claveBytes, keySize);
            Array.Resize(ref iv, ivSize); 
            // Creo un objeto de la clase Rijndael
            

            // Configuro para que utilice el modo ECB
            rij.Mode = CipherMode.ECB;

            // Configuro para que use encriptacion de 256 bits.
            rij.BlockSize =256;

            // Declaro que si necesitara mas bytes agregue ceros.
           // rij.Padding = PaddingMode.Zeros;

            // Declaro un desencriptador que use mi clave secreta y un vector
            // de inicializacion aleatorio
            ICryptoTransform desencriptador;
            desencriptador = rij.CreateDecryptor(claveBytes,iv );

            // Declaro un stream de memoria para que guarde los datos
            // encriptados
            MemoryStream memStream = new MemoryStream(cadenaBytes);

            // Declaro un stream de cifrado para que pueda leer de aqui
            // la cadena a desencriptar. Esta clase utiliza el desencriptador
            // y el stream de memoria para realizar la desencriptacion
            CryptoStream cifradoStream;
            cifradoStream = new CryptoStream(memStream, desencriptador, CryptoStreamMode.Read);

            // Declaro un lector para que lea desde el stream de cifrado.
            // A medida que vaya leyendo se ira desencriptando.
            StreamReader lectorStream = new StreamReader(cifradoStream);

            // Leo todos los bytes y lo almaceno en una cadena
            string resultado = Convert.ToString(lectorStream.ReadToEnd());

            // Cierro los dos streams creados
            memStream.Close();
            cifradoStream.Close();

            // Devuelvo la cadena
            return resultado;
        }


    }
}