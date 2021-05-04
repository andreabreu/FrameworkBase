using System;
using System.Security.Cryptography;
using System.Text;

namespace Framework.Core.Utils
{
    public static class CryptUtils
    {
        private static readonly TripleDESCryptoServiceProvider TripleDES = new();
        private static readonly MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

        private static byte[] MD5Hash(string value)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(value);
            return MD5.ComputeHash(byteArray);
        }

        public static string Crypt(string key, string text)
        {
            TripleDES.Key = MD5Hash(key);
            TripleDES.Mode = CipherMode.CBC;
            byte[] Buffer = Encoding.ASCII.GetBytes(text);
            return Convert.ToBase64String(TripleDES.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        public static string Decrypt(string key, string text)
        {
            TripleDES.Key = MD5Hash(key);
            TripleDES.Mode = CipherMode.ECB;
            byte[] Buffer = Convert.FromBase64String(text);
            return Encoding.ASCII.GetString(TripleDES.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
        }
    }
}
