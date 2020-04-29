using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Web.Api.Common.Functions
{
    public class Criptography
    {
       
        /// <summary>
        //  Encriptacion sha256hash
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //Generamos hash       
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                //Convertimos arreglo a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
