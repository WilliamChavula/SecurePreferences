using SecurePreferences.interfaces;
using MicrosoftPreferences = Microsoft.Maui.Storage.Preferences;

namespace SecurePreferences
{
    public class SecurePreferences
    {
        private readonly IEncryptionProvider encryptionProvider;
        private readonly string encryptionKey;

        private static IPreferences Preferences => MicrosoftPreferences.Default;

        public SecurePreferences(string key, IEncryptionProvider provider)
        {
            if (string.IsNullOrEmpty(key) || key.Length < 16)
                throw new ArgumentException("Encryption key must be at least 16 characters long.");
            encryptionKey = key;
            encryptionProvider = provider;
        }

        public void Save(string key, string value)
        {
            string encryptedValue = encryptionProvider.Encrypt(value, encryptionKey);
            Preferences.Set(key, encryptedValue);
        }

        public string? Get(string key)
        {
            var encryptedValue = Preferences.Get<string?>(key, null);
            if (encryptedValue == null)
                return null;

            return encryptionProvider.Decrypt(encryptedValue, encryptionKey);
        }

        public static void Remove(string key) => Preferences.Remove(key);

        public static void Clear() => Preferences.Clear();
    }
}
