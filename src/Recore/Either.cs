using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

using Recore.Text.Json.Serialization.Converters;

namespace Recore
{
    /// <summary>
    /// Represents a value that can be one of two types.
    /// </summary>
    [JsonConverter(typeof(EitherConverter))]
    public sealed class Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>
    {
        private readonly TLeft left;
        private readonly TRight right;

        /// <summary>
        /// Indicates whether the value is of type <typeparamref name="TLeft"/>.
        /// </summary>
        public bool IsLeft { get; }

        /// <summary>
        /// Indicates whether the value is of type <typeparamref name="TRight"/>.
        /// </summary>
        public bool IsRight => !IsLeft;

        /// <summary>
        /// Constructs an instance of the type from a value of <typeparamref name="TLeft"/>.
        /// </summary>
        public Either(TLeft left)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            this.left = left;
            right = default;
            IsLeft = true;
        }

        /// <summary>
        /// Constructs an instance of the type from a value of <typeparamref name="TRight"/>.
        /// </summary>
        public Either(TRight right)
        {
            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            left = default;
            this.right = right;
            IsLeft = false;
        }

        /// <summary>
        /// Calls one of two functions depending on the underlying value.
        /// </summary>
        public T Switch<T>(Func<TLeft, T> onLeft, Func<TRight, T> onRight)
        {
            if (onLeft is null)
            {
                throw new ArgumentNullException(nameof(onLeft));
            }
            if (onRight is null)
            {
                throw new ArgumentNullException(nameof(onLeft));
            }

            if (IsLeft)
            {
                return onLeft(left);
            }
            else
            {
                return onRight(right);
            }
        }

        /// <summary>
        /// Takes one of two actions depending on the underlying value.
        /// </summary>
        public void Switch(Action<TLeft> onLeft, Action<TRight> onRight)
        {
            if (onLeft is null)
            {
                throw new ArgumentNullException(nameof(onLeft));
            }
            if (onRight is null)
            {
                throw new ArgumentNullException(nameof(onLeft));
            }

            if (IsLeft)
            {
                onLeft(left);
            }
            else
            {
                onRight(right);
            }
        }

        /// <summary>
        /// Converts <see cref="Either{TLeft, TRight}"/> to <c cref="Optional{T}">Optional&lt;TLeft&gt;</c>.
        /// </summary>
        public Optional<TLeft> GetLeft()
            => Switch(
                Optional.Of,
                r => Optional<TLeft>.Empty);

        /// <summary>
        /// Converts <see cref="Either{TLeft, TRight}"/> to <c cref="Optional{T}">Optional&lt;TRight&gt;</c>.
        /// </summary>
        public Optional<TRight> GetRight()
            => Switch(
                l => Optional<TRight>.Empty,
                Optional.Of);

        /// <summary>
        /// Maps a function over the <see cref="Either{TLeft, TRight}"/> only if the value is an instance of <typeparamref name="TLeft"/>.
        /// </summary>
        public Either<TResult, TRight> OnLeft<TResult>(Func<TLeft, TResult> onLeft)
            => Switch(
                l => new Either<TResult, TRight>(onLeft(l)),
                r => new Either<TResult, TRight>(r));

        /// <summary>
        /// Maps a function over the <see cref="Either{TLeft, TRight}"/> only if the value is an instance of <typeparamref name="TRight"/>.
        /// </summary>
        public Either<TLeft, TResult> OnRight<TResult>(Func<TRight, TResult> onRight)
            => Switch(
                l => new Either<TLeft, TResult>(l),
                r => new Either<TLeft, TResult>(onRight(r)));

        /// <summary>
        /// Takes an action only if the value is an instance of <typeparamref name="TLeft"/>.
        /// </summary>
        public void IfLeft(Action<TLeft> onLeft)
            => Switch(
                onLeft,
                r => { });

        /// <summary>
        /// Takes an action only if the value is an instance of <typeparamref name="TRight"/>.
        /// </summary>
        public void IfRight(Action<TRight> onRight)
            => Switch(
                l => { },
                onRight);

        /// <summary>
        /// Converts this <see cref="Either{TLeft, TRight}"/>
        /// to an <c>Either&lt;TRight, TLeft&gt;</c>
        /// </summary>
        public Either<TRight, TLeft> Swap()
            => Switch(
                l => new Either<TRight, TLeft>(l),
                r => new Either<TRight, TLeft>(r));

        /// <summary>
        /// Returns the string representation of the underlying value.
        /// </summary>
        public override string ToString()
            => Switch(
                l => l.ToString(),
                r => r.ToString());

        /// <summary>
        /// Compares this <see cref="Either{TLeft, TRight}"/>
        /// to another object for equality.
        /// </summary>
        /// <remarks>
        /// Two <see cref="Either{TLeft, TRight}"/>s are equal only if they have the same type parameters in the same order.
        /// For example, an <c>Either&lt;int, string&gt;</c> and an <c>Either&lt;string, int&gt;</c>
        /// will always be nonequal.
        /// </remarks>
        public override bool Equals(object obj)
            => obj is Either<TLeft, TRight> either
            && this.Equals(either);

        /// <summary>
        /// Compares two instances of <see cref="Either{TLeft, TRight}"/>
        /// for equality.
        /// </summary>
        /// <remarks>
        /// Equality is defined as both objects' underlying values being equal
        /// and their underlying values occupying the same position (both left or both right).
        /// For example, <c>Either&lt;Color, Day&gt;(Color.Red) != Either&lt;Color, Day&gt;(Day.Monday)</c>
        /// even if <c>Color.Red == Day.Monday</c>.
        /// </remarks>
        public bool Equals(Either<TLeft, TRight> other)
            => other != null
            && Switch(
                l => other.Switch(
                    otherLeft => Equals(l, otherLeft),
                    otherRight => false),
                r => other.Switch(
                    otherLeft => false,
                    otherRight => Equals(r, otherRight)));

        /// <summary>
        /// Returns the hash code of the underlying value.
        /// </summary>
        public override int GetHashCode()
            => Switch(
                l => l.GetHashCode(),
                r => r.GetHashCode());

        /// <summary>
        /// Determines whether two instances of <see cref="Either{TLeft, TRight}"/>
        /// have the same value.
        /// </summary>
        public static bool operator ==(Either<TLeft, TRight> lhs, Either<TLeft, TRight> rhs) => Equals(lhs, rhs);

        /// <summary>
        /// Determines whether two instances of <see cref="Either{TLeft, TRight}"/>
        /// have different values.
        /// </summary>
        public static bool operator !=(Either<TLeft, TRight> lhs, Either<TLeft, TRight> rhs) => !Equals(lhs, rhs);

        /// <summary>
        /// Converts an instance of a type to an <see cref="Either{TLeft, TRight}"/>.
        /// </summary>
        public static implicit operator Either<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);

        /// <summary>
        /// Converts an instance of a type to an <see cref="Either{TLeft, TRight}"/>.
        /// </summary>
        public static implicit operator Either<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);
    }

    /// <summary>
    /// Provides additional methods for <seealso cref="Either{TLeft, TRight}"/>.
    /// </summary>
    public static class Either
    {
        /// <summary>
        /// Combines two unary actions into a single action taking either of their parameters.
        /// </summary>
        public static Action<Either<TLeft, TRight>> Lift<TLeft, TRight>(
            Action<TLeft> leftAction,
            Action<TRight> rightAction)
        {
            if (leftAction is null)
            {
                throw new ArgumentNullException(nameof(leftAction));
            }
            if (rightAction is null)
            {
                throw new ArgumentNullException(nameof(rightAction));
            }

            return either => either.Switch(leftAction, rightAction);
        }

        /// <summary>
        /// Combines two unary functions with the same return type
        /// into a single function taking either of their parameters.
        /// </summary>
        public static Func<Either<TLeft, TRight>, TResult> Lift<TLeft, TRight, TResult>(
            Func<TLeft, TResult> leftFunc,
            Func<TRight, TResult> rightFunc)
        {
            if (leftFunc is null)
            {
                throw new ArgumentNullException(nameof(leftFunc));
            }
            if (rightFunc is null)
            {
                throw new ArgumentNullException(nameof(rightFunc));
            }

            return either => either.Switch(leftFunc, rightFunc);
        }

        /// <summary>
        /// Collects all the left-side values from the sequence.
        /// </summary>
        public static IEnumerable<TLeft> Lefts<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> source)
            => source
            .Select(x => x.GetLeft())
            .NonEmpty();

        /// <summary>
        /// Collects all the right-side values from the sequence.
        /// </summary>
        public static IEnumerable<TRight> Rights<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> source)
            => source
            .Select(x => x.GetRight())
            .NonEmpty();
    }
}