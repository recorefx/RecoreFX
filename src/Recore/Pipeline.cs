using System;

namespace Recore
{
    /// <example>
    /// // Without Pipeline
    /// var result = Baz(Bar(Foo(value)));
    ///
    /// // With Pipeline
    /// var result = Pipeline.Create(value)
    ///     .Pipe(Foo)
    ///     .Pipe(Bar)
    ///     .Pipe(Baz).Result;
    /// </example>
    public readonly struct Pipeline<T>
    {
        /// <summary>
        /// Gets the result of the pipeline.
        /// </summary>
        public T Result { get; }

        /// <summary>
        /// Initializes the pipeline from a value.
        /// </summary>
        public Pipeline(T value)
        {
            Result = value;
        }

        /// <summary>
        /// Invokes a function on the pipeline's current value
        /// and passes the result through the pipeline.
        /// </summary>
        public Pipeline<U> Pipe<U>(Func<T, U> func)
            => new Pipeline<U>(func(Result));

        /// <summary>
        /// Invokes an action on the pipeline's current value
        /// and passes the value through the pipeline.
        /// </summary>
        public Pipeline<T> Pipe(Action<T> action)
            => Pipe(action.Fluent());
    }

    /// <summary>
    /// Provides additional methods for <c cref="Pipeline{T}">Pipeline&lt;T&gt;</c>.
    /// </summary>
    public static class Pipeline
    {
        /// <summary>
        /// Creates a pipeline from a value.
        /// </summary>
        /// <remarks>
        /// This method works the the same as the constructor, but it is useful for type inference.
        /// </remarks>
        public static Pipeline<T> Of<T>(T value) => new Pipeline<T>(value);
    }
}