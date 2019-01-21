namespace System
{
    // A non-null, non-empty string value where whitespace is not allowed.
    public sealed class Token
    {
        // This could be Required, but Token already makes that guarantee,
        // along with it being nonzero length and no whitespace.
        public string Value { get; }

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

            Value = value;
        }

        public static implicit operator string(Token t) => t.Value;
    }
}