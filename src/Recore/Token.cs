using System;

using Recore.Properties;

namespace Recore
{
    /// <summary>
    /// Represents a non-null, non-empty string value where whitespace is not allowed.
    /// </summary>
    /// <remarks>
    /// This type implements <see cref="IComparable{T}"/> with <c cref="Of{T}">Of&lt;String&gt;</c>
    /// instead of <c cref="Of{T}">Of&lt;Token&gt;</c>.
    /// This is for parity with its inherited implementation of <c cref="IEquatable{T}">IEquatable&lt;Of&lt;String&gt;&gt;</c>.
    /// </remarks>
    public sealed class Token : Of<string>, IComparable<Of<string>>
    {
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

            Value = value;
        }

        /// <summary>
        /// Determines whether this instance and another <see cref="Token"/>
        /// have the same value.
        /// A parameter specifies the culture, case, and sort rules used in the comparison.
        /// </summary>
        public bool Equals(Of<string> other, StringComparison comparisonType) => Value.Equals(other.Value, comparisonType);

        /// <summary>
        /// Compares this instance with a specified <see cref="Token"/> object
        /// and indicates whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified object.
        /// </summary>
        public int CompareTo(Of<string> other) => Value.CompareTo(other.Value);

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