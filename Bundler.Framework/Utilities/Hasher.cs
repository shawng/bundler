using System.Security.Cryptography;
using System.Text;

namespace Bundler.Framework.Utilities
{
    public class Hasher
    {
        public static string Create(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            byte[] hashBytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
            return ByteArrayToString(hashBytes);
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            return System.Convert.ToBase64String(arrInput).Replace("=", "").Replace('+', '-').Replace('/', '_');
        }
    }
}