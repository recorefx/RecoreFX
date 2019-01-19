namespace System
{
    public struct Required<T> where T : class
    {
        public Required(T value) => Value = value ?? throw new ArgumentNullException(nameof(value));

        public static implicit operator Required<T>(T value) => new Required<T>(value);

        public T Value { get; }
    }
}