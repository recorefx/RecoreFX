using System;

using Recore.Properties;

namespace Recore
{
    /// <summary>
    /// Represents a non-null, non-empty string value where whitespace is not allowed.
    /// </summary>
    /// <remarks>
    /// This type is meant to feel like a subclass of <c>String</c>, which is sealed.
    /// </remarks>
    public sealed class Token : IEquatable<Token>, IComparable<Token>, IEquatable<string>, IComparable<string>
    {
        // Guaranteed to be non-null, nonzero length, and no whitespace
        private readonly string value;

        /// <summary>
        /// Constructs an instance of <c cref="Token">Token</c> from a string value.
        /// </summary>
        public Token(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value == string.Empty)
            {
                throw new ArgumentException(Resources.TokenEmpty);
            }

            foreach (char c in value)
            {
                if (char.IsWhiteSpace(c))
                {
                    throw new ArgumentException(string.Format(Resources.TokenWhitespace, value));
                }
            }

            this.value = value;
        }

        /// <summary>
        /// Returns the underlying string value.
        /// </summary>
        public override string ToString() => value;

        /// <summary>
        /// Determines whether this instance and another object,
        /// which must be a <c cref="Token">Token</c> or a <c>String</c>,
        /// have the same value.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Token token)
            {
                return this.Equals(token);
            }
            else if (obj is string str)
            {
                return this.Equals(str);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether this instance and another <c cref="Token">Token</c>
        /// have the same value.
        /// </summary>
        public bool Equals(Token other) => value == other.value;

        /// <summary>
        /// Determines whether this instance and a <c>String</c>
        /// have the same value.
        /// </summary>
        public bool Equals(string other) => value.Equals(other);

        /// <summary>
        /// Determines whether two instances of <c cref="Token">Token</c>
        /// have the same value.
        /// </summary>
        public static bool operator ==(Token lhs, Token rhs) => Equals(lhs, rhs);

        /// <summary>
        /// Determines whether two instances of <c cref="Token">Token</c>
        /// have different values.
        /// </summary>
        public static bool operator !=(Token lhs, Token rhs) => !Equals(lhs, rhs);

        /// <summary>
        /// Returns the hash code of the underlying value.
        /// </summary>
        public override int GetHashCode() => value.GetHashCode();

        /// <summary>
        /// Compares this instance with a specified <c>Token</c> object
        /// and indicates whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified object.
        /// </summary>
        public int CompareTo(Token other) => value.CompareTo(other.value);

        /// <summary>
        /// Compares this instance with a specified <c>String</c> object
        /// and indicates whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified object.
        /// </summary>
        public int CompareTo(string other) => value.CompareTo(other);

        /// <summary>
        /// Converts this instance to its underlying value.
        /// </summary>
        public static implicit operator string(Token t) => t.ToString();

        // Implicit conversion to string gives us the string == and != operators for free
        // For some reason, String implements IComparable but does not have comparison operators
    }

    /// <summary>
    /// Provides additional methods for working with strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Splits a string into a sequence of tokens on its whitespace characters.
        /// </summary>
        /// <remarks>
        /// While a particular string may consist of tokens delimited by some other character or string,
        /// this method does not provide an option for this by design.
        /// This is consistent with the <c cref="Token">Token</c> type itself,
        /// which does not check for any characters besides whitespace.
        /// </remarks>
        public static Token[] Tokenize(this string str)
        {
            var parts = str.Split(separator: (char[])null, StringSplitOptions.RemoveEmptyEntries);
            var tokens = new Token[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                tokens[i] = new Token(parts[i]);
            }

            return tokens;
        }
    }
}