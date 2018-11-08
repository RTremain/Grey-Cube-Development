using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GCDGameStore.Classes
{
    public static class AccountHashing
    {
        // byte array of base 64 string "0d3nuW96aA65mhWOwi7RMQ=="
        private static readonly IReadOnlyList<byte> pepper = new byte[16] { 0xD1, 0xDD, 0xE7, 0xB9, 0x6F, 0x7A, 0x68, 0x0E, 0xB9, 0x9A, 0x15, 0x8E, 0xC2, 0x2E, 0xD1, 0x31 };

        /// <summary>
        ///     Returns randomly generated 128-bit salt.
        /// </summary>
        /// <returns>byte[] form of a salt</returns>
        public static byte[] GenSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            return salt;
        }

        /// <summary>
        ///     Hashes a password with the application pepper concatenated as a suffix to the salt
        /// </summary>
        /// <param name="password">Password entered by user</param>
        /// <param name="salt">Salt associated with account</param>
        /// <returns>
        ///     base 64 string of resulting hash
        /// </returns>
        public static string GenHash(string password, byte[] salt)
        {
            byte[] pepperArray = pepper.ToArray<byte>();

            byte[] newSalt = new byte[pepperArray.Length + salt.Length];
            salt.CopyTo(newSalt, 0);
            pepperArray.CopyTo(newSalt, salt.Length);



            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: newSalt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
