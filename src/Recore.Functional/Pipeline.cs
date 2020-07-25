using System;

namespace Recore.Functional
{
    /// <summary>
    /// Creates a function pipeline, calling each function or action on the value
    /// with postfix syntax.
    /// </summary>
    /// <remarks>
    /// Functions are called on the value eagerly.
    /// </remarks>
    /// <example>
    /// Without <see cref="Pipeline{T}"/>:
    /// <code>
    /// var result = Baz(Bar(Foo(value)));
    /// </code>
    ///
    /// With <see cref="Pipeline{T}"/>:
    /// <code>
    /// var result = Pipeline.Of(value)
    ///     .Then(Foo)
    ///     .Then(Bar)
    ///     .Then(Baz)
    ///     .Result;
    /// </code>
    /// </example>
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
    /// Provides additional methods for <see cref="Pipeline{T}"/>.
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