using SecurePreferences.EncryptionProviders;
using SecurePreferences.interfaces;

namespace SecurePreferences
{
    public static class Encryption
    {
        /// <summary>
        /// Gets an instance of the AES encryption provider.
        /// </summary>
        /// <returns>An instance of <see cref="AESEncryptionProvider"/>.</returns>
        public static IEncryptionProvider AES => new AESEncryptionProvider();

        /// <summary>
        /// Gets an instance of the RSA encryption provider.
        /// </summary>
        /// <returns>An instance of <see cref="RSAEncryptionProvider"/>.</returns>
        public static IRSAEncryptionProvider RSA => new RSAEncryptionProvider();
    }
}
