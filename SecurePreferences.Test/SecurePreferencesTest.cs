using Moq;
using SecurePreferences.interfaces;

namespace SecurePreferences.Test
{
    public class SecurePreferencesTest
    {
        private readonly Mock<IPreferences> _mock;
        private readonly Mock<IEncryptionProvider> _provider;

        public SecurePreferencesTest()
        {
            _mock = new Mock<IPreferences>();
            _provider = new Mock<IEncryptionProvider>();
        }

        [Fact]
        public void ShouldSaveToPreferencesWhenGivenKeyAndValue()
        {
            // Given
            var encryptionKey = "aessupersecretkey";
            var storageKey = "test";
            var value = "value";

            _mock.Setup(x => x.Set(encryptionKey, It.IsAny<string>(), null));

            var securePreferences = new SecurePreferences(Encryption.AES, _mock.Object);

            // When
            securePreferences.Save(encryptionKey, storageKey, value);

            // Then
            _mock.Verify(x => x.Set(storageKey, It.IsAny<string>(), null), Times.Once);
        }

        [Fact]
        public void ShouldGetPreferencesWhenGivenKey()
        {
            // Given
            var decryptionKey = "aessupersecretkey";
            var storageKey = "test";
            var value = "value";

            _mock.Setup(x => x.Get<string?>(storageKey, null, null)).Returns(value);
            _provider.Setup(x => x.Decrypt(It.IsAny<string>(), decryptionKey)).Returns(value);

            var securePreferences = new SecurePreferences(_provider.Object, _mock.Object);

            // When
            var result = securePreferences.Get(decryptionKey, storageKey);

            // Then
            _mock.Verify(x => x.Get(storageKey, It.IsAny<string>(), null), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(value, result);
        }

        [Fact]
        public void ShouldVerifyPreferencesRemoveCalledWhenGivenStorageKey()
        {
            // Given
            var storageKey = "test";

            _mock.Setup(x => x.Remove(storageKey, null));

            var securePreferences = new SecurePreferences(_provider.Object, _mock.Object);

            // When
            securePreferences.Remove(storageKey);

            // Then
            _mock.Verify(x => x.Remove(storageKey, null), Times.Once);
        }

        [Fact]
        public void ShouldVerifyPreferencesClearCalledWhenGivenStorageKey()
        {
            // Given

            _mock.Setup(x => x.Clear(null));

            var securePreferences = new SecurePreferences(_provider.Object, _mock.Object);

            // When
            securePreferences.Clear();

            // Then
            _mock.Verify(x => x.Clear(null), Times.Once);
        }
    }
}
