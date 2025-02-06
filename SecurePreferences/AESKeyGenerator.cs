using System.Security.Cryptography;
using SecurePreferences.interfaces;

namespace SecurePreferences
{
    public static class AESKeyGenerator
    {
        static byte[] GenerateAesKey(int keySizeInBits = 256)
        {
            if (keySizeInBits != 128 && keySizeInBits != 192 && keySizeInBits != 256)
            {
                throw new ArgumentException(
                    "Key size must be 128, 192, or 256 bits.",
                    nameof(keySizeInBits)
                );
            }

            using Aes aesAlg = Aes.Create();
            aesAlg.KeySize = keySizeInBits;
            aesAlg.GenerateKey();
            return aesAlg.Key;
        }

        /// <summary>
        /// Generates a cryptographically random AES key of a specified size and returns it as a Base64 encoded string.
        /// </summary>
        /// <param name="keySizeInBits">The desired key size in bits.  Must be 128, 192, or 256.  256 is recommended for strong security.</param>
        /// <returns>A Base64 encoded string representing the generated AES key.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided key size is not 128, 192, or 256.</exception>
        public static string ExportAesKey(int keySizeInBits = 256)
        {
            byte[] key = GenerateAesKey(keySizeInBits);
            return Convert.ToBase64String(key);
        }
    }
}
