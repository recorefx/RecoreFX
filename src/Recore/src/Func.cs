using System;

namespace Recore
{
    /// <summary>
    /// Contains methods for working with functions.
    /// </summary>
    public static class Func
    {
        /// <summary>
        /// Call a function and return its result.
        /// </summary>
        /// <remarks>
        /// This method is useful for making immediately-invoked function expressions in C#.
        /// </remarks>
        public static T Invoke<T>(Func<T> f) => f();
    }
}
