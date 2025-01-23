using SecurePreferences.interfaces;
using MicrosoftPreferences = Microsoft.Maui.Storage.Preferences;

namespace SecurePreferences
{
    public class SecurePreferences
    {
        private readonly IEncryptionProvider encryptionProvider;
        private readonly string encryptionKey;

        private IPreferences Preferences { get; }

        /// <summary>
        /// Initializes a new instance of the SecurePreferences class with the specified encryption key and <see cref="IEncryptionProvider" />.
        /// </summary>
        /// <param name="key">The encryption key to use. Cannot be null or empty.</param>
        /// <param name="provider">The encryption provider to manage encryption operations.</param>
        /// <exception cref="ArgumentException">Thrown when the key is null or empty.</exception>
        public SecurePreferences(
            string key,
            IEncryptionProvider provider,
            IPreferences? preferences = null
        )
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Encryption key cannot be null or empty", nameof(key));

            if (preferences is null)
                Preferences = MicrosoftPreferences.Default;
            else
                Preferences = preferences;

            encryptionKey = key;
            encryptionProvider = provider;
        }

        /// <summary>
        /// Saves a string value securely by encrypting it before storage.
        /// </summary>
        /// <param name="key">The key under which the value will be stored. Must be unique within the context of usage.</param>
        /// <param name="value">The value to be encrypted and saved.</param>
        /// <remarks>
        /// The encryption is performed using the provider and key set during the initialization of <see cref="SecurePreferences"/>.
        /// </remarks>
        public void Save(string key, string value)
        {
            string encryptedValue = encryptionProvider.Encrypt(value, encryptionKey);
            Preferences.Set(key, encryptedValue);
        }

        /// <summary>
        /// Retrieves and decrypts a previously saved string value from secure storage.
        /// </summary>
        /// <param name="key">The key associated with the stored value.</param>
        /// <returns>
        /// The decrypted string if the key exists and decryption succeeds; otherwise, returns null.
        /// </returns>
        /// <remarks>
        /// If the key does not exist, or if decryption fails internally, this method returns null.
        /// </remarks>
        public string? Get(string key)
        {
            var encryptedValue = Preferences.Get<string?>(key, null);
            if (encryptedValue == null)
                return null;

            return encryptionProvider.Decrypt(encryptedValue, encryptionKey);
        }

        /// <summary>
        /// Removes a specific preference from storage based on its key.
        /// </summary>
        /// <param name="key">The key of the preference to remove.</param>
        /// <remarks>
        /// This operation is immediate and affects all instances sharing the same preference storage.
        /// </remarks>
        public void Remove(string key) => Preferences.Remove(key);

        /// <summary>
        /// Clears all preferences from storage.
        /// </summary>
        /// <remarks>
        /// This operation removes all stored preferences and should be used carefully as it affects all data stored through this class.
        /// </remarks>
        public void Clear() => Preferences.Clear();
    }
}
