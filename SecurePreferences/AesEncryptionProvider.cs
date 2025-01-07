using System.Security.Cryptography;
using System.Text;
using SecurePreferences.interfaces;

namespace SecurePreferences
{
    public class AesEncryptionProvider : IEncryptionProvider
    {
        /// <summary>
        /// Decrypts a Base64 encoded string that was encrypted with AES.
        /// </summary>
        /// <param name="cipherText">A Base64 encoded string containing the IV and encrypted data.</param>
        /// <param name="key">The decryption key as a string, which will be converted to bytes internally.</param>
        /// <returns>The decrypted string.</returns>
        /// <exception cref="ArgumentException">Thrown if the ciphertext or key is null or empty, or if the ciphertext format is incorrect.</exception>
        /// <exception cref="CryptographicException">Thrown if decryption fails due to incorrect key or corrupted data.</exception>

        public string Decrypt(string cipherText, string key)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException(
                    "Ciphertext cannot be null or empty.",
                    nameof(cipherText)
                );

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            using var aes = Aes.Create();
            aes.Key = GetKeyBytes(key);

            byte[] combinedBytes;
            try
            {
                combinedBytes = Convert.FromBase64String(cipherText);
            }
            catch (FormatException)
            {
                throw new ArgumentException(
                    "Ciphertext is not a valid Base64 string.",
                    nameof(cipherText)
                );
            }

            // Extract IV and ciphertext
            int ivLength = aes.IV.Length;

            if (combinedBytes.Length < ivLength)
                throw new ArgumentException(
                    "Ciphertext is too short to contain IV.",
                    nameof(cipherText)
                );

            byte[] iv = new byte[ivLength];
            byte[] cipherBytes = new byte[combinedBytes.Length - iv.Length];
            Array.Copy(combinedBytes, 0, iv, 0, iv.Length);
            Array.Copy(combinedBytes, iv.Length, cipherBytes, 0, cipherBytes.Length);

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            try
            {
                byte[] plainBytes = decryptor.TransformFinalBlock(
                    cipherBytes,
                    0,
                    cipherBytes.Length
                );

                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (CryptographicException ex)
            {
                throw new ArgumentException(
                    "Decryption failed, likely due to incorrect key or corrupted data.",
                    ex
                );
            }
        }

        /// <summary>
        /// Encrypts the provided plaintext string using AES encryption with the given key.
        /// </summary>
        /// <param name="plainText">The string to be encrypted.</param>
        /// <param name="key">The encryption key as a string.</param>
        /// <returns>A Base64 encoded string.</returns>
        /// <exception cref="ArgumentException">Thrown if the plaintext or key is null or empty, or if the key length is invalid.</exception>
        public string Encrypt(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException(
                    "Plaintext cannot be null or empty.",
                    nameof(plainText)
                );

            using var aes = Aes.Create();

            var byteKey = GetKeyBytes(key);

            if (
                byteKey == null
                || (byteKey.Length != 16 && byteKey.Length != 24 && byteKey.Length != 32)
            )
                throw new ArgumentException(
                    "Key provided is too short. Provide a longer key to complete the operation.",
                    nameof(key)
                );

            aes.Key = byteKey;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Combine IV and ciphertext
            byte[] combinedBytes = new byte[aes.IV.Length + cipherBytes.Length];
            Array.Copy(aes.IV, 0, combinedBytes, 0, aes.IV.Length);
            Array.Copy(cipherBytes, 0, combinedBytes, aes.IV.Length, cipherBytes.Length);

            return Convert.ToBase64String(combinedBytes);
        }

        /// <summary>
        /// Encrypts the provided plaintext string using AES encryption with the given key.
        /// The method generates a new IV for each encryption operation, which is combined
        /// with the ciphertext for later decryption.
        /// </summary>
        /// <param name="plainText">The string to be encrypted.</param>
        /// <param name="key">The encryption key as a byte[].</param>
        /// <returns>A Base64 encoded string.</returns>
        /// <exception cref="ArgumentException">Thrown if the plaintext or key is null or empty, or if the key length is invalid.</exception>
        public string Encrypt(string plainText, byte[] key)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException(
                    "Plaintext cannot be null or empty.",
                    nameof(plainText)
                );

            if (key == null || (key.Length != 16 && key.Length != 24 && key.Length != 32))
                throw new ArgumentException("Key must be 16, 24, or 32 bytes long.", nameof(key));

            using var aes = Aes.Create();
            aes.Key = key;

            // Generate a new IV for each encryption
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Combine IV and ciphertext
            byte[] combinedBytes = new byte[aes.IV.Length + cipherBytes.Length];
            Array.Copy(aes.IV, 0, combinedBytes, 0, aes.IV.Length);
            Array.Copy(cipherBytes, 0, combinedBytes, aes.IV.Length, cipherBytes.Length);

            return Convert.ToBase64String(combinedBytes);
        }

        /// <summary>
        /// Converts a string key into a byte array suitable for AES encryption, ensuring the key length matches one of the AES key sizes.
        /// </summary>
        /// <param name="key">The key as a string to be converted to bytes.</param>
        /// <returns>A byte array representing the key, truncated or extended to match AES key sizes (16, 24, or 32 bytes).</returns>
        /// <exception cref="ArgumentException">Thrown if the key is null or empty.</exception>
        private static byte[] GetKeyBytes(string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // Determine the appropriate key size
            if (keyBytes.Length >= 32)
                return keyBytes[..32]; // Use first 32 bytes for 256-bit key
            else if (keyBytes.Length >= 24)
                return keyBytes[..24]; // Use first 24 bytes for 192-bit key
            else
                return keyBytes[..16]; // Use first 16 bytes for 128-bit key or fewer
        }
    }
}
