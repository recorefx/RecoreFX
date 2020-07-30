using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Xunit;

namespace Recore.Tests
{
    public class OptionalTests
    {
        [Fact]
        public void Constructor()
        {
            var referenceOptional = new Optional<string>("Hello world");
            Assert.NotEmpty(referenceOptional);

            var valueOptional = new Optional<int>(123);
            Assert.NotEmpty(valueOptional);

            var nullOptional = new Optional<object>(null);
            Assert.Empty(nullOptional);
        }

        [Fact]
        public void DefaultConstructor()
        {
            var referenceOptional = new Optional<object>();
            Assert.Empty(referenceOptional);

            var valueOptional = new Optional<int>();
            Assert.Empty(valueOptional);
        }

        [Fact]
        public void Empty()
        {
            var referenceOptional = Optional<object>.Empty;
            Assert.Empty(referenceOptional);

            var valueOptional = Optional<int>.Empty;
            Assert.Empty(valueOptional);
        }

        [Fact]
        public void SwitchFunc()
        {
            Optional<int> optional;
            int result;

            optional = 123;
            result = optional.Switch(
                x => 1,
                () => throw new Exception("Should not be called"));
            Assert.Equal(1, result);

            optional = Optional<int>.Empty;
            result = optional.Switch(
                x => throw new Exception("Should not be called"),
                () => 10);
            Assert.Equal(10, result);

            Assert.Throws<ArgumentNullException>(
                () => optional.Switch(
                    null,
                    () => throw new Exception("Should not be called")));

            Assert.Throws<ArgumentNullException>(
                () => optional.Switch(
                    x => throw new Exception("Should not be called"),
                    null));
        }

        [Fact]
        public void SwitchAction()
        {
            Optional<int> optional;
            bool called;

            optional = 123;
            called = false;
            optional.Switch(
                x => { called = true; },
                () => throw new Exception("Should not be called"));
            Assert.True(called);

            optional = Optional<int>.Empty;
            called = false;
            optional.Switch(
                x => throw new Exception("Should not be called"),
                () => { called = true; });
            Assert.True(called);

            Assert.Throws<ArgumentNullException>(
                () => optional.Switch(
                    null,
                    () => throw new Exception("Should not be called")));

            Assert.Throws<ArgumentNullException>(
                () => optional.Switch(
                    x => throw new Exception("Should not be called"),
                    null));
        }

        [Fact]
        public void ValueOr()
        {
            Optional<string> optional;

            optional = "hello world";
            Assert.Equal("hello world", optional.ValueOr("xxx"));

            optional = null;
            Assert.Equal("xxx", optional.ValueOr("xxx"));
        }

        [Fact]
        public void OnValue()
        {
            Optional<int> optional;
            int Square(int n) => n * n;

            optional = 10;
            Assert.Equal(100, optional.OnValue(Square));

            optional = Optional<int>.Empty;
            Assert.Empty(optional.OnValue(Square));

            optional = 100;
            Assert.Equal("100", optional.OnValue(x => x.ToString()));
        }

        [Fact]
        public void IfValueIfEmpty()
        {
            Optional<int> optional;
            bool called;

            optional = 123;
            called = false;
            optional.IfValue(x => { called = true; });
            Assert.True(called);

            called = false;
            optional.IfEmpty(() => { called = true; });
            Assert.False(called);

            optional = Optional<int>.Empty;
            called = false;
            optional.IfValue(x => { called = true; });
            Assert.False(called);

            called = false;
            optional.IfEmpty(() => { called = true; });
            Assert.True(called);
        }

        [Fact]
        public void FunctorLaws()
        {
            // Identity
            Optional<string> referenceOptional = "hello world";
            Optional<int> valueOptional = 123;
            T Id<T>(T t) => t;

            Assert.Equal("hello world", referenceOptional.OnValue(Id));
            Assert.Equal(123, valueOptional.OnValue(Id));
            Assert.Equal(Optional<object>.Empty, Optional<object>.Empty.OnValue(Id));

            // Composition
            int Square(int n) => n * n;
            int PlusTwo(int n) => n + 2;

            Assert.Equal(
                new Optional<int>(10).OnValue(Square).OnValue(PlusTwo),
                new Optional<int>(10).OnValue(x => PlusTwo(Square(x))));

            Assert.Equal(
                Optional<int>.Empty.OnValue(Square).OnValue(PlusTwo),
                Optional<int>.Empty.OnValue(x => PlusTwo(Square(x))));
        }

        [Fact]
        public void Then()
        {
            Optional<int> FindFirstSpace(string str)
            {
                var result = str.IndexOf(' ');
                if (result == -1)
                {
                    return Optional<int>.Empty;
                }
                else
                {
                    return result;
                }
            }

            Optional<string> optionalString;
            Optional<int> actual;

            optionalString = "hello world";
            actual = optionalString.Then(FindFirstSpace);
            Assert.Equal(5, actual);

            optionalString = "hello";
            actual = optionalString.Then(FindFirstSpace);
            Assert.Empty(actual);

            optionalString = Optional<string>.Empty;
            actual = optionalString.Then(FindFirstSpace);
            Assert.Empty(actual);
        }

        [Fact]
        public void MonadLaws()
        {
            Optional<string> StringToOption(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return Optional<string>.Empty;
                }
                else
                {
                    return str;
                }
            }

            // Left identity
            foreach (var value in new[] { "hello", string.Empty, null })
            {
                Assert.Equal(
                    StringToOption(value),
                    new Optional<string>(value).Then(StringToOption));
            }

            // Right identity
            foreach (var optional in new[] { "hello", null, Optional<string>.Empty })
            {
                Assert.Equal(
                    optional,
                    optional.Then(Optional.Of<string>));
            }

            // Associativity
            Optional<int> NullsafeLength(string str)
            {
                if (str == null)
                {
                    return Optional<int>.Empty;
                }
                else
                {
                    return str.Length;
                }
            }

            foreach (var optional in new[] { "hello", null, Optional<string>.Empty })
            {
                Assert.Equal(
                    optional.Then(StringToOption).Then(NullsafeLength),
                    optional.Then(x => StringToOption(x).Then(NullsafeLength)));
            }
        }

        [Fact]
        public void ToString_()
        {
            Assert.Equal("hello", Optional.Of("hello").ToString());
            Assert.Equal("none", Optional<string>.Empty.ToString());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Equals_(int value)
        {
            // object.Equals
            Assert.True(
                Equals(Optional.Of(value), Optional.Of(value)));

            Assert.True(
                Equals(new Optional<string>(), new Optional<string>()));

            Assert.False(
                Equals(Optional.Of(value), Optional<string>.Empty));

            // Optional.Equals
            Assert.True(
                Optional.Of(value).Equals(Optional.Of(value)));

            Assert.True(
                Optional<int>.Empty.Equals(Optional<int>.Empty));

            Assert.False(
                Optional<int>.Empty.Equals(Optional.Of(value)));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void EqualityOperators(int value)
        {
            // operator==
            Assert.True(
                Optional.Of(value) == Optional.Of(value));

            Assert.True(
                new Optional<int>() == new Optional<int>());

            Assert.False(
                Optional.Of(0) == Optional<int>.Empty);

            // operator!=
            Assert.False(
                Optional.Of(value) != Optional.Of(value));

            Assert.False(
                new Optional<int>() != new Optional<int>());

            Assert.True(
                Optional.Of(value) != Optional<int>.Empty);
        }

        [Fact]
        public void EqualsEquivalenceRelation()
        {
            // Reflexive
            Optional<int> a = 1;
            Assert.Equal(a, a);

            // Symmetric
            Optional<int> b = 1;
            Assert.Equal(a, b);
            Assert.Equal(b, a);

            // Transitive
            Optional<int> c = 1;
            Assert.Equal(b, c);
            Assert.Equal(a, c);
        }

        [Fact]
        public void GetHashCode_()
        {
            Optional<string> optional = "hello";

            // Idempotence
            Assert.Equal(optional.GetHashCode(), optional.GetHashCode());

            // Does not depend on object identity
            Assert.Equal(optional.GetHashCode(), Optional.Of("hello").GetHashCode());
        }

        [Fact]
        public void GetEnumerator()
        {
            {
                var optional = Optional.Of(123);
                var enumerator = optional.GetEnumerator();

                Assert.True(enumerator.MoveNext());
                Assert.Equal(123, enumerator.Current);
                Assert.False(enumerator.MoveNext());

                var numberOfElements = 0;
                foreach (var item in optional)
                {
                    Assert.Equal(123, item);
                    numberOfElements++;
                }

                Assert.Equal(1, numberOfElements);
            }
            {
                var optional = Optional<int>.Empty;
                var enumerator = optional.GetEnumerator();

                Assert.False(enumerator.MoveNext());

                foreach (var item in optional)
                {
                    Assert.True(false);
                }
            }
        }

        [Fact]
        public void Cast()
        {
            int CountChar(string str, char toCount)
            {
                int total = 0;
                foreach (char c in str)
                {
                    if (c == toCount)
                    {
                        total++;
                    }
                }
                return total;
            }

            Optional<string> optional;

            optional = "hello";
            Assert.Equal(2, CountChar((string)optional, 'l'));

            optional = Optional<string>.Empty;
            Assert.Null((string)optional);
        }

        [Fact]
        public void Of()
        {
            Assert.Equal(new Optional<int>(12), Optional.Of(12));
        }

        [Fact]
        public void If()
        {
            var success = Optional.If(int.TryParse("123", out int result), result);
            Assert.Equal(123, success);

            var failure = Optional.If(int.TryParse("abc", out result), result);
            Assert.Empty(failure);
        }

        [Fact]
        public void Flatten()
        {
            var doubleValue = new Optional<Optional<string>>("hello");
            Assert.Equal("hello", doubleValue.Flatten());

            var doubleNone = new Optional<Optional<string>>();
            Assert.Empty(doubleNone.Flatten());

            var valueNone = new Optional<Optional<string>>(Optional<string>.Empty);
            Assert.Empty(valueNone.Flatten());
        }

        [Fact]
        public void NonEmpty()
        {
            var collection = new[]
            {
                "hello",
                Optional<string>.Empty,
                "abc",
                "hello world",
                Optional<string>.Empty
            };

            var nonempty = new[]
            {
                "hello",
                "abc",
                "hello world"
            };

            Assert.Equal(
                nonempty,
                collection.NonEmpty().ToArray());
        }

        [Fact]
        public async Task AwaitAsync()
        {
            var optionalTask = Optional.Of(Task.FromResult(1));
            var optional = await optionalTask.AwaitAsync();
            Assert.Equal(1, optional);

            optional = await Optional<Task<int>>.Empty.AwaitAsync();
            Assert.Empty(optional);
        }

        [Fact]
        public void ToJson()
        {
            Assert.Equal(
                expected: "12",
                actual: JsonSerializer.Serialize(Optional.Of(12)));

            Assert.Equal(
               expected: "null",
               actual: JsonSerializer.Serialize(Optional<int>.Empty));

            Assert.Equal(
                expected: "\"hello\"",
                actual: JsonSerializer.Serialize(Optional.Of("hello")));

            Assert.Equal(
               expected: "null",
               actual: JsonSerializer.Serialize(Optional<string>.Empty));
        }

        [Fact]
        public void FromJson()
        {
            Assert.Equal(
                expected: Optional.Of(12),
                actual: JsonSerializer.Deserialize<Optional<int>>("12"));

            Assert.Equal(
                expected: Optional<int>.Empty,
                actual: JsonSerializer.Deserialize<Optional<int>>("null"));

            Assert.Equal(
                expected: Optional.Of("hello"),
                actual: JsonSerializer.Deserialize<Optional<string>>("\"hello\""));

            Assert.Equal(
                expected: Optional<string>.Empty,
                actual: JsonSerializer.Deserialize<Optional<string>>("null"));
        }
    }
}
