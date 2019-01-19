namespace System
{
    // A non-null, non-empty string value where whitespace is not allowed.
    public sealed class Token
    {
        public string Value { get; }

        public Token(Required<string> value)
        {
            if (value.Value == string.Empty)
            {
                // TODO Resources
                throw new ArgumentException("A Token value must not be empty.");
            }

            foreach (char c in value.Value)
            {
                if (Char.IsWhiteSpace(c))
                {
                    // TODO Resources
                    throw new ArgumentException($"A Token value must not contain whitespace. Token value: {value.Value}");
                }
            }

            Value = value.Value;
        }
    }
}