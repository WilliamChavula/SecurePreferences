namespace SecurePreferences.Test
{
    public class AESKeyGeneratorTest
    {
        [Fact]
        public void ShouldGenerateAesKey()
        {
            // Given
            var keySizeInBits = 256;
            // When
            var result = AESKeyGenerator.ExportAesKey(keySizeInBits);
            // Then
            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldThrowArgumentExceptionWhenGivenInvalidKeySize()
        {
            // Given
            var keySizeInBits = 129;
            // When
            // Then
            Assert.Throws<ArgumentException>(() => AESKeyGenerator.ExportAesKey(keySizeInBits));
        }
    }
}
