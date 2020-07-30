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
        public static Func<T, T> Fluent<T>(this Action<T> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            return x =>
            {
                action(x);
                return x;
            };
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
            => Memoize(func, EqualityComparer<TSource>.Default);

        /// <summary>
        /// Creates a function that caches the results <paramref name="func"/> to avoid calling it more than once,
        /// using <paramref name="comparer"/> to determine when two arguments should be considered the same.
        /// </summary>
        /// <remarks>
        /// For the memoized function to be correct, <paramref name="func"/> should return the same result every time it is called with the same argument.
        /// The memoized function is not thread-safe.
        /// The memoized function is not meant to serve as a general-purpose cache.
        /// The lifetime of the memoized function should be bounded
        /// to prevent the memoized results from consuming too much memory.
        /// </remarks>
        public static Func<TSource, TResult> Memoize<TSource, TResult>(Func<TSource, TResult> func, IEqualityComparer<TSource> comparer)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            var memo = new Dictionary<TSource, TResult>(comparer);
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

        #region TupleArgs

        /// <summary>
        /// Converts a function's arguments to a tuple.
        /// </summary>
        public static Func<Tuple<T1, T2>, TResult> TupleArgs<T1, T2, TResult>(Func<T1, T2, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return tupleArgs => func(tupleArgs.Item1, tupleArgs.Item2);
        }

        /// <summary>
        /// Converts a function's arguments to a tuple.
        /// </summary>
        public static Func<Tuple<T1, T2, T3>, TResult> TupleArgs<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return tupleArgs => func(tupleArgs.Item1, tupleArgs.Item2, tupleArgs.Item3);
        }

        /// <summary>
        /// Converts a function's arguments to a tuple.
        /// </summary>
        public static Func<Tuple<T1, T2, T3, T4>, TResult> TupleArgs<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return tupleArgs => func(tupleArgs.Item1, tupleArgs.Item2, tupleArgs.Item3, tupleArgs.Item4);
        }

        /// <summary>
        /// Converts a function's arguments to a tuple.
        /// </summary>
        public static Func<Tuple<T1, T2, T3, T4, T5>, TResult> TupleArgs<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return tupleArgs => func(tupleArgs.Item1, tupleArgs.Item2, tupleArgs.Item3, tupleArgs.Item4, tupleArgs.Item5);
        }

        /// <summary>
        /// Converts a function's arguments to a tuple.
        /// </summary>
        public static Func<Tuple<T1, T2, T3, T4, T5, T6>, TResult> TupleArgs<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return tupleArgs => func(tupleArgs.Item1, tupleArgs.Item2, tupleArgs.Item3, tupleArgs.Item4, tupleArgs.Item5, tupleArgs.Item6);
        }

        /// <summary>
        /// Converts a function's arguments to a tuple.
        /// </summary>
        public static Func<Tuple<T1, T2, T3, T4, T5, T6, T7>, TResult> TupleArgs<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return tupleArgs => func(tupleArgs.Item1, tupleArgs.Item2, tupleArgs.Item3, tupleArgs.Item4, tupleArgs.Item5, tupleArgs.Item6, tupleArgs.Item7);
        }

        #endregion TupleArgs
        #region UntupleArgs

        /// <summary>
        /// Spreads a unary function's tuple argument.
        /// </summary>
        public static Func<T1, T2, TResult> UntupleArgs<T1, T2, TResult>(Func<Tuple<T1, T2>, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return (arg1, arg2) => func(Tuple.Create(arg1, arg2));
        }

        /// <summary>
        /// Spreads a unary function's tuple argument.
        /// </summary>
        public static Func<T1, T2, T3, TResult> UntupleArgs<T1, T2, T3, TResult>(Func<Tuple<T1, T2, T3>, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return (arg1, arg2, arg3) => func(Tuple.Create(arg1, arg2, arg3));
        }

        /// <summary>
        /// Spreads a unary function's tuple argument.
        /// </summary>
        public static Func<T1, T2, T3, T4, TResult> UntupleArgs<T1, T2, T3, T4, TResult>(Func<Tuple<T1, T2, T3, T4>, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return (arg1, arg2, arg3, arg4) => func(Tuple.Create(arg1, arg2, arg3, arg4));
        }

        /// <summary>
        /// Spreads a unary function's tuple argument.
        /// </summary>
        public static Func<T1, T2, T3, T4, T5, TResult> UntupleArgs<T1, T2, T3, T4, T5, TResult>(Func<Tuple<T1, T2, T3, T4, T5>, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return (arg1, arg2, arg3, arg4, arg5) => func(Tuple.Create(arg1, arg2, arg3, arg4, arg5));
        }

        /// <summary>
        /// Spreads a unary function's tuple argument.
        /// </summary>
        public static Func<T1, T2, T3, T4, T5, T6, TResult> UntupleArgs<T1, T2, T3, T4, T5, T6, TResult>(Func<Tuple<T1, T2, T3, T4, T5, T6>, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return (arg1, arg2, arg3, arg4, arg5, arg6) => func(Tuple.Create(arg1, arg2, arg3, arg4, arg5, arg6));
        }

        /// <summary>
        /// Spreads a unary function's tuple argument.
        /// </summary>
        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> UntupleArgs<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<Tuple<T1, T2, T3, T4, T5, T6, T7>, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => func(Tuple.Create(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
        }

        #endregion UntupleArgs
    }
}
