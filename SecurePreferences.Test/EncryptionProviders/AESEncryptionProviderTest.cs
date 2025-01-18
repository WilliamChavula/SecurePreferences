using System.Diagnostics;

namespace SecurePreferences.Test.EncryptionProviders
{
    public class AESEncryptionProviderTest
    {
        public static string Key => "5a46d6975d51764f10eda8fe266aa599352cd6ae";
        public static string StringToEncrypt => "stringToEncrypt";

        public class EncryptionTestsGroup
        {
            [Fact]
            public void ShouldEncryptPlainTextUsingAESAlgorithm()
            {
                // Given
                var aes = Encryption.AES;

                // When
                var encrypted = aes.Encrypt(StringToEncrypt, Key);

                // Then
                Assert.NotEqual(encrypted, StringToEncrypt);
            }

            [Theory]
            [InlineData("")]
            [InlineData("df4f5d529bced76")]
            public void ShouldFailToEncryptWithArgumentOutOfRangeException(string key)
            {
                // Given
                var aes = Encryption.AES;
                var stringToEncrypt = "stringToEncrypt";

                // Then
                Assert.Throws<ArgumentOutOfRangeException>(() => aes.Encrypt(stringToEncrypt, key));
            }

            [Theory]
            [InlineData(null, "5a46d6975d51764f10eda8fe266aa599352cd6ae")]
            public void ShouldThrowArgumentExceptionGivenNullPlainText(string plainText, string key)
            {
                // Given
                var aes = Encryption.AES;

                // Then
                Assert.Throws<ArgumentException>(() => aes.Encrypt(plainText, key));
            }

            [Theory]
            [InlineData(null)]
            public void ShouldFailToEncryptWithArgumentException(string key)
            {
                // Given
                var aes = Encryption.AES;
                var stringToEncrypt = "stringToEncrypt";

                // Then
                Assert.Throws<ArgumentNullException>(() => aes.Encrypt(stringToEncrypt, key));
            }
        }

        public class DecryptionTestsGroup
        {
            [Fact]
            public void ShouldCorrectlyDecryptCipherTextToOriginalInput()
            {
                // Given
                var aes = Encryption.AES;

                // When
                var encrypted = aes.Encrypt(StringToEncrypt, Key);
                var decrypted = aes.Decrypt(encrypted, Key);

                // Then
                Assert.Equal(decrypted, StringToEncrypt);
            }

            [Fact]
            public void ShouldFailToDecryptCipherTextToOriginalInputWhenUsingWrongKey()
            {
                // Given
                var aes = Encryption.AES;
                var wrongKey = "ae65fb42c9b261e10729f94443e595fc91fdc10a";
                var exceptionMsg =
                    "Decryption failed, likely due to incorrect key or corrupted data.";

                // When
                var encrypted = aes.Encrypt(StringToEncrypt, Key);

                // Then
                var exception = Assert.Throws<ArgumentException>(
                    () => aes.Decrypt(encrypted, wrongKey)
                );
                Assert.Equal(exceptionMsg, exception.Message);
            }

            [Theory]
            [InlineData(
                null,
                "ae65fb42c9b261e10729f94443e595fc91fdc10a",
                "Ciphertext cannot be null or empty."
            )]
            [InlineData("gibberish", null, "Key cannot be null or empty.")]
            [InlineData(
                "gibberish",
                "ae65fb42c9b261e10729f94443e595fc91fdc10a",
                "Ciphertext is not a valid Base64 string."
            )]
            public void ShouldThrowArgumentExceptionGivenNullCipherText(
                string cipherText,
                string key,
                string errorMsg
            )
            {
                // Given
                var aes = Encryption.AES;

                // Then
                var exception = Assert.Throws<ArgumentException>(
                    () => aes.Decrypt(cipherText, key)
                );
                Assert.StartsWith(errorMsg, exception.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
