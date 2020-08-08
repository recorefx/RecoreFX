using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Recore.Security.Cryptography
{
    /// <summary>
    /// A strongly-typed representation of a string passed through a cryptographic hash function.
    /// </summary>
    /// <remarks>
    /// Use this type on fields that you want to be sure get encrypted.
    /// The .NET type system will make it impossible for a plaintext string to be assigned to that field.
    /// For example, if your application is storing users' passwords in a database,
    /// you could use this type for the .NET representation of the stored passwords.
    /// </remarks>
    public sealed class Ciphertext<THash> : Of<string> where THash : HashAlgorithm
    {
        // For deserialization with System.Text.Json
        private Ciphertext()
        {
        }

        private Ciphertext(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Hashes a plaintext string to create an instance of <see cref="Ciphertext{THash}"/>.
        /// </summary>
        /// <param name="plaintext">The string to encrypt.</param>
        /// <param name="salt">
        /// A cryptographic salt to append to the plaintext.
        /// This is used to protect the hashing algorithm from being broken by a rainbow table.
        /// However, it cannot protect easily guessed plaintexts.
        /// </param>
        /// <param name="hash">An instance of the hashing algorithm to apply to the plaintext.</param>
        /// <remarks>
        /// <see cref="Ciphertext{THash}"/> uses a factory method because hashing can be an expensive operation.
        /// In the future, this operation will be asynchronous.
        /// The <paramref name="hash"/> parameter is needed because there is no generic way to create an instance of hashing algorithm.
        /// The extension methods in <see cref="Ciphertext"/> fill in this parameter
        /// for their respective hashing algorithms.
        /// Those methods should be preferred to this one unless finer-grained control is needed.
        /// </remarks>
        public static Ciphertext<THash> Encrypt(string plaintext, byte[] salt, THash hash)
        {
            if (plaintext is null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            if (salt is null)
            {
                throw new ArgumentNullException(nameof(salt));
            }

            if (hash is null)
            {
                throw new ArgumentNullException(nameof(hash));
            }

            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            var saltedPlaintextBytes = plaintextBytes.Concat(salt).ToArray();
            var hashBytes = hash.ComputeHash(saltedPlaintextBytes);
            return new Ciphertext<THash>(Convert.ToBase64String(hashBytes));
        }
    }

    /// <summary>
    /// Provides helper methods for working with <see cref="Ciphertext{THash}"/>.
    /// </summary>
    /// <remarks>
    /// This type exists because of constraints with generics in .NET.
    /// Implementations of <see cref="HashAlgorithm"/> are conventionally created through a
    /// factory method like <see cref="HashAlgorithm.Create()"/>.
    /// However, you can't call a static method on a type parameter.
    /// </remarks>
    public static class Ciphertext
    {
        /// <summary>
        /// Hashes a plaintext string to create an instance of <see cref="Ciphertext{THash}"/>.
        /// </summary>
        /// <remarks>
        /// This helper method provides type inference for <see cref="Ciphertext{THash}.Encrypt(string, byte[], THash)"/>.
        /// Use it when you want to use a hashing algorithm that isn't provided by one of the other helpers.
        /// </remarks>
        public static Ciphertext<THash> Encrypt<THash>(string plaintext, byte[] salt, THash hash) where THash : HashAlgorithm
        {
            return Ciphertext<THash>.Encrypt(plaintext, salt, hash);
        }

        /// <summary>
        /// Encrypts the plaintext with the MD5 hashing algorithm.
        /// </summary>
        public static Ciphertext<MD5> MD5(string plaintext, byte[] salt)
        {
            using (var hash = System.Security.Cryptography.MD5.Create())
            {
                return Ciphertext<MD5>.Encrypt(plaintext, salt, hash);
            }
        }

        /// <summary>
        /// Encrypts the plaintext with the SHA1 hashing algorithm.
        /// </summary>
        public static Ciphertext<SHA1> SHA1(string plaintext, byte[] salt)
        {
            using (var hash = System.Security.Cryptography.SHA1.Create())
            {
                return Ciphertext<SHA1>.Encrypt(plaintext, salt, hash);
            }
        }

        /// <summary>
        /// Encrypts the plaintext with the SHA256 hashing algorithm.
        /// </summary>
        public static Ciphertext<SHA256> SHA256(string plaintext, byte[] salt)
        {
            using (var hash = System.Security.Cryptography.SHA256.Create())
            {
                return Ciphertext<SHA256>.Encrypt(plaintext, salt, hash);
            }
        }
    }
}
