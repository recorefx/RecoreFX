using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Recore.Properties;

namespace Recore
{
    /// <summary>
    /// Provides type-safe access to a nullable value.
    /// </summary>
    public readonly struct Optional<T> : IEquatable<Optional<T>>, IEnumerable<T>
    {
        private readonly T value;

        /// <summary>
        /// Indicates whether the <c cref="Optional{T}">Optional</c> was created with a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Creates an <c cref="Optional{T}">Optional</c> with a value.
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
        /// Chooses a function to call depending on whether the <c cref="Optional{T}">Optional</c> has a value.
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
        /// Chooses an action to take depending on whether the <c cref="Optional{T}">Optional</c> has a value.
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
        /// Extracts the value with a fallback if the <c cref="Optional{T}">Optional</c> is empty.
        /// </summary>
        public T ValueOr(T fallback)
            => Switch(
                x => x,
                () => fallback);

        /// <summary>
        /// Maps a function over the <c cref="Optional{T}">Optional</c>'s value, or propagates <c cref="Optional.Empty{T}">Empty</c>.
        /// </summary>
        public Optional<U> OnValue<U>(Func<T, U> f)
            => Switch(
                x => Optional.Of(f(x)),
                Optional.Empty<U>);

        /// <summary>
        /// Takes an action only if the <c cref="Optional{T}">Optional</c> has a value.
        /// </summary>
        public void IfValue(Action<T> onValue)
            => Switch(
                onValue,
                () => { });

        /// <summary>
        /// Takes an action only if the <c cref="Optional{T}">Optional</c> is empty.
        /// </summary>
        public void IfEmpty(Action onEmpty)
            => Switch(
                x => { },
                onEmpty);

        /// <summary>
        /// Chains another <c cref="Optional{T}">Optional</c>-producing operation onto the result of another.
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
        /// Returns the value's string representation, or a localized "none" message.
        /// </summary>
        public override string ToString()
            => Switch(
                x => x.ToString(),
                () => Resources.OptionalEmptyToString);

        /// <summary>
        /// Determines whether this instance and another object,
        /// which must also be an <c cref="Optional{T}">Optional&lt;T&gt;</c>,
        /// have the same value.
        /// </summary>
        public override bool Equals(object obj)
            => obj is Optional<T>
            && this.Equals((Optional<T>)obj);

        /// <summary>
        /// Determines whether this instance and another <c cref="Optional{T}">Optional&lt;T&gt;</c>
        /// have the same value.
        /// </summary>
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

        /// <summary>
        /// Returns the hash code for the underlying type
        /// or zero if there is no value.
        /// </summary>
        public override int GetHashCode()
            => Switch(
                x => x.GetHashCode(),
                () => 0);

        /// <summary>
        /// Returns an object that either yields the underlying value once
        /// or yields nothing if there is no value.
        /// </summary>
        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// Returns an object that either yields the underlying value once
        /// or yields nothing if there is no value.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private sealed class Enumerator : IEnumerator<T>
        {
            private Optional<T> target;
            private bool hasNextValue;

            public Enumerator(Optional<T> target)
            {
                this.target = target;
                Reset();
            }

            public T Current { get; private set; }
            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (hasNextValue)
                {
                    Current = target.value;
                    hasNextValue = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                hasNextValue = target.HasValue;
                Current = default; // undefined
            }

            public void Dispose() { }
        }

        /// <summary>
        /// Determines whether two instances of <c cref="Optional{T}">Optional&lt;T&gt;</c>
        /// have the same value.
        /// </summary>
        public static bool operator ==(Optional<T> lhs, Optional<T> rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Determines whether two instances of <c cref="Optional{T}">Optional&lt;T&gt;</c>
        /// have different values.
        /// </summary>
        public static bool operator !=(Optional<T> lhs, Optional<T> rhs) => !lhs.Equals(rhs);

        /// <summary>
        /// Converts an instance of a type to an optional value.
        /// </summary>
        public static implicit operator Optional<T>(T value) => new Optional<T>(value);

        /// <summary>
        /// Casts this instance to its underlying value
        /// or the default value for the underlying type.
        /// </summary>
        public static explicit operator T(Optional<T> optional)
            => optional.ValueOr(default);
    }

    /// <summary>
    /// Provides additional methods for <c cref="Optional{T}">Optional&lt;T&gt;</c>.
    /// </summary>
    public static class Optional
    {
        /// <summary>
        /// Makes a value optional.
        /// </summary>
        /// <remarks>
        /// This is useful for type inference in some cases where the implicit conversion
        /// can't be used, such as creating an <c cref="Optional{T}">Optional&lt;T&gt;</c>
        /// and immediately invoking a method.
        /// It can also be passed as a delegate whereas the constructor can't be.
        /// </remarks>
        public static Optional<T> Of<T>(T value) => new Optional<T>(value);

        /// <summary>
        /// Sets an optional value if a condition is true.
        /// </summary>
        /// <remarks>
        /// This method is useful for converting the TryParse pattern to an <c>Optional</c> result.
        /// </remarks>
        public static Optional<T> If<T>(bool condition, T value)
        {
            if (condition)
            {
                return Optional.Of(value);
            }
            else
            {
                return Optional.Empty<T>();
            }
        }

        /// <summary>
        /// Creates an <c cref="Optional{T}">Optional</c> without a value.
        /// </summary>
        /// <remarks>
        /// While an empty <c cref="Optional{T}">Optional</c> can also be created by calling the default constructor
        /// or passing <c>null</c> to the constructor,
        /// <c cref="Empty">Empty</c> is more expressive, making the absence of a value more obvious.
        /// It can also be passed as a delegate whereas the constructor can't be.
        /// </remarks>
        public static Optional<T> Empty<T>() => new Optional<T>();

        /// <summary>
        /// Converts an <c cref="Optional{T}">Optional&lt;Optional&lt;T&gt;&gt;</c>
        /// to an <c cref="Optional{T}">Optional&lt;T&gt;</c>.
        /// </summary>
        public static Optional<T> Flatten<T>(this Optional<Optional<T>> doubleOption)
            => doubleOption.Then(x => x);

        /// <summary>
        /// Collects the non-empty values from the sequence.
        /// </summary>
        public static IEnumerable<T> NonEmpty<T>(this IEnumerable<Optional<T>> source)
            => source.SelectMany(x => x);
    }
}
