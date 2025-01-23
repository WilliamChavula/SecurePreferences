namespace SecurePreferences.interfaces
{
    public interface IEncryptionProvider
    {
        /// <summary>
        /// Encrypts the provided plaintext string using an encryption algorithm with the given key.
        /// </summary>
        /// <param name="plainText">The string to be encrypted.</param>
        /// <param name="key">The encryption key as a string.</param>
        /// <returns>A Base64 encoded string.</returns>
        /// <exception cref="ArgumentException">Thrown if the plaintext or key is null or empty, or if the key length is invalid.</exception>
        string Encrypt(string plainText, string key);

        /// <summary>
        /// Decrypts a Base64 encoded string that was encrypted with an encryption algorithm.
        /// </summary>
        /// <param name="cipherText">A Base64 encoded string containing the IV and encrypted data.</param>
        /// <param name="key">The decryption key as a string, which will be converted to bytes internally.</param>
        /// <returns>The decrypted string.</returns>
        /// <exception cref="ArgumentException">Thrown if the ciphertext or key is null or empty, or if the ciphertext format is incorrect.</exception>
        string Decrypt(string cipherText, string key);
    }
}
