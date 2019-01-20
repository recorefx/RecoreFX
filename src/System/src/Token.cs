namespace System
{
    // A non-null, non-empty string value where whitespace is not allowed.
    public sealed class Token
    {
        public Required<string> Value { get; }

        public Token(Required<string> value)
        {
            if (value == string.Empty)
            {
                throw new ArgumentException(Strings.TokenEmpty);
            }

            foreach (char c in value.Value)
            {
                if (char.IsWhiteSpace(c))
                {
                    throw new ArgumentException(string.Format(Strings.TokenWhitespace, value.Value));
                }
            }

            Value = value;
        }

        public static implicit operator string(Token t) => t.Value;
    }
}