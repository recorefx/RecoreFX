using System;

namespace Recore
{
    public sealed class Either<TLeft, TRight>
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
        /// Takes an action only if the <c cref="Either{TLeft, TRight}">Either</c> holds a value of <c>TLeft</c>.
        /// </summary>
        public void IfLeft(Action<TLeft> onLeft)
            => Switch(
                onLeft,
                left => { });

        /// <summary>
        /// Takes an action only if the <c cref="Either{TLeft, TRight}">Either</c> holds a value of <c>TRight</c>.
        /// </summary>
        public void IfRight(Action<TRight> onRight)
            => Switch(
                right => { },
                onRight);

        public static implicit operator Either<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);
        public static implicit operator Either<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);

        // TODO Equals, ToString()
    }
}