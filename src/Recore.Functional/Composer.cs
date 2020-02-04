using System;

namespace Recore.Functional
{
    /// <example>
    /// // Without Composer
    /// var result = Baz(Bar(Foo(value)));
    ///
    /// // With Composer
    /// var result = Composer.Create(value)
    ///     .Then(Foo)
    ///     .Then(Bar)
    ///     .Then(Baz)
    ///     .Result;
    /// </example>
    public readonly struct Composer<T>
    {
        /// <summary>
        /// Gets the result of the Composer.
        /// </summary>
        public T Result { get; }

        /// <summary>
        /// Initializes the Composer from a value.
        /// </summary>
        public Composer(T value)
        {
            Result = value;
        }

        /// <summary>
        /// Invokes a function on the Composer's current value
        /// and passes the result through the Composer.
        /// </summary>
        public Composer<U> Then<U>(Func<T, U> func)
            => new Composer<U>(func(Result));

        /// <summary>
        /// Invokes an action on the Composer's current value
        /// and passes the value through the Composer.
        /// </summary>
        public Composer<T> Then(Action<T> action)
            => Then(action.Fluent());
    }

    /// <summary>
    /// Provides additional methods for <c cref="Composer{T}">Composer&lt;T&gt;</c>.
    /// </summary>
    public static class Composer
    {
        /// <summary>
        /// Creates a Composer from a value.
        /// </summary>
        /// <remarks>
        /// This method works the the same as the constructor, but it is useful for type inference.
        /// </remarks>
        public static Composer<T> Of<T>(T value) => new Composer<T>(value);
    }
}