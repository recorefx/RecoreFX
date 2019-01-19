namespace System
{
    public struct Required<T> where T : class
    {
        public Required(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), value);
            }

            Value = value;
        }

        public static implicit operator Required(T value) => new Required(value);

        public T Value { get; }
    }
}