using System;

using Recore.Properties;

namespace Recore
{
    /// <summary>
    /// Represents a non-null, non-empty string value where whitespace is not allowed.
    /// </summary>
    public sealed class Token : IEquatable<Token>, IComparable<Token>
    {
        /// <summary>
        /// The value of the token.
        /// </summary>
        /// <remarks>
        /// This is guaranteed to be non-null, have nonzero length, and have no whitespace.
        /// </remarks>
        public string Value { get; }

        /// <summary>
        /// Constructs an instance of <see cref="Token"/> from a string value.
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

            Value = value;
        }

        /// <summary>
        /// Returns the underlying string value.
        /// </summary>
        public override string ToString() => Value;

        /// <summary>
        /// Determines whether this instance and another object,
        /// which must be a <see cref="Token"/> or a <see cref="string"/>,
        /// have the same value.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Token token)
            {
                return this.Equals(token);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the hash code of the underlying value.
        /// </summary>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Compares this instance with a specified <see cref="Token"/> object
        /// and indicates whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified object.
        /// </summary>
        public int CompareTo(Token other) => Value.CompareTo(other.Value);

        /// <summary>
        /// Determines whether this instance and another <see cref="Token"/>
        /// have the same value.
        /// </summary>
        public bool Equals(Token other) => Value.Equals(other.Value);

        /// <summary>
        /// Determines whether this instance and another <see cref="Token"/>
        /// have the same value.
        /// A parameter specifies the culture, case, and sort rules used in the comparison.
        /// </summary>
        public bool Equals(Token other, StringComparison comparisonType) => Value.Equals(other.Value, comparisonType);

        /// <summary>
        /// Determines whether two instances of <see cref="Token"/>
        /// have the same value.
        /// </summary>
        public static bool operator ==(Token lhs, Token rhs) => Equals(lhs, rhs);

        /// <summary>
        /// Determines whether two instances of <see cref="Token"/>
        /// have different values.
        /// </summary>
        public static bool operator !=(Token lhs, Token rhs) => !Equals(lhs, rhs);

        // For some reason, String implements IComparable but does not have comparison operators.
        // Therefore, I won't add them to Token, either.
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
        /// This is consistent with the <see cref="Token"/> type itself,
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