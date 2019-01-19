namespace System
{
    public struct Nullsafe<T> where T : class
    {
        public Nullsafe(T value) => Value = value;

        public static implicit operator Nullsafe(T value) => new Nullsafe(value);

        public T Value { get; }

        public Nullsafe<U> Try<U>(Func<T, U> f) where U : class
        {
            if (Value == null)
            {
                return new Nullsafe<U>(null);
            }
            else
            {
                return f(Value);
            }
        }
    }
}