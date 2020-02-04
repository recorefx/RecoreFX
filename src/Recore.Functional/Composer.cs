using System;

namespace Recore.Functional
{
    /// <summary>
    /// Composes many functions or actions into a single function.
    /// </summary>
    /// <example>
    /// // Without Composer
    /// var result = Baz(Bar(Foo(value)));
    ///
    /// // With Composer
    /// var result = Composer.Create(value)
    ///     .Then(Foo)
    ///     .Then(Bar)
    ///     .Then(Baz)
    ///     .Func();
    /// </example>
    public sealed class Composer<T>
    {
        /// <summary>
        /// Gets the composed function.
        /// </summary>
        public Func<T> Func { get; }

        /// <summary>
        /// Initializes the <see cref="Composer" /> from a function.
        /// </summary>
        public Composer(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            Func = func;
        }

        /// <summary>
        /// Adds another function to the composed result.
        /// </summary>
        public Composer<U> Then<U>(Func<T, U> func)
            => new Composer<U>(() => func(Func()));

        /// <summary>
        /// Adds an action to be performed when evaluating the composed function.
        /// </summary>
        /// <remarks>
        /// Note that the action will be called lazily.
        /// It will not be called until the composed function is called.
        /// </remarks>
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
        public static Composer<T> Of<T>(T value) => new Composer<T>(() => value);
    }
}