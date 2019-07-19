using System;

namespace Recore.Security.Cryptography
{
    /// <summary>
    /// Provides methods for securely comparing objects.
    /// </summary>
    public static class SecureCompare
    {
        /// <summary>
        /// Checks two byte arrays for equality without early termination.
        /// </summary>
        /// <remarks>
        /// This method is used to guard against <b>timing attacks</b>.
        ///
        /// When checking untrusted input against a secret,
        /// using a regular element-by-element equality method
        /// such as <c>String.Equals</c> is insecure.
        /// For example, suppose you are checking whether an incoming request's signature
        /// matches what you expect.
        /// In this case, you hash the request payload with your own private key
        /// and compare that to the actual signature.
        /// If the comparison stops at the first unmatched element in the sequence,
        /// an attacker can time the comparison with a high-resolution timer
        /// and infer how many elements they guessed correctly.
        ///
        /// This method assumes that the length of the sequences are equal,
        /// such as two strings processed by a hashing algorithm.
        /// If the length of the sequence is considered a secret,
        /// this method should not be used
        /// as it will leak that information in a timing attack.
        /// </remarks>
        public static bool SafeEquals(byte[] lhs, byte[] rhs)
        {
            if (lhs == null)
            {
                throw new ArgumentNullException(nameof(lhs));
            }

            if (rhs == null)
            {
                throw new ArgumentNullException(nameof(rhs));
            }

            if (lhs.Length != rhs.Length)
            {
                return false;
            }

            // We can't use conditional branches here
            // since that will change the execution time depending on the secret
            bool areEqual = true;
            for (int i = 0; i < lhs.Length; i++)
            {
                areEqual &= (lhs[i] == rhs[i]);
            }

            return areEqual;
        }
    }
}