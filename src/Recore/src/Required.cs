using System;

namespace Recore
{
    public readonly struct Required<T> where T : class
    {
        private readonly T value;
        public T Value
        {
            get => value ?? throw new UninitializedStructException<Required<T>>();
        }

        public Required(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.value = value;
        }

        public static implicit operator T(in Required<T> required) => required.Value;
    }
}