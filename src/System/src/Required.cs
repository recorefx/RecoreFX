namespace System
{
    public struct Required<T> where T : class
    {
        public Required(T value) => Value = value ?? throw new ArgumentNullException(nameof(value));

        public static implicit operator T(Required<T> required) => required.Value;

        public T Value { get; }
    }
}