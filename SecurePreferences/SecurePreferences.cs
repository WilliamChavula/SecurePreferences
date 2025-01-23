using SecurePreferences.interfaces;
using MicrosoftPreferences = Microsoft.Maui.Storage.Preferences;

namespace SecurePreferences
{
    public class SecurePreferences
    {
        private readonly IEncryptionProvider encryptionProvider;

        private IPreferences Preferences { get; }

        /// <summary>
        /// Initializes a new instance of the SecurePreferences class with the specified encryption key and <see cref="IEncryptionProvider" />.
        /// </summary>
        /// <param name="provider">The encryption provider to manage encryption operations.</param>
        /// <param name="preferences">An Optional implementation of IPreferences instance.</param>
        /// <remarks>
        /// If no <see cref="IPreferences" /> instance is provided, the default implementation will be used.
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the key is null or empty.</exception>
        public SecurePreferences(IEncryptionProvider provider, IPreferences? preferences = null)
        {
            if (preferences is null)
                Preferences = MicrosoftPreferences.Default;
            else
                Preferences = preferences;
            encryptionProvider = provider;
        }

        /// <summary>
        /// Saves a string value securely by encrypting it before storage.
        /// </summary>
        /// <param name="encryptionKey">The key used to encrypt the value.</param>
        /// <param name="storageKey">The key under which the value will be stored. Must be unique within the context of usage.</param>
        /// <param name="value">The value to be encrypted and saved.</param>
        /// <remarks>
        /// The encryption is performed using the provider and key set during the initialization of <see cref="SecurePreferences"/>.
        /// </remarks>
        public void Save(string encryptionKey, string storageKey, string value)
        {
            if (string.IsNullOrEmpty(storageKey))
                throw new ArgumentException("Encryption key cannot be null or empty");

            string encryptedValue = encryptionProvider.Encrypt(value, encryptionKey);
            Preferences.Set(storageKey, encryptedValue);
        }

        /// <summary>
        /// Retrieves and decrypts a previously saved string value from secure storage.
        /// </summary>
        /// <param name="decryptionKey">The key used to decrypt the stored value.</param>
        /// <param name="storageKey">The key associated with the stored value.</param>
        /// <returns>
        /// The decrypted string if the key exists and decryption succeeds; otherwise, returns null.
        /// </returns>
        /// <remarks>
        /// If the key does not exist, or if decryption fails internally, this method returns null.
        /// </remarks>
        public string? Get(string decryptionKey, string storageKey)
        {
            if (string.IsNullOrEmpty(storageKey))
                throw new ArgumentException("Encryption key cannot be null or empty");

            var encryptedValue = Preferences.Get<string?>(storageKey, null);
            if (encryptedValue == null)
                return null;

            return encryptionProvider.Decrypt(encryptedValue, decryptionKey);
        }

        /// <summary>
        /// Removes a specific preference from storage based on its key.
        /// </summary>
        /// <param name="storageKey">The key of the preference to remove.</param>
        /// <remarks>
        /// This operation is immediate and affects all instances sharing the same preference storage.
        /// </remarks>
        public void Remove(string storageKey) => Preferences.Remove(storageKey);

        /// <summary>
        /// Clears all preferences from storage.
        /// </summary>
        /// <remarks>
        /// This operation removes all stored preferences and should be used carefully as it affects all data stored through this class.
        /// </remarks>
        public void Clear() => Preferences.Clear();
    }
}
