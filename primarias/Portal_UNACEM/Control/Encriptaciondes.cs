using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

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
}
