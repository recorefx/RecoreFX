using System;
using System.Linq;
using System.Text.Json;

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
                left => 1,
                right => throw new Exception("Should not be called"));
            Assert.Equal(1, result);

            either = 12;
            result = either.Switch(
                left => throw new Exception("Should not be called"),
                right => right * 2);
            Assert.Equal(24, result);

            Assert.Throws<ArgumentNullException>(
                () => either.Switch(
                    left => throw new Exception("Should not be called"),
                    null));

            Assert.Throws<ArgumentNullException>(
                () => either.Switch(
                    null,
                    right => throw new Exception("Should not be called")));
        }

        [Fact]
        public void SwitchAction()
        {
            Either<string, int> either;
            bool called;

            either = "hello";
            called = false;
            either.Switch(
                left => { called = true; },
                right => throw new Exception("Should not be called"));
            Assert.True(called);

            either = 12;
            called = false;
            either.Switch(
                left => throw new Exception("Should not be called"),
                right => { called = true; });
            Assert.True(called);

            Assert.Throws<ArgumentNullException>(
                () => either.Switch(
                    left => throw new Exception("Should not be called"),
                    null));

            Assert.Throws<ArgumentNullException>(
                () => either.Switch(
                    null,
                    right => throw new Exception("Should not be called")));
        }

        [Fact]
        public void GetLeftGetRight()
        {
            var left = new Either<int, string>(-5);
            Assert.Equal(-5, left.GetLeft());
            Assert.Empty(left.GetRight());

            var right = new Either<int, string>("hello");
            Assert.Empty(right.GetLeft());
            Assert.Equal("hello", right.GetRight());
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
                new Either<int, string>("abc").Equals((Either<int, string>)null));
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

        [Fact]
        public void ToJson()
        {
            {
                Either<int, string> either;

                either = 12;
                Assert.Equal(
                    expected: "12",
                    actual: JsonSerializer.Serialize(either));

                either = "hello";
                Assert.Equal(
                    expected: "\"hello\"",
                    actual: JsonSerializer.Serialize(either));
            }
            {
                Either<string, int> either;

                either = 12;
                Assert.Equal(
                    expected: "12",
                    actual: JsonSerializer.Serialize(either));

                either = "hello";
                Assert.Equal(
                    expected: "\"hello\"",
                    actual: JsonSerializer.Serialize(either));
            }
        }

        [Fact]
        public void FromJson()
        {
            Assert.Equal(
                expected: new Either<int, string>(12),
                actual: JsonSerializer.Deserialize<Either<int, string>>("12"));

            Assert.Equal(
                expected: new Either<int, string>("hello"),
                actual: JsonSerializer.Deserialize<Either<int, string>>("\"hello\""));

            Assert.Equal(
                expected: new Either<string, int>(12),
                actual: JsonSerializer.Deserialize<Either<string, int>>("12"));

            Assert.Equal(
                expected: new Either<string, int>("hello"),
                actual: JsonSerializer.Deserialize<Either<string, int>>("\"hello\""));
        }
    }
}