using System;

namespace Recore
{
    /// <summary>
    /// A type-safe wrapper for a nullable value.
    /// </summary>
    public readonly struct Optional<T>
    {
        private readonly T value;

        public Optional(T value)
        {
            this.value = value;
            this.HasValue = value != null;
        }

        public bool HasValue { get; }

        /// <summary>
        /// An <c cref="Optional{T}">Optional</c> without a value.
        /// </summary>
        /// <remarks>
        /// This generic alias is sometimes needed to help C#'s type inference.
        /// </remarks>
        public static Optional<T> Empty => Optional.Empty;

        /// <summary>
        /// Choose a function to call depending on whether the <c cref="Optional{T}">Optional</c> has a value.
        /// </summary>
        /// <param name="onValue">Called when the <c cref="Optional{T}">Optional</c> has a value.</param>
        /// <param name="onEmpty">Called when the <c cref="Optional{T}">Optional</c> does not have a value.</param>
        /// <returns>Result of the function that was called.</returns>
        public U Switch<U>(Func<T, U> onValue, Func<U> onEmpty)
        {
            if (HasValue)
            {
                return onValue(value);
            }
            else
            {
                return onEmpty();
            }
        }

        /// <summary>
        /// Choose an action to take depending on whether the <c cref="Optional{T}">Optional</c> has a value.
        /// </summary>
        /// <param name="onValue">Called when the <c cref="Optional{T}">Optional</c> has a value.</param>
        /// <param name="onEmpty">Called when the <c cref="Optional{T}">Optional</c> does not have a value.</param>
        public void Switch(Action<T> onValue, Action onEmpty)
        {
            if (HasValue)
            {
                onValue(value);
            }
            else
            {
                onEmpty();
            }
        }

        /// <summary>
        /// Extract the value with a fallback if the <c cref="Optional{T}">Optional</c> is empty.
        /// </summary>
        public T ValueOr(T fallback)
            => Switch(
                x => x,
                () => fallback);

        /// <summary>
        /// Map a function over the <c cref="Optional{T}">Optional</c>'s value, or propagate <c cref="Empty">Empty</c>.
        /// </summary>
        public Optional<U> OnValue<U>(Func<T, U> f)
            => Switch(
                x => new Optional<U>(f(x)),
                () => Optional.Empty);

        /// <summary>
        /// Take an action only if the <c cref="Optional{T}">Optional</c> has a value.
        /// </summary>
        public void IfValue(Action<T> onValue)
            => Switch(
                onValue,
                () => { });

        /// <summary>
        /// Take an action only if the <c cref="Optional{T}">Optional</c> is empty.
        /// </summary>
        public void IfEmpty(Action onEmpty)
            => Switch(
                x => { },
                onEmpty);

        /// <summary>
        /// Chain another <c cref="Optional{T}">Optional</c>-producing operation onto the result of another.
        /// </summary>
        /// <remarks>
        /// This is a monad bind operation.
        /// Conceptually, it is the same as passing <paramref name="f"/> to <c cref="OnValue{U}(Func{T, U})">OnValue</c>
        /// and then "flattening" the <c>Optionlt;Optional&lt;<typeparamref name="T"/>&gt;&gt;</c> into an <c>Optional&lt;<typeparamref name="T"/>&gt;</c>.
        /// (Note that <c>Optionlt;Optional&lt;<typeparamref name="T"/>&gt;&gt;</c> is not a valid <c cref="Optional{T}">Optional</c> because of the
        /// type constraint <c>where T : class</c>.)
        /// </remarks>
        public Optional<U> Then<U>(Func<T, Optional<U>> f)
            => Switch(
                f,
                () => Optional.Empty);

        // TODO ToString

        // TODO Equals

        // TODO HashCode

        public static implicit operator Optional<T>(T value) => new Optional<T>(value);

        // Only need one empty option
        private static readonly Optional<T> empty = new Optional<T>();
        public static implicit operator Optional<T>(Optional.EmptyType empty) => Optional<T>.empty;
    }

    public static class Optional
    {
        public readonly struct EmptyType
        {
            // TODO Equals
        }

        /// <summary>
        /// Represents the absence of an optional value.
        /// </summary>
        /// <remarks>
        /// This is the analog of <c>null</c> for optionals.
        /// While an empty <c cref="Optional{T}">Optional</c> can also be created by calling the default constructor
        /// or passing <c>null</c> to the constructor, <c cref="Empty">Empty</c>,
        /// and the associated implicit operator, prevent
        /// unnecessary allocations with a single, statically-allocated object.
        /// It is also more expressive, making the absence of a value more obvious.
        /// </remarks>
        public static EmptyType Empty { get; } = new EmptyType();

        /// <summary>
        /// Convert an <c cref="Optional{Optional{T}}">Optional&lt;Optional&lt;T&gt;&gt;</c>
        /// to an <c cref="Optional{T}">Optional&lt;T&gt;</c>.
        /// </summary>
        public static Optional<T> Flatten<T>(this Optional<Optional<T>> doubleOption)
            => doubleOption.ValueOr(Empty);
    }
}
