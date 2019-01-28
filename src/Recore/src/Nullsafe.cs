using System;

namespace Recore
{
    public static class NullsafeExtensions
    {
        public static U Nullsafe<T, U>(this T t, Func<T, U> f, U safe = default) where T : class
        {
            if (t == null)
            {
                return safe;
            }
            else
            {
                return f(t);
            }
        }
    }
}