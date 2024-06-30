using System;
using System.Security.Cryptography;
using System.Text;

namespace XRoute.Utilities
{
    public static class PasswordHelper
    {
        public static string CreateSalt(int size = 32)
        {
            byte[] salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                byte[] hash = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            string hash = HashPassword(password, storedSalt);
            return hash == storedHash;
        }
    }
}
