using System;

namespace Recore
{
    /// <summary>
    /// Contains methods for working with functions.
    /// </summary>
    public static class Func
    {
        /// <summary>
        /// Calls a function and return its result.
        /// </summary>
        /// <remarks>
        /// This method is useful for making immediately-invoked function expressions in C#.
        /// </remarks>
        public static T Invoke<T>(Func<T> f) => f();

        /// <summary>
        /// Passes through the argument passed to a void-returning routine.
        /// </summary>
        public static Func<T, T> Fluent<T>(Action<T> action) => x => { action(x); return x; };
    }
}
