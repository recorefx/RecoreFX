using System;
using System.Collections.Generic;
using System.Linq;

using Recore.Linq;

namespace Recore.Functional
{
    /// <summary>
    /// Adapts functions to operate on functors.
    /// </summary>
    public static class Lift
    {
        /// <summary>
        /// Converts a unary function to work with <c cref="Optional{T}">Optional</c>.
        /// </summary>
        public static Func<Optional<T>, Optional<TResult>> OnOptional<T, TResult>(Func<T, TResult> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return optional => optional.OnValue(func);
        }

        /// <summary>
        /// Converts a unary action to work with <c cref="Optional{T}">Optional</c>.
        /// </summary>
        public static Action<Optional<T>> OnOptional<T>(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return optional => optional.IfValue(action);
        }

        /// <summary>
        /// Combines two unary functions with the same return type
        /// into a single function taking either of their parameters.
        /// </summary>
        public static Func<Either<TLeft, TRight>, TResult> OnEither<TLeft, TRight, TResult>(
            Func<TLeft, TResult> leftFunc,
            Func<TRight, TResult> rightFunc)
        {
            if (leftFunc == null)
                throw new ArgumentNullException(nameof(leftFunc));
            if (rightFunc == null)
                throw new ArgumentNullException(nameof(rightFunc));

            return either => either.Switch(leftFunc, rightFunc);
        }

        /// <summary>
        /// Combines two unary actions into a single action taking either of their parameters.
        /// </summary>
        public static Action<Either<TLeft, TRight>> OnEither<TLeft, TRight>(
            Action<TLeft> leftAction,
            Action<TRight> rightAction)
        {
            if (leftAction == null)
                throw new ArgumentNullException(nameof(leftAction));
            if (rightAction == null)
                throw new ArgumentNullException(nameof(rightAction));

            return either => either.Switch(leftAction, rightAction);
        }

        /// <summary>
        /// Converts a function operating on a scalar value to a function operating on a sequence of values.
        /// </summary>
        public static Func<IEnumerable<T>, IEnumerable<TResult>> OnEnumerable<T, TResult>(Func<T, TResult> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return enumerable => enumerable.Select(func);
        }

        /// <summary>
        /// Converts an action operating on a scalar value to an action operating on a sequence of values.
        /// </summary>
        public static Action<IEnumerable<T>> OnEnumerable<T>(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return enumerable => enumerable.ForEach(action);
        }
    }
}