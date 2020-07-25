using System;

namespace Recore.Functional
{
    /// <summary>
    /// Composes many functions or actions into a single function.
    /// </summary>
    /// <remarks>
    /// <see cref="Composer{TValue, TResult}"/> and <seealso cref="Pipeline{T}"/> differ
    /// in that <see cref="Composer{TValue, TResult}"/> invokes its functions lazily,
    /// while <seealso cref="Pipeline{T}"/> invokes its functions eagerly.
    /// <see cref="Composer{TValue, TResult}"/> produces a function as its final result, whereas
    /// <seealso cref="Pipeline{T}"/> produces a value.
    /// </remarks>
    /// <example>
    /// Without <see cref="Composer{TValue, TResult}"/>:
    /// <code>
    /// var result = Baz(Bar(Foo(value)));
    /// </code>
    ///
    /// With <see cref="Composer{TValue, TResult}"/>:
    /// <code>
    /// var result = new Composer&lt;string, int&gt;(Foo)
    ///     .Then(Bar)
    ///     .Then(Baz)
    ///     .Func(value);
    /// </code>
    /// </example>
    public sealed class Composer<TValue, TResult>
    {
        /// <summary>
        /// Gets the composed function.
        /// </summary>
        public Func<TValue, TResult> Func { get; }

        /// <summary>
        /// Initializes the <see cref="Composer{TValue, TResult}"/> from a function.
        /// </summary>
        public Composer(Func<TValue, TResult> func)
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
}