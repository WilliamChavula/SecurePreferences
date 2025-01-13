using System.Security.Cryptography;
using System.Text;
using SecurePreferences.interfaces;

namespace SecurePreferences.EncryptionProviders
{
    class RSAEncryptionProvider : IRSAEncryptionProvider
    {
        private readonly RSA rsa;

        /// <summary>
        /// Initializes a new instance of the <see cref="RSAEncryptionProvider"/> class with the specified key size.
        /// </summary>
        /// <param name="keySize">The size of the RSA key in bits. Defaults to 2048 bits.
        /// <param name="keySize"> Must be between 2048 and 16384 bits.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the key size is not within the acceptable range for RSA.</exception>
        public RSAEncryptionProvider(int keySize = 2048)
        {
            if (keySize < 2048 || keySize > 16384)
                throw new ArgumentOutOfRangeException(
                    nameof(keySize),
                    "Key size must be between 2048 and 16384 bits."
                );
            rsa = RSA.Create();
            rsa.KeySize = keySize;
        }

        /// <summary>
        /// Exports the public key of the RSA instance to a Base64 string.
        /// </summary>
        /// <returns>A Base64 string representation of the public key.</returns>
        public string ExportPublicKey() => Convert.ToBase64String(rsa.ExportRSAPublicKey());

        /// <summary>
        /// Exports the private key of the RSA instance to a Base64 string.
        /// </summary>
        /// <returns>A Base64 string representation of the private key.</returns>
        public string ExportPrivateKey() => Convert.ToBase64String(rsa.ExportRSAPrivateKey());

        /// <summary>
        /// Decrypts the given Base64 encoded cipher text using the provided private key.
        /// </summary>
        /// <param name="cipherText">The Base64 string of the encrypted data to decrypt.</param>
        /// <param name="key">The Base64 string representation of the private key used for decryption.</param>
        /// <returns>The decrypted string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cipherText"/> or <paramref name="key"/> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if decryption fails due to cryptographic issues.</exception>
        public string Decrypt(string cipherText, string key)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(
                    nameof(cipherText),
                    "Cipher Text cannot be null here."
                );

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "key cannot be null here.");

            try
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(key), out _);
                byte[] encryptedBytes = Convert.FromBase64String(cipherText);

                // Decrypt the data
                byte[] decryptedData = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (CryptographicException ce)
            {
                throw new InvalidOperationException($"Failed to decrypt due to {ce.Message}", ce);
            }
        }

        /// <summary>
        /// Encrypts the given string value using the provided public key.
        /// </summary>
        /// <param name="value">The string to encrypt.</param>
        /// <param name="key">The Base64 string representation of the public key used for encryption.</param>
        /// <returns>A Base64 string of the encrypted data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="key"/> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if encryption fails due to cryptographic issues.</exception>
        public string Encrypt(string value, string key)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value), "value cannot be null here.");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "key cannot be null here.");

            try
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(key), out _);
                byte[] dataBytes = Encoding.UTF8.GetBytes(value);

                // Encrypt the data
                byte[] encryptedData = rsa.Encrypt(dataBytes, RSAEncryptionPadding.OaepSHA256);

                return Convert.ToBase64String(encryptedData);
            }
            catch (CryptographicException ce)
            {
                throw new InvalidOperationException($"Failed to encrypt due to {ce.Message}", ce);
            }
        }
    }
}
