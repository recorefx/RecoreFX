using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Recore.Properties;
using Recore.Text.Json.Serialization.Converters;

namespace Recore
{
    /// <summary>
    /// Provides type-safe access to a nullable value.
    /// </summary>
    /// <remarks>
    /// <see cref="Optional{T}"/> differs from <see cref="Nullable{T}"/> in the following ways:
    /// <list type="bullet">
    /// <item><description><see cref="Nullable{T}"/> has syntactic sugar like the alias <c>T?</c> and the operators <c>?.</c> and <c>??</c></description></item>
    /// <item><description><see cref="Nullable{T}"/> has some special-case behavior in the CLR for boxing and <see cref="object.GetType()"/></description></item>
    /// <item><description><see cref="Nullable{T}"/> works only with value types while <see cref="Optional{T}"/> works with both value and reference types</description></item>
    /// <item><description><see cref="Nullable{T}"/> provides direct access to its value through <see cref="Nullable{T}.Value"/>, while <see cref="Optional{T}"/> requires access through its methods</description></item>
    /// </list>
    /// 
    /// The last point is the most significant. Accessing the value directly through <see cref="Nullable{T}.Value"/> opens up the possibility for a <see cref="NullReferenceException"/>.
    /// With <see cref="Optional{T}"/>, once you have an optional value, all operations on it happen in an "optional context."
    /// You can't get rid of <see cref="Optional{T}"/> until you do something to handle the null case such as by calling <see cref="Optional{T}.Switch{U}(Func{T, U}, Func{U})"/>
    /// or <see cref="Optional{T}.ValueOr(T)"/>.
    /// </remarks>
    [JsonConverter(typeof(OptionalConverter))]
    public readonly struct Optional<T> : IEquatable<Optional<T>>
    {
        private readonly T value;

        /// <summary>
        /// Indicates whether the <see cref="Optional{T}"/> was created with a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Creates an <see cref="Optional{T}"/> with a value.
        /// </summary>
        /// <remarks>
        /// If <c>null</c> is passed for <paramref name="value"/>, then the <see cref="Optional{T}"/>
        /// is considered empty.
        /// </remarks>
        public Optional(T value)
        {
            this.value = value;
            this.HasValue = value != null;
        }

        /// <summary>
        /// Creates an <see cref="Optional{T}"/> without a value.
        /// </summary>
        /// <remarks>
        /// While an empty <see cref="Optional{T}"/> can also be created by calling the default constructor
        /// or passing <c>null</c> to the constructor,
        /// <see cref="Empty"/> is more expressive, making the absence of a value more obvious.
        /// </remarks>
        public static Optional<T> Empty => new Optional<T>();

        /// <summary>
        /// Chooses a function to call depending on whether the <see cref="Optional{T}"/> has a value.
        /// </summary>
        /// <param name="onValue">Called when the <see cref="Optional{T}"/> has a value.</param>
        /// <param name="onEmpty">Called when the <see cref="Optional{T}"/> does not have a value.</param>
        /// <returns>Result of the function that was called.</returns>
        public U Switch<U>(Func<T, U> onValue, Func<U> onEmpty)
        {
            if (onValue is null)
            {
                throw new ArgumentNullException(nameof(onValue));
            }

            if (onEmpty is null)
            {
                throw new ArgumentNullException(nameof(onEmpty));
            }

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
        /// Chooses an action to take depending on whether the <see cref="Optional{T}"/> has a value.
        /// </summary>
        /// <param name="onValue">Called when the <see cref="Optional{T}"/> has a value.</param>
        /// <param name="onEmpty">Called when the <see cref="Optional{T}"/> does not have a value.</param>
        public void Switch(Action<T> onValue, Action onEmpty)
        {
            if (onValue is null)
            {
                throw new ArgumentNullException(nameof(onValue));
            }

            if (onEmpty is null)
            {
                throw new ArgumentNullException(nameof(onEmpty));
            }

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
        /// Extracts the value with a fallback if the <see cref="Optional{T}"/> is empty.
        /// </summary>
        public T ValueOr(T fallback)
            => Switch(
                x => x,
                () => fallback);

        /// <summary>
        /// Extracts the value or throws an <see cref="InvalidOperationException"/> if the <see cref="Optional{T}"/> is empty.
        /// </summary>
        public T AssertValue()
            => Switch(
                x => x,
                () => throw new InvalidOperationException(Resources.OptionalEmptyToString));

        /// <summary>
        /// Maps a function over the <see cref="Optional{T}"/>'s value, or propagates <see cref="Optional{T}.Empty"/>.
        /// </summary>
        public Optional<U> OnValue<U>(Func<T, U> f)
            => Switch(
                x => Optional.Of(f(x)),
                () => Optional<U>.Empty);

        /// <summary>
        /// Takes an action only if the <see cref="Optional{T}"/> has a value.
        /// </summary>
        public void IfValue(Action<T> onValue)
            => Switch(
                onValue,
                () => { });

        /// <summary>
        /// Chains another <see cref="Optional{T}"/>-producing operation onto the result of another.
        /// </summary>
        /// <remarks>
        /// This is a monad bind operation.
        /// Conceptually, it is the same as passing <paramref name="f"/> to <see cref="OnValue{U}(Func{T, U})"/>
        /// and then "flattening" the <c>Optionlt;Optional&lt;<typeparamref name="T"/>&gt;&gt;</c> into an <c>Optional&lt;<typeparamref name="T"/>&gt;</c>.
        /// (Note that <c>Optionlt;Optional&lt;<typeparamref name="T"/>&gt;&gt;</c> is not a valid <see cref="Optional{T}"/> because of the
        /// type constraint <c>where T : class</c>.)
        /// </remarks>
        public Optional<U> Then<U>(Func<T, Optional<U>> f)
            => Switch(
                f,
                () => Optional<U>.Empty);

        /// <summary>
        /// Converts an optional value to an enumerable.
        /// The enumerable will have either zero or one elements.
        /// </summary>
        public IEnumerable<T> ToEnumerable()
            => Switch(
                x => new[] { x },
                () => Enumerable.Empty<T>());

        /// <summary>
        /// Returns the value's string representation, or a localized "none" message.
        /// </summary>
        public override string ToString()
            => Switch(
                x => x.ToString(),
                () => Resources.OptionalEmptyToString);

        /// <summary>
        /// Returns the hash code for the underlying type
        /// or zero if there is no value.
        /// </summary>
        public override int GetHashCode()
            => Switch(
                x => x.GetHashCode(),
                () => 0);

        /// <summary>
        /// Determines whether this instance and another object,
        /// which must also be an <see cref="Optional{T}"/>,
        /// have the same value.
        /// </summary>
        public override bool Equals(object obj)
            => obj is Optional<T> optional
            && this.Equals(optional);

        /// <summary>
        /// Determines whether this instance and another <see cref="Optional{T}"/>
        /// have different values.
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
        /// Determines whether two instances of <see cref="Optional{T}"/> have the same value.
        /// </summary>
        public static bool operator ==(Optional<T> lhs, Optional<T> rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Determines whether two instances of <see cref="Optional{T}"/> have different values.
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
    /// Provides additional methods for <seealso cref="Optional{T}"/>.
    /// </summary>
    public static class Optional
    {
        /// <summary>
        /// Makes a value optional.
        /// </summary>
        /// <remarks>
        /// This is useful for type inference in cases where the implicit conversion
        /// can't be used, such as creating an <see cref="Optional{T}"/>
        /// and immediately invoking a method.
        /// It can also be passed as a delegate whereas the constructor can't be.
        /// </remarks>
        public static Optional<T> Of<T>(T value) => new Optional<T>(value);

        /// <summary>
        /// Converts a <see cref="Nullable{T}"/> to <see cref="Optional{T}"/>.
        /// </summary>
        /// <remarks>
        /// This is a special overload for nullable types to avoid ending up with <c>Optional&lt;T?&gt;</c>.
        /// For a value type such as <see cref="int"/>, this method is equivalent to <c>new Optional&lt;int?&gt;(x).Collapse()</c>.
        /// </remarks>
        public static Optional<T> Of<T>(T? value) where T : struct
        {
            if (value.HasValue)
            {
                return new Optional<T>(value.GetValueOrDefault());
            }
            else
            {
                return Optional<T>.Empty;
            }
        }

        /// <summary>
        /// Sets an optional value if a condition is true.
        /// </summary>
        /// <remarks>
        /// This method is useful for converting the <c>TryParse</c> pattern to an <see cref="Optional{T}"/> result.
        /// </remarks>
        public static Optional<T> If<T>(bool condition, T value)
        {
            if (condition)
            {
                return Of(value);
            }
            else
            {
                return Optional<T>.Empty;
            }
        }

        /// <summary>
        /// Sets an optional value if a condition is true.
        /// </summary>
        /// <remarks>
        /// This method is useful for converting the <c>TryParse</c> pattern to an <see cref="Optional{T}"/> result.
        /// </remarks>
        public static Optional<T> If<T>(bool condition, Func<T> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            if (condition)
            {
                return Of(func());
            }
            else
            {
                return Optional<T>.Empty;
            }
        }

        /// <summary>
        /// Converts a unary action to work with <see cref="Optional{T}"/>.
        /// </summary>
        public static Action<Optional<T>> Lift<T>(Action<T> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return optional => optional.IfValue(action);
        }

        /// <summary>
        /// Converts a unary function to work with <see cref="Optional{T}"/>.
        /// </summary>
        public static Func<Optional<T>, Optional<TResult>> Lift<T, TResult>(Func<T, TResult> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return optional => optional.OnValue(func);
        }

        /// <summary>
        /// Converts an <c>Optional&lt;Optional&lt;T&gt;&gt;</c>
        /// to an <see cref="Optional{T}"/>.
        /// </summary>
        public static Optional<T> Flatten<T>(this Optional<Optional<T>> optionalOptional)
            => optionalOptional.Then(x => x);

        /// <summary>
        /// Converts an <c>Optional&lt;Nullable&lt;T&gt;&gt;</c>
        /// to an <see cref="Optional{T}"/>.
        /// </summary>
        public static Optional<T> Flatten<T>(this Optional<T?> optionalNullable) where T : struct
            => optionalNullable.Then(Of);

        /// <summary>
        /// Collects the non-empty values from the sequence.
        /// </summary>
        public static IEnumerable<T> NonEmpty<T>(this IEnumerable<Optional<T>> source)
            => source.SelectMany(
                optional => optional.ToEnumerable());

        /// <summary>
        /// Converts an <c>Optional&lt;Task&lt;T&gt;&gt;</c>
        /// to a <c>Task&lt;Optional&lt;T&gt;&gt;</c>.
        /// </summary>
        public static Task<Optional<T>> AwaitAsync<T>(this Optional<Task<T>> optionalTask)
            => optionalTask.Switch(
                async x => Of(await x),
                () => Task.FromResult(Optional<T>.Empty));
    }
}
