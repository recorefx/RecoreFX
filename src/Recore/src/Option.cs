using System;

namespace Recore
{
    // TODO ref struct?
    public readonly struct Option<T> where T : class
    {
        private readonly T value;

        public Option(T value)
        {
            this.value = value;
        }

        public bool HasValue => value != null;

        // TODO can't have this with a ref struct
        public static Option<T> None = new Option<T>();

        public U Match<U>(Func<T, U> onValue, Func<T, U> onNone)
        {
            if (HasValue)
            {
                return onValue(value);
            }
            else
            {
                return onNone(value);
            }
        }

        public void Match(Action<T> onValue, Action<T> onNone)
        {
            if (HasValue)
            {
                onValue(value);
            }
            else
            {
                onNone(value);
            }
        }

        public Option<U> Map<U>(Func<T, U> f) where U : class
            => Match(
                x => new Option<U>(f(x)),
                x => Option<U>.None);

        public Option<U> Bind<U>(Func<T, Option<U>> f) where U : class
            => Match(
                f,
                x => Option<U>.None);

        // TODO Equals
    }
}
