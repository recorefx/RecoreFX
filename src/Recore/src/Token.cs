using System;

namespace Recore
{
    // A non-null, non-empty string value where whitespace is not allowed.
    // Meant to feel like a subclass of String, which is sealed.
    public struct Token : IEquatable<Token>, IComparable<Token>, IEquatable<string>, IComparable<string>
    {
        private readonly string value;

        // Guaranteed to be non-null, nonzero length, and no whitespace
        private string Value
        {
            // `value` can be null if the struct is uninitialized
            // This is the same code as `Required<string>`, but inlined to keep the struct size down
            get => value ?? throw new UninitializedStructException<Token>();
        }

        public Token(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value == string.Empty)
            {
                throw new ArgumentException(Strings.TokenEmpty);
            }

            foreach (char c in value)
            {
                if (char.IsWhiteSpace(c))
                {
                    throw new ArgumentException(string.Format(Strings.TokenWhitespace, value));
                }
            }

            this.value = value;
        }

        public override string ToString() => Value;

        public bool Equals(Token other) => Value == other.Value;

        public bool Equals(string other) => Value.Equals(other);

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

        public override int GetHashCode() => Value.GetHashCode();

        public int CompareTo(Token other) => Value.CompareTo(other.Value);

        public int CompareTo(string other) => Value.CompareTo(other);

        public static implicit operator string(Token t) => t.ToString();

        // Implicit conversion to string gives us the string == and != operators for free
        // For some reason, String implements IComparable but does not have comparison operators
    }
}