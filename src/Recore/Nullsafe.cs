using System;

namespace Recore
{
    /// <summary>
    /// Provides methods for handling nullable values.
    /// </summary>
    public static class NullsafeExtensions
    {
        /// <summary>
        /// Applies a function only if a value is not null.
        /// </summary>
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