using System;
using System.Linq;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace Recore.Tests
{
    public class OptionalTests
    {
        [Fact]
        public void Constructor()
        {
            var referenceOptional = new Optional<string>("Hello world");
            Assert.True(referenceOptional.HasValue);

            var valueOptional = new Optional<int>(123);
            Assert.True(valueOptional.HasValue);

            var nullOptional = new Optional<object>(null);
            Assert.False(nullOptional.HasValue);
        }

        [Fact]
        public void DefaultConstructor()
        {
            var referenceOptional = new Optional<object>();
            Assert.False(referenceOptional.HasValue);

            var valueOptional = new Optional<int>();
            Assert.False(valueOptional.HasValue);
        }

        [Fact]
        public void Empty()
        {
            var referenceOptional = Optional<object>.Empty;
            Assert.False(referenceOptional.HasValue);

            var valueOptional = Optional<int>.Empty;
            Assert.False(valueOptional.HasValue);
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

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public void ValueOr_Associativity(int? value)
        {
            Assert.Equal(
                Optional.Of(value ?? 0),
                Optional.Of(value).ValueOr(0));
        }

        [Fact]
        public void AssertValue()
        {
            Optional<string> optional;

            optional = "hello world";
            Assert.Equal("hello world", optional.AssertValue());

            optional = null;
            Assert.Throws<InvalidOperationException>(() => optional.AssertValue());
        }

        [Fact]
        public void OnValue()
        {
            Optional<int> optional;
            int Square(int n) => n * n;

            optional = 10;
            Assert.Equal(100, optional.OnValue(Square));

            optional = Optional<int>.Empty;
            Assert.False(optional.OnValue(Square).HasValue);

            optional = 100;
            Assert.Equal("100", optional.OnValue(x => x.ToString()));
        }

        [Fact]
        public void IfValue()
        {
            Optional<int> optional;
            bool called;

            optional = 123;
            called = false;
            optional.IfValue(x => { called = true; });
            Assert.True(called);

            optional = Optional<int>.Empty;
            called = false;
            optional.IfValue(x => { called = true; });
            Assert.False(called);
        }

        [Fact]
        public void FunctorLaws_Identity()
        {
            Optional<string> referenceOptional = "hello world";
            Optional<int> valueOptional = 123;
            T Id<T>(T t) => t;

            Assert.Equal("hello world", referenceOptional.OnValue(Id));
            Assert.Equal(123, valueOptional.OnValue(Id));
            Assert.Equal(Optional<object>.Empty, Optional<object>.Empty.OnValue(Id));
        }

        [Fact]
        public void FunctorLaws_Composition()
        {
            int Square(int n) => n * n;
            int PlusTwo(int n) => n + 2;

            Assert.Equal(
                new Optional<int>(10).OnValue(Square).OnValue(PlusTwo),
                new Optional<int>(10).OnValue(x => PlusTwo(Square(x))));

            Assert.Equal(
                Optional<int>.Empty.OnValue(Square).OnValue(PlusTwo),
                Optional<int>.Empty.OnValue(x => PlusTwo(Square(x))));
        }

        // A bunch of functions for testing monad properties
        static class MonadFuncs
        {
            public static Optional<int> FindFirstSpace(string str)
            {
                if (str == null)
                {
                    return Optional<int>.Empty;
                }

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

            public static Optional<string> StringToOptional(string str)
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

            public static Optional<int> NullableToOptional(int? n)
                => Optional.Of(n ?? 0);

            public static Optional<int> NullsafeLength(string str) => Optional.Of(str).OnValue(x => x.Length);
        }

        [Fact]
        public void Then()
        {
            Optional<string> optionalString;
            Optional<int> actual;

            optionalString = "hello world";
            actual = optionalString.Then(MonadFuncs.FindFirstSpace);
            Assert.Equal(5, actual);

            optionalString = "hello";
            actual = optionalString.Then(MonadFuncs.FindFirstSpace);
            Assert.False(actual.HasValue);

            optionalString = Optional<string>.Empty;
            actual = optionalString.Then(MonadFuncs.FindFirstSpace);
            Assert.False(actual.HasValue);
        }

        [Property]
        public void MonadLaws_LeftIdentity_FindFirstSpace(string value)
        {
            Assert.Equal(
                MonadFuncs.FindFirstSpace(value),
                new Optional<string>(value).Then(MonadFuncs.FindFirstSpace));
        }

        [Property]
        public void MonadLaws_LeftIdentity_StringToOptional(string value)
        {
            Assert.Equal(
                MonadFuncs.StringToOptional(value),
                new Optional<string>(value).Then(MonadFuncs.StringToOptional));
        }

        [Property]
        public void MonadLaws_LeftIdentity_NullableToOptional(int? value)
        {
            Assert.Equal(
                MonadFuncs.NullableToOptional(value),
                Optional.Of(value).Then(x => MonadFuncs.NullableToOptional(x)));
        }

        [Property]
        public void MonadLaws_LeftIdentity_NullsafeLength(string value)
        {
            Assert.Equal(
                MonadFuncs.NullsafeLength(value),
                new Optional<string>(value).Then(MonadFuncs.NullsafeLength));
        }

        [Property]
        public void MonadLaws_RightIdentity(string value)
        {
            var optional = Optional.Of(value);
            Assert.Equal(
                optional,
                optional.Then(Optional.Of));
        }

        [Property]
        public void MonadLaws_Associativity_StringToOptional_NullsafeLength(string value)
        {
            var optional = Optional.Of(value);
            Assert.Equal(
                optional.Then(MonadFuncs.StringToOptional).Then(MonadFuncs.NullsafeLength),
                optional.Then(x => MonadFuncs.StringToOptional(x).Then(MonadFuncs.NullsafeLength)));
        }

        [Fact]
        public void ToString_()
        {
            Assert.Equal("hello", Optional.Of("hello").ToString());
            Assert.Equal("none", Optional<string>.Empty.ToString());
        }

        [Property]
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

        [Property]
        public void Equals_EquivalenceRelation_Reflexive(int? a)
        {
            var optionalA = Optional.Of(a);
            Assert.Equal(optionalA, optionalA);
        }

        [Property]
        public void Equals_EquivalenceRelation_Symmetric(int? a, int? b)
        {
            var optionalA = Optional.Of(a);
            var optionalB = Optional.Of(b);

            if (Equals(optionalA, optionalB))
            {
                Assert.Equal(b, a);
            }
            else
            {
                Assert.NotEqual(b, a);
            }
        }

        [Property]
        public void Equals_EquivalenceRelation_Transitive(int? a, int? b, int? c)
        {
            var optionalA = Optional.Of(a);
            var optionalB = Optional.Of(b);
            var optionalC = Optional.Of(c);

            if (Equals(optionalA, optionalB)
                && Equals(optionalB, optionalC))
            {
                Assert.Equal(optionalA, optionalC);
            }
        }

        [Property]
        public void EqualityOperators(int? value)
        {
            // operator==
            Assert.True(
                Optional.Of(value) == Optional.Of(value));

            Assert.True(
                new Optional<int>() == new Optional<int>());

            // operator!=
            Assert.False(
                Optional.Of(value) != Optional.Of(value));

            Assert.False(
                new Optional<int>() != new Optional<int>());
        }

        [Property]
        public void Equality_OptionalWithValueNeverEqualsEmpty(int value)
        {
            Assert.False(
                Optional.Of(value) == Optional<int>.Empty);
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

        // You shouldn't be able to trick Optional by giving it a Nullable.
        // For more Nullable handling, see Of() and Flatten().
        [Fact]
        public void Nullable()
        {
            var optional = Optional.Of((int?)null);
            Assert.False(optional.HasValue);
            Assert.Throws<InvalidOperationException>(() => optional.AssertValue());

            optional = 5;
            Assert.Equal(5, optional);
            Assert.Equal(5, optional.AssertValue());

            var optionalNullable = new Optional<int?>(null);
            Assert.False(optionalNullable.HasValue);
            Assert.Throws<InvalidOperationException>(() => optionalNullable.AssertValue());

            optionalNullable = Optional<int?>.Empty;
            Assert.False(optionalNullable.HasValue);
            Assert.Throws<InvalidOperationException>(() => optionalNullable.AssertValue());

            optionalNullable = 5;
            Assert.Equal(5, optionalNullable);
            Assert.Equal(5, optionalNullable.AssertValue());
        }

        [Fact]
        public void If()
        {
            var success = Optional.If(int.TryParse("123", out int result), result);
            Assert.Equal(123, success);

            var failure = Optional.If(int.TryParse("abc", out result), result);
            Assert.False(failure.HasValue);

            Optional<int> optional;
            bool called;

            called = false;
            optional = Optional.If(true, () =>
            {
                called = true;
                return 123;
            });

            Assert.True(called);
            Assert.Equal(123, optional);

            called = false;
            optional = Optional.If(false, () =>
            {
                called = true;
                return 123;
            });

            Assert.False(called);
            Assert.False(optional.HasValue);

            Assert.Throws<ArgumentNullException>(() => Optional.If<int>(true, null));
        }

        [Fact]
        public void Lift_ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => Optional.Lift<string>(null));

            Assert.Throws<ArgumentNullException>(
                () => Optional.Lift<string, int>(null));
        }

        [Fact]
        public void LiftAction()
        {
            bool called = false;
            var lifted = Optional.Lift(
                (string s) => { called = true; });

            Assert.False(called);

            var optional = Optional<string>.Empty;
            lifted(optional);
            Assert.False(called);

            optional = "hello";
            lifted(optional);
            Assert.True(called);
        }

        [Fact]
        public void LiftFunc()
        {
            var lifted = Optional.Lift(
                (string s) => 1);

            var optional = Optional<string>.Empty;
            lifted(optional);
            Assert.Equal(Optional<int>.Empty, lifted(optional));

            optional = "hello";
            lifted(optional);
            Assert.Equal(1, lifted(optional));
        }

        [Fact]
        public void FlattenOptional()
        {
            var doubleValue = new Optional<Optional<string>>("hello");
            Assert.Equal("hello", doubleValue.Flatten());

            var doubleNone = new Optional<Optional<string>>();
            Assert.False(doubleNone.Flatten().HasValue);

            var valueNone = new Optional<Optional<string>>(Optional<string>.Empty);
            Assert.False(valueNone.Flatten().HasValue);
        }

        [Fact]
        public void FlattenNullable()
        {
            var hasValue = new Optional<int?>(1);
            Assert.Equal(1, hasValue.Flatten());

            var isNull = new Optional<int?>();
            Assert.False(isNull.Flatten().HasValue);
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
            Assert.False(optional.HasValue);
        }
    }
}
