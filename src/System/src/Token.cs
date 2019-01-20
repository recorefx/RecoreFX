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
                // TODO 21 Resources
                throw new ArgumentException("A Token value must not be empty.");
            }

            foreach (char c in value.Value)
            {
                if (char.IsWhiteSpace(c))
                {
                    // TODO 21 Resources
                    throw new ArgumentException($"A Token value must not contain whitespace. Token value: {value.Value}");
                }
            }

            Value = value;
        }

        public static implicit operator string(Token t) => t.Value;
    }
}