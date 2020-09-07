using System;
using System.Linq;

using Xunit;

namespace Recore.Tests
{
    public class EitherTests
    {
        [Fact]
        public void Constructor()
        {
            var left = new Either<int, bool>(-5);
            Assert.True(left.IsLeft);
            Assert.False(left.IsRight);

            var right = new Either<int, bool>(false);
            Assert.False(right.IsLeft);
            Assert.True(right.IsRight);
        }

        [Fact]
        public void SwitchFunc()
        {
            Either<string, int> either;
            int result;

            either = "hello";
            result = either.Switch(
                l => 1,
                r => throw new Exception("Should not be called"));
            Assert.Equal(1, result);

            either = 12;
            result = either.Switch(
                l => throw new Exception("Should not be called"),
                r => r * 2);
            Assert.Equal(24, result);

            Assert.Throws<ArgumentNullException>(
                () => either.Switch(
                    l => throw new Exception("Should not be called"),
                    null));

            Assert.Throws<ArgumentNullException>(
                () => either.Switch(
                    null,
                    r => throw new Exception("Should not be called")));
        }

        [Fact]
        public void SwitchAction()
        {
            Either<string, int> either;
            bool called;

            either = "hello";
            called = false;
            either.Switch(
                l => { called = true; },
                r => throw new Exception("Should not be called"));
            Assert.True(called);

            either = 12;
            called = false;
            either.Switch(
                l => throw new Exception("Should not be called"),
                r => { called = true; });
            Assert.True(called);

            Assert.Throws<ArgumentNullException>(
                () => either.Switch(
                    l => throw new Exception("Should not be called"),
                    null));

            Assert.Throws<ArgumentNullException>(
                () => either.Switch(
                    null,
                    r => throw new Exception("Should not be called")));
        }

        [Fact]
        public void GetLeftGetRight()
        {
            var left = new Either<int, string>(-5);
            Assert.Equal(-5, left.GetLeft());
            Assert.False(left.GetRight().HasValue);

            var right = new Either<int, string>("hello");
            Assert.False(right.GetLeft().HasValue);
            Assert.Equal("hello", right.GetRight());
        }

        [Fact]
        public void OnLeftOnRight()
        {
            Either<int, string> either;

            either = 123;
            Assert.Equal(1, either.OnLeft(x => 1));
            Assert.Equal(123, either.OnRight(x => 1).Collapse());

            either = "hello";
            Assert.Equal("hello", either.OnLeft(x => 1));
            Assert.Equal(1, either.OnRight(x => 1).Collapse());
        }

        [Fact]
        public void IfLeftIfRight()
        {
            Either<int, string> either;
            bool called;

            either = 123;
            called = false;
            either.IfLeft(x => { called = true; });
            Assert.True(called);

            called = false;
            either.IfRight(x => { called = true; });
            Assert.False(called);

            either = string.Empty;
            called = false;
            either.IfLeft(x => { called = true; });
            Assert.False(called);

            called = false;
            either.IfRight(x => { called = true; });
            Assert.True(called);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "abc")]
        public void Equals_(int left, string right)
        {
            // object.Equals
            Assert.True(
                Equals(new Either<int, string>(left), new Either<int, string>(left)));

            Assert.True(
                Equals(new Either<int, string>(right), new Either<int, string>(right)));

            Assert.False(
                Equals(new Either<int, string>(left), Optional<string>.Empty));

            // Either.Equals
            Assert.True(
                new Either<int, string>(left).Equals(new Either<int, string>(left)));

            Assert.True(
                new Either<int, string>(right).Equals(new Either<int, string>(right)));

            Assert.False(
                new Either<int, string>(right).Equals(new Either<int, string>(left)));
        }

        [Fact]
        public void EqualsWithNull()
        {
            Assert.False(
                new Either<int, string>("abc").Equals(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void SwapEquals(int value)
        {
            Assert.True(
                Equals(new Either<int, string>(value), new Either<string, int>(value).Swap()));

            Assert.True(
                new Either<int, string>(value).Equals(new Either<string, int>(value).Swap()));

            Assert.True(
                new Either<string, int>(value).Equals(new Either<int, string>(value).Swap()));
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "abc")]
        public void EqualityOperators(int left, string right)
        {
            // operator==
            Assert.True(
                new Either<int, string>(left) == new Either<int, string>(left));

            Assert.True(
                new Either<int, string>(right) == new Either<int, string>(right));

            Assert.False(
                new Either<int, string>(left) == new Either<int, string>(right));

            // operator!=
            Assert.False(
                new Either<int, string>(left) != new Either<int, string>(left));

            Assert.False(
                new Either<int, string>(right) != new Either<int, string>(right));

            Assert.True(
                new Either<int, string>(left) != new Either<int, string>(right));
        }

        [Fact]
        public void EqualsEquivalenceRelation()
        {
            // Reflexive
            var a = new Either<int, int>(left: 1);
            Assert.Equal(a, a);

            // Symmetric
            var b = new Either<string, int>(1).OnLeft(x => x.Length);
            Assert.NotEqual(a, b); // a is left and b is right

            b = b.Swap();
            Assert.Equal(a, b);
            Assert.Equal(b, a);

            // Transitive
            var c = new Either<int, Uri>(1).OnRight(x => x.Port);
            Assert.Equal(b, c);
            Assert.Equal(a, c);
        }

        [Fact]
        public void GetHashCode_()
        {
            Either<string, double> either = "hello";

            Assert.Equal(either.GetHashCode(), either.GetHashCode());
            Assert.Equal(either.GetHashCode(), new Either<string, Uri>("he" + "llo").GetHashCode());
        }

        [Fact]
        public void ToString_()
        {
            var left = new Either<int, string>(-5);
            Assert.Equal("-5", left.ToString());

            var right = new Either<int, string>("hello");
            Assert.Equal("hello", right.ToString());
        }

        [Fact]
        public void Lift_ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => Either.Lift<string, int>(null, x => { }));

            Assert.Throws<ArgumentNullException>(
                () => Either.Lift<string, int>(x => { }, null));

            Assert.Throws<ArgumentNullException>(
                () => Either.Lift<string, int, int>(null, x => 1));

            Assert.Throws<ArgumentNullException>(
                () => Either.Lift<string, int, int>(x => 1, null));
        }

        [Fact]
        public void LiftAction()
        {
            bool leftCalled = false;
            bool rightCalled = false;
            var lifted = Either.Lift(
                (string s) => { leftCalled = true; },
                (int n) => { rightCalled = true; });

            Assert.False(leftCalled);
            Assert.False(rightCalled);

            Either<string, int> either;

            either = "hello";
            lifted(either);
            Assert.True(leftCalled);
            Assert.False(rightCalled);

            either = 1;
            lifted(either);
            Assert.True(rightCalled);
        }

        [Fact]
        public void LiftFunc()
        {
            var lifted = Either.Lift(
                (string s) => 0,
                (int n) => 1);

            Either<string, int> either;

            either = "hello";
            lifted(either);
            Assert.Equal(0, lifted(either));

            either = 1;
            Assert.Equal(1, lifted(either));
        }

        [Fact]
        public void Lefts()
        {
            var collection = new Either<string, int>[]
            {
                string.Empty,
                "abc",
                1,
                "5",
                23,
                "hello world"
            };

            var lefts = new[]
            {
                string.Empty,
                "abc",
                "5",
                "hello world"
            };

            Assert.Equal(lefts, collection.Lefts().ToArray());
        }

    [   Fact]
        public void Rights()
        {
            var collection = new Either<string, int>[]
            {
                string.Empty,
                "abc",
                1,
                "5",
                23,
                "hello world"
            };

            var rights = new[]
            {
                1,
                23
            };

            Assert.Equal(rights, collection.Rights().ToArray());
        }
    }
}