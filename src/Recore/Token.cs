using System;

using Recore.Properties;

namespace Recore
{
    /// <summary>
    /// Represents a non-null, non-empty string value where whitespace is not allowed.
    /// </summary>
    public sealed record Token : IComparable<Token?>
    {
        // Guaranteed to be non-null, nonzero length, and no whitespace
        private readonly string value;

        /// <summary>
        /// Constructs an instance of <see cref="Token"/> from a string value.
        /// </summary>
        public Token(string value)
        {
            if (value is null)
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
        /// Determines whether this instance and another <see cref="Token"/>
        /// have the same value.
        /// A parameter specifies the culture, case, and sort rules used in the comparison.
        /// </summary>
        public bool Equals(Token? other, StringComparison comparisonType) => value.Equals(other?.value, comparisonType);

        /// <summary>
        /// Compares this instance with a specified <see cref="Token"/> object
        /// and indicates whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified object.
        /// </summary>
        public int CompareTo(Token? other) => value.CompareTo(other?.value);

        // For some reason, String implements IComparable but does not have comparison operators.
        // Therefore, I won't add them to Token, either.

        /// <summary>
        /// Converts this instance to its underlying value.
        /// </summary>
        public static implicit operator string(Token t) => t.ToString();
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
        /// This is consistent with the <seealso cref="Token"/> type itself,
        /// which does not check for any characters besides whitespace.
        /// </remarks>
        public static Token[] Tokenize(this string str)
        {
            var parts = str.Split(separator: (char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            var tokens = new Token[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                tokens[i] = new Token(parts[i]);
            }

            return tokens;
        }
    }
}