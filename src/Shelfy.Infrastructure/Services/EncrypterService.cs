using System;
using System.Security.Cryptography;
using Shelfy.Infrastructure.Extensions;

namespace Shelfy.Infrastructure.Services
{
    public class EncrypterService : IEncrypterService
    {
        private static readonly int DeriveBytesIterationsCount = 10000;
        private static readonly int SaltSize = 40;

        public string GetSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var saltBytes = new byte[SaltSize];
                randomNumberGenerator.GetBytes(saltBytes);

                return Convert.ToBase64String(saltBytes);
            }
        }

        /// <summary>
        /// Getting safe hash, from password.
        /// More iteration make harder to brute force our hash.
        /// 10000 iteration its standard, hash will be generated in ~160ms. 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string GetHash(string password, string salt)
        {
            if (password.IsEmpty())
            {
                throw new ArgumentException("Can not generate hash for an empty password.", nameof(password));
            }

            if (salt.IsEmpty())
            {
                throw new ArgumentException("Can not generate hash for an empty salt.", nameof(salt));
            }
            
            using (var rfc2898 = new Rfc2898DeriveBytes(password, GetBytes(salt), DeriveBytesIterationsCount))
            {
                return Convert.ToBase64String(rfc2898.GetBytes(SaltSize));
            }
        }

        // Converting string to byte array
        private static byte[] GetBytes(string value)
        {
            var bytes = new byte[value.Length * sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }
    }
}
