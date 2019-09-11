using System;
using System.Collections.Generic;

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
        public static Func<T, T> Fluent<T>(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return x => { action(x); return x; };
        }

        /// <summary>
        /// Creates a function that caches the results <paramref name="func"/> to avoid calling it more than once.
        /// </summary>
        /// <remarks>
        /// For the memoized function to be correct, <paramref name="func"/> should return the same result every time it is called with the same argument.
        /// The memoized function is not thread-safe.
        /// The memoized function is not meant to serve as a general-purpose cache.
        /// The lifetime of the memoized function should be bounded
        /// to prevent the memoized results from consuming too much memory.
        /// </remarks>
        public static Func<TSource, TResult> Memoize<TSource, TResult>(Func<TSource, TResult> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            var memo = new Dictionary<TSource, TResult>();
            return arg =>
            {
                if (memo.TryGetValue(arg, out var memoResult))
                {
                    return memoResult;
                }
                else
                {
                    var result = func(arg);
                    memo[arg] = result;
                    return result;
                }
            };
        }
    }
}
