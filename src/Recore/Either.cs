using System;
using System.Collections.Generic;
using System.Linq;

namespace Recore
{
    public sealed class Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>
    {
        private readonly TLeft left;
        private readonly TRight right;

        public bool IsLeft { get; }

        public bool IsRight => !IsLeft;

        public Either(TLeft left)
        {
            this.left = left;
            right = default;
            IsLeft = true;
        }

        public Either(TRight right)
        {
            left = default;
            this.right = right;
            IsLeft = false;
        }

        public T Switch<T>(Func<TLeft, T> onLeft, Func<TRight, T> onRight)
        {
            if (IsLeft)
            {
                return onLeft(left);
            }
            else
            {
                return onRight(right);
            }
        }

        public void Switch(Action<TLeft> onLeft, Action<TRight> onRight)
        {
            if (IsLeft)
            {
                onLeft(left);
            }
            else
            {
                onRight(right);
            }
        }

        public Optional<TLeft> GetLeft()
            => Switch(
                left => new Optional<TLeft>(left),
                right => Optional.Empty<TLeft>());

        public Optional<TRight> GetRight()
            => Switch(
                left => Optional.Empty<TRight>(),
                right => new Optional<TRight>(right));

        /// <summary>
        /// Calls a function only if the <c cref="Either{TLeft, TRight}">Either</c> holds a value of <c>TLeft</c>.
        /// </summary>
        public Either<TResult, TRight> OnLeft<TResult>(Func<TLeft, TResult> onLeft)
            => Switch(
                left => new Either<TResult, TRight>(onLeft(left)),
                right => new Either<TResult, TRight>(right));

        /// <summary>
        /// Calls a function only if the <c cref="Either{TLeft, TRight}">Either</c> holds a value of <c>TRight</c>.
        /// </summary>
        public Either<TLeft, TResult> OnRight<TResult>(Func<TRight, TResult> onRight)
            => Switch(
                left => new Either<TLeft, TResult>(left),
                right => new Either<TLeft, TResult>(onRight(right)));

        /// <summary>
        /// Takes an action only if the <c cref="Either{TLeft, TRight}">Either</c> holds a value of <c>TLeft</c>.
        /// </summary>
        public void IfLeft(Action<TLeft> onLeft)
            => Switch(
                onLeft,
                right => { });

        /// <summary>
        /// Takes an action only if the <c cref="Either{TLeft, TRight}">Either</c> holds a value of <c>TRight</c>.
        /// </summary>
        public void IfRight(Action<TRight> onRight)
            => Switch(
                left => { },
                onRight);

        /// <summary>
        /// Converts this <c cref="Either{TLeft, TRight}">Either&lt;TLeft, TRight&gt;</c>
        /// to an <c cref="Either{TRight, TLeft}">Either&lt;TRight, TLeft&gt;</c>.
        /// </summary>
        public Either<TRight, TLeft> Swap()
            => Switch(
                left => new Either<TRight, TLeft>(left),
                right => new Either<TRight, TLeft>(right));

        public override string ToString()
            => Switch(
                left => left.ToString(),
                right => right.ToString());

        /// <summary>
        /// Compares this <c cref="Either{TLeft, TRight}">Either&lt;TLeft, TRight&gt;</c>
        /// to another object for equality.
        /// </summary>
        /// <remarks>
        /// Two <c>Either</c>s are equal only if they have the same type parameters in the same order.
        /// For example, an <c>Either&lt;int, string&gt;</c> and an <c>Either&lt;string, int&gt;</c>
        /// will always be nonequal.
        /// </remarks>
        public override bool Equals(object obj)
            => obj is Either<TLeft, TRight>
            && this.Equals((Either<TLeft, TRight>)obj);

        /// <summary>
        /// Compares two <c cref="Either{TLeft, TRight}">Either&lt;TLeft, TRight&gt;</c>
        /// instances for equality.
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
                left => other.Switch(
                    otherLeft => Equals(left, otherLeft),
                    otherRight => false),
                right => other.Switch(
                    otherLeft => false,
                    otherRight => Equals(right, otherRight)));

        public override int GetHashCode()
            => Switch(
                left => left.GetHashCode(),
                right => right.GetHashCode());

        public static bool operator==(Either<TLeft, TRight> lhs, Either<TLeft, TRight> rhs) => Equals(lhs, rhs);
        public static bool operator!=(Either<TLeft, TRight> lhs, Either<TLeft, TRight> rhs) => !Equals(lhs, rhs);

        public static implicit operator Either<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);
        public static implicit operator Either<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);
    }

    /// <summary>
    /// Provides additional methods for <c>Either</c>.
    /// </summary>
    public static class Either
    {
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