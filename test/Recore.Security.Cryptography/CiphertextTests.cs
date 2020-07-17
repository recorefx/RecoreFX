using System;
using System.Security.Cryptography;
using Xunit;

namespace Recore.Security.Cryptography.Tests
{
    public class CiphertextTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            using (var sha256 = SHA256.Create())
            {
                Assert.Throws<ArgumentNullException>(
                    () => Ciphertext<SHA256>.Encrypt(null, Array.Empty<byte>(), sha256));

                Assert.Throws<ArgumentNullException>(
                    () => Ciphertext<SHA256>.Encrypt("hello", null, sha256));

                Assert.Throws<ArgumentNullException>(
                    () => Ciphertext<SHA256>.Encrypt("hello", Array.Empty<byte>(), null));
            }
        }

        // With this test, it's ok to use the short forms in the rest of the tests.
        [Fact]
        public void HelperMethods()
        {
            using (var md5 = MD5.Create())
            {
                var longForm = Ciphertext<MD5>.Encrypt("hello", Array.Empty<byte>(), md5);
                var inferAlgo = Ciphertext.Encrypt("hello", Array.Empty<byte>(), md5);
                var shortForm = Ciphertext.MD5("hello", Array.Empty<byte>());

                Assert.Equal(longForm.Value, inferAlgo.Value);
                Assert.Equal(longForm.Value, shortForm.Value);
            }

            using (var sha1 = SHA1.Create())
            {
                var longForm = Ciphertext<SHA1>.Encrypt("hello", Array.Empty<byte>(), sha1);
                var inferAlgo = Ciphertext.Encrypt("hello", Array.Empty<byte>(), sha1);
                var shortForm = Ciphertext.SHA1("hello", Array.Empty<byte>());

                Assert.Equal(longForm.Value, inferAlgo.Value);
                Assert.Equal(longForm.Value, shortForm.Value);
            }

            using (var sha256 = SHA256.Create())
            {
                var longForm = Ciphertext<SHA256>.Encrypt("hello", Array.Empty<byte>(), sha256);
                var inferAlgo = Ciphertext.Encrypt("hello", Array.Empty<byte>(), sha256);
                var shortForm = Ciphertext.SHA256("hello", Array.Empty<byte>());

                Assert.Equal(longForm.Value, inferAlgo.Value);
                Assert.Equal(longForm.Value, shortForm.Value);
            }
        }

        [Fact]
        public void ValueNoSalt()
        {
            using (var md5 = MD5.Create())
            {
                var ciphertext = Ciphertext.Encrypt("hello", Array.Empty<byte>(), md5);

                // This ciphertext was computed with the Bash command
                // printf "hello" | md5sum | awk '{print $1}' | xxd -r -p | base64
                var expected = "XUFAKrxLKna5cZ2REBfFkg==";
                Assert.Equal(expected, ciphertext.Value);
            }

            using (var sha1 = SHA1.Create())
            {
                var ciphertext = Ciphertext.Encrypt("hello", Array.Empty<byte>(), sha1);

                // This ciphertext was computed with the Bash command
                // printf "hello" | sha1sum | awk '{print $1}' | xxd -r -p | base64
                var expected = "qvTGHdzF6KLavt4PO0gs2a6pQ00=";
                Assert.Equal(expected, ciphertext.Value);
            }

            using (var sha256 = SHA256.Create())
            {
                var ciphertext = Ciphertext.Encrypt("hello", Array.Empty<byte>(), sha256);

                // This ciphertext was computed with the Bash command
                // printf "hello" | sha256sum | awk '{print $1}' | xxd -r -p | base64
                var expected = "LPJNul+wow4m6DsqxbninhsWHlwfp0JecwQzYpOLmCQ=";
                Assert.Equal(expected, ciphertext.Value);
            }
        }

        [Fact]
        public void GetHashCode_()
        {
            var ciphertext = Ciphertext.SHA256("hello", Array.Empty<byte>());

            // This ciphertext was computed with the Bash command
            // printf "hello" | sha256sum | awk '{print $1}' | xxd -r -p | base64
            var expected = "LPJNul+wow4m6DsqxbninhsWHlwfp0JecwQzYpOLmCQ=";
            Assert.Equal(expected.GetHashCode(), ciphertext.GetHashCode());
        }

        [Fact]
        public void EqualityOperators()
        {
            // Equal objects
            var ciphertext1 = Ciphertext.SHA256("hello", Array.Empty<byte>());
            var ciphertext2 = Ciphertext.SHA256("hello", Array.Empty<byte>());

            Assert.True(
                ciphertext1 == ciphertext2);
            Assert.False(
                ciphertext1 != ciphertext2);

            // Unequal objects
            var differentPlaintext = Ciphertext.SHA256("world", Array.Empty<byte>());
            Assert.False(
                ciphertext1 == differentPlaintext);
            Assert.True(
                ciphertext1 != differentPlaintext);

            var withSalt = Ciphertext.SHA256("hello", new byte[] { 0x01, 0x02, 0x03 });
            Assert.False(
                ciphertext1 == withSalt);
            Assert.True(
                ciphertext1 != withSalt);
        }
    }
}