using System;

namespace Recore
{
    /// <summary>
    /// A type-safe wrapper for a nullable value.
    /// </summary>
    public readonly struct Optional<T> : IEquatable<Optional<T>>
    {
        private readonly T value;

        /// <summary>
        /// Create an <c cref="Optional{T}">Optional</c> with a value.
        /// </summary>
        /// <remarks>
        /// If <c>null</c> is passed for <paramref name="value"/>, then the <c cref="Optional{T}">Optional</c>
        /// is considered empty.
        /// </remarks>
        public Optional(T value)
        {
            this.value = value;
            this.HasValue = value != null;
        }

        /// <summary>
        /// Whether the <c cref="Optional{T}">Optional</c> was created with a value.
        /// </summary>
        public bool HasValue { get; }

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
                x => Optional.Of(f(x)),
                Optional.Empty<U>);

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
                Optional.Empty<U>);

        /// <summary>
        /// Return the value's string representation, or a localized "none" message.
        /// </summary>
        public override string ToString()
            => Switch(
                x => x.ToString(),
                () => Strings.OptionalEmptyToString);

        public override bool Equals(object other)
            => other is Optional<T>
            && this.Equals((Optional<T>)other);

        public bool Equals(Optional<T> other)
        {
            if (!this.HasValue || !other.HasValue)
            {
                return !this.HasValue && !other.HasValue;
            }
            else
            {
                return Equals(this.value, other.value);
            }
        }

        public override int GetHashCode()
            => Switch(
                x => x.GetHashCode(),
                () => 0);

        public static bool operator==(Optional<T> lhs, Optional<T> rhs) => lhs.Equals(rhs);

        public static bool operator !=(Optional<T> lhs, Optional<T> rhs) => !lhs.Equals(rhs);

        public static implicit operator Optional<T>(T value) => new Optional<T>(value);

        public static explicit operator T(Optional<T> optional)
            => optional.Switch(
                x => optional.value,
                () => throw new InvalidCastException(string.Format(Strings.OptionalEmptyInvalidCast, typeof(T))));
    }

    /// <summary>
    /// Additional methods for <c cref="Optional{T}">Optional&lt;T&gt;</c>.
    /// </summary>
    public static class Optional
    {
        /// <summary>
        /// Make a value optional.
        /// </summary>
        /// <remarks>
        /// This is useful for type inference in some cases where the implicit conversion
        /// can't be used, such as creating an <c cref="Optional{T}">Optional&lt;T&gt;</c>
        /// and immediately invoking a method.
        /// It can also be passed as a delegate whereas the constructor can't be.
        /// </remarks>
        public static Optional<T> Of<T>(T value) => new Optional<T>(value);

        /// <summary>
        /// An <c cref="Optional{T}">Optional</c> without a value.
        /// </summary>
        /// <remarks>
        /// While an empty <c cref="Optional{T}">Optional</c> can also be created by calling the default constructor
        /// or passing <c>null</c> to the constructor,
        /// <c cref="Empty">Empty</c> is more expressive, making the absence of a value more obvious.
        /// It can also be passed as a delegate whereas the constructor can't be.
        /// </remarks>
        public static Optional<T> Empty<T>() => new Optional<T>();

        /// <summary>
        /// Convert an <c cref="Optional{Optional{T}}">Optional&lt;Optional&lt;T&gt;&gt;</c>
        /// to an <c cref="Optional{T}">Optional&lt;T&gt;</c>.
        /// </summary>
        public static Optional<T> Flatten<T>(this Optional<Optional<T>> doubleOption)
            => doubleOption.Then(x => x);
    }
}
