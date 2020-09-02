using System;

namespace Recore
{
    /// <summary>
    /// Creates a function pipeline, calling each function or action on the value
    /// with postfix syntax.
    /// </summary>
    /// <remarks>
    /// <see cref="Pipeline{T}"/> and <seealso cref="Composer{TValue, TResult}"/> differ
    /// in that <see cref="Pipeline{T}"/> invokes its functions eagerly,
    /// while <seealso cref="Composer{TValue, TResult}"/> invokes its functions lazily.
    /// <see cref="Pipeline{T}"/> produces a value as its final result, whereas
    /// <seealso cref="Composer{TValue, TResult}"/> produces a function.
    /// </remarks>
    public sealed class Pipeline<T>
    {
        /// <summary>
        /// Gets the result of the <see cref="Pipeline{T}"/>.
        /// </summary>
        public T Result { get; }

        /// <summary>
        /// Initializes the <see cref="Pipeline{T}"/> from a value.
        /// </summary>
        public Pipeline(T value)
        {
            Result = value;
        }

        /// <summary>
        /// Invokes a function on the <see cref="Pipeline{T}"/>'s current value
        /// and passes the result through the <see cref="Pipeline{T}"/>.
        /// </summary>
        public Pipeline<U> Then<U>(Func<T, U> func)
            => new Pipeline<U>(func(Result));

        /// <summary>
        /// Invokes an action on the <see cref="Pipeline{T}"/>'s current value
        /// and passes the value through the <see cref="Pipeline{T}"/>.
        /// </summary>
        public Pipeline<T> Then(Action<T> action)
            => Then(action.Fluent());
    }

    /// <summary>
    /// Provides additional methods for <seealso cref="Pipeline{T}"/>.
    /// </summary>
    public static class Pipeline
    {
        /// <summary>
        /// Creates a <see cref="Pipeline{T}"/> from a value.
        /// </summary>
        /// <remarks>
        /// This method works the the same as the constructor, but it is useful for type inference.
        /// </remarks>
        public static Pipeline<T> Of<T>(T value) => new Pipeline<T>(value);
    }
}