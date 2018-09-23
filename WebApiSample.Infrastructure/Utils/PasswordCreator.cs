using System;
using System.Security.Cryptography;

namespace WebApiSample.Infrastructure.Utils
{
    public static class PasswordCreator
    {
        public static string GetSalt()
        {
            const int saltSize = 16; // 128-bit
            var bytes = new byte[saltSize];

            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);

                return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
            }
        }

        public static string GetHash(string password, string salt)
        {
            return Hasher.GetSha256Hash(password + salt).Replace("-", string.Empty).ToLower();
        }

        public static bool CheckPassword(string password, string hashedPassword, string salt)
        {
            return GetHash(password, salt) == hashedPassword;
        }
    }
}
