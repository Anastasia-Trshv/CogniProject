using Cogni.Authentication.Abstractions;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Cogni.Authentication
{
    public class PasswordHasher :IPasswordHasher

    {
        const int keySize = 64; //длина соли
        const int iterations = 350000; //количество итераций хэширования
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public string HashPassword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                 salt,
                 iterations,
                 hashAlgorithm,
                 keySize);
            return Convert.ToHexString(hash);
        }

        public bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));

        }
    }
}
