using System;

namespace Recore
{
    public struct Required<T> where T : class
    {
        public Required(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        public static implicit operator T(Required<T> required) => required.Value;

        public T Value { get; }
    }
}