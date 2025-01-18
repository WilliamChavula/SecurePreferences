namespace SecurePreferences.Test.EncryptionProviders
{
    public class RSAFixture
    {
        public string StringToEncrypt { get; init; }

        public RSAFixture()
        {
            StringToEncrypt = "stringToEncrypt";
        }
    }

    public class RSAEncryptionProviderTest
    {
        public class RSAEncryptionTests(RSAFixture rSAFixture) : IClassFixture<RSAFixture>
        {
            [Fact]
            public void ShouldEncryptPlainTextUsingRSAAlgorithm()
            {
                // Given
                var rsaInstance = Encryption.RSA;
                var pubKey = rsaInstance.ExportPublicKey();

                // When
                var encrypted = rsaInstance.Encrypt(rSAFixture.StringToEncrypt, pubKey);

                // Then
                Assert.NotEmpty(pubKey);
                Assert.NotEmpty(rSAFixture.StringToEncrypt);
                Assert.NotEqual(rSAFixture.StringToEncrypt, encrypted);
            }

            [Theory]
            [InlineData(null, "ae65fb42c9b261", "value cannot be null here.")]
            [InlineData("gibberish", null, "key cannot be null here.")]
            public void ShouldThrowArgumentExceptionGivenNullParameters(
                string value,
                string key,
                string errorMsg
            )
            {
                // Given
                var rsaInstance = Encryption.RSA;
                var pubKey = rsaInstance.ExportPublicKey();

                // When & Then
                var exception = Assert.Throws<ArgumentNullException>(
                    () => rsaInstance.Encrypt(value, key)
                );
                Assert.Contains(errorMsg, exception.Message);
            }

            [Fact]
            public void ShouldThrowInvalidOperationExceptionWhenEncryptingWithNonRSAKey()
            {
                // Given
                var rsaInstance = Encryption.RSA;
                var nonRSAKey = "ae65fb42c9b261e10729f94443e595fc91fdc10a";
                var value = "gibberish";

                // When & Then
                var exception = Assert.Throws<InvalidOperationException>(
                    () => rsaInstance.Encrypt(value, nonRSAKey)
                );
                Assert.Contains("Failed to encrypt", exception.Message);
            }
        }

        public class RSADecryptionTests(RSAFixture rSAFixture) : IClassFixture<RSAFixture>
        {
            [Fact]
            public void ShouldDecryptCipherTextUsingRSAAlgorithm()
            {
                // Given
                var rsaInstance = Encryption.RSA;
                var pubKey = rsaInstance.ExportPublicKey();
                var privateKey = rsaInstance.ExportPrivateKey();
                var encrypted = rsaInstance.Encrypt(rSAFixture.StringToEncrypt, pubKey);

                // When
                var decrypted = rsaInstance.Decrypt(encrypted, privateKey);

                // Then
                Assert.NotEmpty(pubKey);
                Assert.NotEmpty(privateKey);
                Assert.Equal(rSAFixture.StringToEncrypt, decrypted);
            }

            [Theory]
            [InlineData(null, "ae65fb42c9b261", "Cipher Text cannot be null here.")]
            [InlineData("gibberish", null, "key cannot be null here.")]
            public void ShouldThrowArgumentExceptionGivenNullParameters(
                string value,
                string key,
                string errorMsg
            )
            {
                // Given
                var rsaInstance = Encryption.RSA;
                var pubKey = rsaInstance.ExportPublicKey();
                var privateKey = rsaInstance.ExportPrivateKey();
                var encrypted = rsaInstance.Encrypt(rSAFixture.StringToEncrypt, pubKey);

                // When & Then
                var exception = Assert.Throws<ArgumentNullException>(
                    () => rsaInstance.Decrypt(value, key)
                );
                Assert.Contains(errorMsg, exception.Message);
            }

            [Fact]
            public void ShouldThrowInvalidOperationExceptionWhenDecryptingWithNonRSAKey()
            {
                // Given
                var rsaInstance = Encryption.RSA;
                var nonRSAKey = "ae65fb42c9b261e10729f94443e595fc91fdc10a";
                var pubKey = rsaInstance.ExportPublicKey();
                var encrypted = rsaInstance.Encrypt(rSAFixture.StringToEncrypt, pubKey);

                // When & Then
                var exception = Assert.Throws<InvalidOperationException>(
                    () => rsaInstance.Decrypt(encrypted, nonRSAKey)
                );

                Assert.Contains("Failed to decrypt", exception.Message);
            }
        }
    }
}
