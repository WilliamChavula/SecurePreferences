using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecurePreferences.interfaces;
using Xunit;

namespace SecurePreferences.Test
{
    public class EncryptionTest
    {
        [Fact]
        public void ShouldBeCorrestInstanceOfAESEncryptionProvider()
        {
            var aesEncryptionProvider = Encryption.AES;
            Assert.IsAssignableFrom<IEncryptionProvider>(aesEncryptionProvider);
        }

        [Fact]
        public void ShouldBeCorrestInstanceOfRSAEncryptionProvider()
        {
            var rsaEncryptionProvider = Encryption.RSA;
            Assert.IsAssignableFrom<IRSAEncryptionProvider>(rsaEncryptionProvider);
        }
    }
}
