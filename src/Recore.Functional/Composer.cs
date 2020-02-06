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
    /// // Without Composer
    /// var result = Baz(Bar(Foo(value)));
    ///
    /// // With Composer
    /// var result = Composer.Of(value)
    ///     .Then(Foo)
    ///     .Then(Bar)
    ///     .Then(Baz)
    ///     .Result;
    /// </example>
    public sealed class Composer<T>
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
    /// Composes many functions or actions into a single function.
    /// </summary>
    /// <example>
    /// // Without Composer
    /// var result = Baz(Bar(Foo(value)));
    ///
    /// // With Composer
    /// var result = new Composer&lt;string, int&gt;(Foo)
    ///     .Then(Bar)
    ///     .Then(Baz)
    ///     .Func();
    /// </example>
    public sealed class Composer<TValue, TResult>
    {
        /// <summary>
        /// Gets the composed function.
        /// </summary>
        public Func<TValue, TResult> Func { get; }

        /// <summary>
        /// Initializes the <see cref="Composer" /> from a function.
        /// </summary>
        public Composer(Func<TValue, TResult> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            Func = func;
        }

        /// <summary>
        /// Adds another function to the composed result.
        /// </summary>
        public Composer<TValue, TNextResult> Then<TNextResult>(Func<TResult, TNextResult> func)
            => new Composer<TValue, TNextResult>(x => func(Func(x)));

        /// <summary>
        /// Adds an action to be performed when evaluating the composed function.
        /// </summary>
        /// <remarks>
        /// Note that the action will be called lazily.
        /// It will not be called until the composed function is called.
        /// </remarks>
        public Composer<TValue, TResult> Then(Action<TResult> action)
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