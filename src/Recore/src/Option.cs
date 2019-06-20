using System;

namespace Recore
{
    /// <summary>
    /// A type-safe wrapper for a nullable value.
    /// </summary>
    public readonly struct Option<T>
    {
        private readonly T value;

        public Option(T value)
        {
            this.value = value;
            this.HasValue = value != null;
        }

        public bool HasValue { get; }

        /// <summary>
        /// An <c cref="Option{T}">Option</c> without a value.
        /// </summary>
        /// <remarks>
        /// While an empty <c cref="Option{T}">Option</c> can also be created by calling the default constructor
        /// or passing <c>null</c> to the constructor, <c cref="None">None</c> prevents
        /// unnecessary allocations with a single, statically-allocated object.
        /// It is also more expressive, making the absence of a value more obvious.
        /// </remarks>
        public static Option<T> None { get; } = new Option<T>();

        /// <summary>
        /// Choose a function to call depending on whether the <c cref="Option{T}">Option</c> has a value.
        /// </summary>
        /// <param name="onValue">Called when the <c cref="Option{T}">Option</c> has a value.</param>
        /// <param name="onNone">Called when the <c cref="Option{T}">Option</c> does not have a value.</param>
        /// <returns>Result of the function that was called.</returns>
        public U Switch<U>(Func<T, U> onValue, Func<U> onNone)
        {
            if (HasValue)
            {
                return onValue(value);
            }
            else
            {
                return onNone();
            }
        }

        /// <summary>
        /// Choose an action to take depending on whether the <c cref="Option{T}">Option</c> has a value.
        /// </summary>
        /// <param name="onValue">Called when the <c cref="Option{T}">Option</c> has a value.</param>
        /// <param name="onNone">Called when the <c cref="Option{T}">Option</c> does not have a value.</param>
        public void Switch(Action<T> onValue, Action onNone)
        {
            if (HasValue)
            {
                onValue(value);
            }
            else
            {
                onNone();
            }
        }

        /// <summary>
        /// Extract the value with a fallback if the <c cref="Option{T}">Option</c> is empty.
        /// </summary>
        public T ValueOr(T fallback)
            => Switch(
                x => x,
                () => fallback);

        /// <summary>
        /// Take an action only if the <c cref="Option{T}">Option</c> has a value.
        /// </summary>
        public void IfValue(Action<T> onValue)
            => Switch(
                onValue,
                () => { });

        /// <summary>
        /// Take an action only if the <c cref="Option{T}">Option</c> is empty.
        /// </summary>
        public void IfNone(Action onNone)
            => Switch(
                x => { },
                onNone);

        /// <summary>
        /// Map a function over the <c cref="Option{T}">Option</c>'s value, or propagate <c cref="None">None</c>.
        /// </summary>
        public Option<U> Try<U>(Func<T, U> f)
            => Switch(
                x => new Option<U>(f(x)),
                () => Option<U>.None);

        /// <summary>
        /// Chain another <c cref="Option{T}">Option</c>-producing operation onto the result of another.
        /// </summary>
        /// <remarks>
        /// This is a monad bind operation.
        /// Conceptually, it is the same as passing <paramref name="f"/> to <c cref="Try{U}(Func{T, U})">Try</c>
        /// and then "flattening" the <c>Optionlt;Option&lt;<typeparamref name="T"/>&gt;&gt;</c> into an <c>Option&lt;<typeparamref name="T"/>&gt;</c>.
        /// (Note that <c>Optionlt;Option&lt;<typeparamref name="T"/>&gt;&gt;</c> is not a valid <c cref="Option{T}">Option</c> because of the
        /// type constraint <c>where T : class</c>.)
        /// </remarks>
        public Option<U> Then<U>(Func<T, Option<U>> f)
            => Switch(
                f,
                () => Option<U>.None);

        // TODO ToString

        // TODO Equals

        // TODO HashCode

        public static implicit operator Option<T>(T value) => new Option<T>(value);
    }

    public static class Option
    {
        /// <summary>
        /// Convert an <c cref="Option{Option{T}}">Option&lt;Option&lt;T&gt;&gt;</c>
        /// to an <c cref="Option{T}">Option&lt;T&gt;</c>.
        /// </summary>
        public static Option<T> Flatten<T>(this Option<Option<T>> doubleOption)
            => doubleOption.ValueOr(Option<T>.None);
    }
}
