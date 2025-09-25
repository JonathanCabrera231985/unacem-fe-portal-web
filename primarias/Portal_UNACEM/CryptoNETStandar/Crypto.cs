using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoNETStandar
{
    public class Crypto : ICrypto
    {
        public string Encrypt(string plainText)
        {
            byte[] encrypted;
            var key = "EncrypAES$ipecom";
            // Create an Aes object with the specified key and IV.
            using (Aes aes = Aes.Create())
            {

                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16];

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                // Create a new MemoryStream object to contain the encrypted bytes.
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Create a CryptoStream object to perform the encryption.
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        // Encrypt the plaintext.
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        encrypted = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }
        public string Decrypt(string cipherText)
        {
            string decrypted;
            var key = "EncrypAES$ipecom";
            // Create an Aes object with the specified key and IV.
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16];

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                // Create a new MemoryStream object to contain the decrypted bytes.
                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    // Create a CryptoStream object to perform the decryption.
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        // Decrypt the ciphertext.
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            decrypted = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return decrypted;
        }
    }
}
