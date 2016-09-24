using System;
using System.Security.Cryptography;
using System.Text;

namespace cinnamon.api
{
    public class Helper
    {
        private static char[] _base62chars =
                "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
                .ToCharArray();

        private static Random _random = new Random();

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = System.Security.Cryptography.SHA256.Create();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static string GenerateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            var val = BitConverter.ToInt64(buffer, 0);
            return val.ToString();
        }

        public static string GetBase62(int length)
        {
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
                sb.Append(_base62chars[_random.Next(62)]);

            return sb.ToString();
        }

        public static string GetBase36(int length)
        {
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
                sb.Append(_base62chars[_random.Next(36)]);

            return sb.ToString();
        }
    }
}