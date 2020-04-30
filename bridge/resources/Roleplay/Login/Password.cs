using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;


namespace Roleplay.Login
{
    public static class Password
    {
        private const int SALT_SIZE = 24;
        private const int HASH_SIZE = 30;
        public const int PBKDF2_ITERATIONS = 100000;

        public static string CreateSalt()
        {
            byte[] salt = new byte[SALT_SIZE];
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider()) {
                provider.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        public static string CreateHash(string pass, string salt, int iterations)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(pass, Convert.FromBase64String(salt), iterations, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(HASH_SIZE));
            }
        }

        public static bool Compare(string pass, string hash, string salt, int iterations)
        {
            string pwHash = CreateHash(pass, salt, iterations);
            return pwHash.Equals(hash);
        }
    }
}
