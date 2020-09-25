using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using Recore.Collections.Generic;

namespace Recore.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Constructor()
        {
            var success = new Result<int, bool>(-5);
            Assert.True(success.IsSuccessful);

            var failure = new Result<int, bool>(false);
            Assert.False(failure.IsSuccessful);
        }

        [Fact]
        public void ConstructorNullable()
        {
            var success = new Result<int?, bool?>(value: null);
            Assert.True(success.IsSuccessful);

            var failure = new Result<int?, bool?>(error: null);
            Assert.False(failure.IsSuccessful);
        }

        [Fact]
        public void SwitchFunc()
        {
            Result<string, int> Result;
            int result;

            Result = "hello";
            result = Result.Switch(
                value => 1,
                error => throw new Exception("Should not be called"));
            Assert.Equal(1, result);

            Result = 12;
            result = Result.Switch(
                value => throw new Exception("Should not be called"),
                error => error * 2);
            Assert.Equal(24, result);

            Assert.Throws<ArgumentNullException>(
                () => Result.Switch(
                    value => throw new Exception("Should not be called"),
                    null!));

            Assert.Throws<ArgumentNullException>(
                () => Result.Switch(
                    null!,
                    error => throw new Exception("Should not be called")));
        }

        [Fact]
        public void SwitchAction()
        {
            Result<string, int> result;
            bool called;

            result = "hello";
            called = false;
            result.Switch(
                value => { called = true; },
                error => throw new Exception("Should not be called"));
            Assert.True(called);

            result = 12;
            called = false;
            result.Switch(
                value => throw new Exception("Should not be called"),
                error => { called = true; });
            Assert.True(called);

            Assert.Throws<ArgumentNullException>(
                () => result.Switch(
                    value => throw new Exception("Should not be called"),
                    null!));

            Assert.Throws<ArgumentNullException>(
                () => result.Switch(
                    null!,
                    error => throw new Exception("Should not be called")));
        }

        [Fact]
        public void SwitchThrow()
        {
            Result<int, Exception> result;

            result = 1;
            var actual = result.Switch(
                value => value,
                error => throw error);
            Assert.Equal(1, actual);

            result = new DivideByZeroException();
            Assert.Throws<DivideByZeroException>(
                () => result.Switch(
                    value => value,
                    error => throw error));
        }

        [Fact]
        public void OnValueOnError()
        {
            Result<int, string> result;

            result = 123;
            Assert.Equal(1, result.OnValue(x => 1));
            Assert.Equal(123, result.OnError(x => 1).Collapse());

            result = "hello";
            Assert.Equal("hello", result.OnValue(x => 1));
            Assert.Equal(1, result.OnError(x => 1).Collapse());
        }

        [Fact]
        public void IfValueIfError()
        {
            Result<int, string> result;
            bool called;

            result = 123;
            called = false;
            result.IfValue(x => { called = true; });
            Assert.True(called);

            called = false;
            result.IfError(x => { called = true; });
            Assert.False(called);

            result = string.Empty;
            called = false;
            result.IfValue(x => { called = true; });
            Assert.False(called);

            called = false;
            result.IfError(x => { called = true; });
            Assert.True(called);
        }

        [Fact]
        public void Then()
        {
            Result<int, Exception> LengthOfString(string str)
            {
                if (str == string.Empty)
                {
                    return new DivideByZeroException();
                }
                else
                {
                    return str.Length;
                }
            }

            Result<string, Exception> resultString;
            Result<int, Exception> actual;

            resultString = "hello world";
            actual = resultString.Then(LengthOfString);
            Assert.Equal(11, actual);

            resultString = string.Empty;
            actual = resultString.Then(LengthOfString);
            Assert.False(actual.IsSuccessful);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "abc")]
        public void Equals_(int value, string error)
        {
            // object.Equals
            Assert.True(
                Equals(new Result<int, string>(value), new Result<int, string>(value)));

            Assert.True(
                Equals(new Result<int, string>(error), new Result<int, string>(error)));

            Assert.False(
                Equals(new Result<int, string>(value), Optional<string>.Empty));

            Assert.False(
                Equals(Result.Success<int, int>(1), Result.Failure<int, int>(1)));

            // Result.Equals
            Assert.True(
                new Result<int, string>(value).Equals(new Result<int, string>(value)));

            Assert.True(
                new Result<int, string>(error).Equals(new Result<int, string>(error)));

            Assert.False(
                new Result<int, string>(error).Equals(new Result<int, string>(value)));
        }

        [Fact]
        public void EqualsWithNull()
        {
            Assert.False(
                new Result<int, string>("abc").Equals(null!));
        }

        [Theory]
        [InlineData("", "", true)]
        [InlineData("a", "b", true)]
        [InlineData("hello", "world", true)]
        [InlineData("", "a", false)]
        [InlineData("", "world", false)]
        [InlineData("hello", "", false)]
        public void Equals_EqualityComparer(string a, string b, bool expected)
        {
            var compareOnLength = new MappedEqualityComparer<string, int>(x => x.Length);
            var intComparer = EqualityComparer<int>.Default;

            { // Value
                Result<string, int> resultA = a;
                Result<string, int> resultB = b;

                Assert.True(resultA.Equals(resultA, compareOnLength, intComparer));
                Assert.True(resultB.Equals(resultB, compareOnLength, intComparer));

                Assert.Equal(expected, resultA.Equals(resultB, compareOnLength, intComparer));
                Assert.Equal(expected, resultB.Equals(resultA, compareOnLength, intComparer));

                Assert.False(resultA.Equals(new Result<string, int>(1), compareOnLength, intComparer));
                Assert.False(resultB.Equals(new Result<string, int>(1), compareOnLength, intComparer));
            }
            { // Error
                Result<int, string> resultA = a;
                Result<int, string> resultB = b;

                Assert.True(resultA.Equals(resultA, intComparer, compareOnLength));
                Assert.True(resultB.Equals(resultB, intComparer, compareOnLength));

                Assert.Equal(expected, resultA.Equals(resultB, intComparer, compareOnLength));
                Assert.Equal(expected, resultB.Equals(resultA, intComparer, compareOnLength));

                Assert.False(resultA.Equals(new Result<int, string>(1), intComparer, compareOnLength));
                Assert.False(resultB.Equals(new Result<int, string>(1), intComparer, compareOnLength));
            }
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "abc")]
        public void EqualityOperators(int value, string error)
        {
            // operator==
            Assert.True(
                new Result<int, string>(value) == new Result<int, string>(value));

            Assert.True(
                new Result<int, string>(error) == new Result<int, string>(error));

            Assert.False(
                new Result<int, string>(value) == new Result<int, string>(error));

            // operator!=
            Assert.False(
                new Result<int, string>(value) != new Result<int, string>(value));

            Assert.False(
                new Result<int, string>(error) != new Result<int, string>(error));

            Assert.True(
                new Result<int, string>(value) != new Result<int, string>(error));
        }

        [Fact]
        public void EqualsEquivalenceRelation()
        {
            // Reflexive
            Result<int, Exception> a = 1;
            Assert.Equal(a, a);

            // Symmetric
            Result<int, Exception> b = 1;
            Assert.Equal(a, b);
            Assert.Equal(b, a);

            // Transitive
            Result<int, Exception> c = 1;
            Assert.Equal(b, c);
            Assert.Equal(a, c);
        }

        [Fact]
        public void GetHashCode_()
        {
            Result<string, double> Result = "hello";

            Assert.Equal(Result.GetHashCode(), Result.GetHashCode());
            Assert.Equal(Result.GetHashCode(), new Result<string, Uri>("he" + "llo").GetHashCode());
        }

        [Fact]
        public void ToString_()
        {
            var success = Result.Success<int, string>(-5);
            Assert.Equal("-5", success.ToString());

            var failure = Result.Failure<int, string>("hello");
            Assert.Equal("hello", failure.ToString());
        }

        [Fact]
        public void GetValueGetError()
        {
            var success = Result.Success<int, string>(-5);
            Assert.Equal(-5, success.GetValue());
            Assert.False(success.GetError().HasValue);

            var failure = Result.Failure<int, string>("hello");
            Assert.False(failure.GetValue().HasValue);
            Assert.Equal("hello", failure.GetError());
        }

        [Fact]
        public void GetLeftGetRightNullable()
        {
            var left = new Result<int?, string>(-5);
            Assert.Equal(-5, left.GetValue());
            Assert.False(left.GetError().HasValue);

            left = new Result<int?, string>(value: null);
            Assert.Null(left.GetValue());
            Assert.False(left.GetError().HasValue);

            var right = new Result<int?, string>("hello");
            Assert.Null(right.GetValue());
            Assert.Equal("hello", right.GetError());
        }

        [Fact]
        public void TryCatch()
        {
            var success = Result
                .Try(() => 1)
                .Catch<Exception>();

            Assert.Equal(1, success);

            // Avoid "divide by constant zero" compiler error
            int zero = 0;

            var failure = Result
                .Try<double>(() => 1 / zero) // throws DivideByZeroException
                .Catch<DivideByZeroException>();

            Assert.False(failure.IsSuccessful);

            Assert.Throws<ArgumentException>(
                () => Result
                    .Try<double>(() => throw new ArgumentException())
                    .Catch<DivideByZeroException>());
        }

        [Fact]
        public void TryCatchMap()
        {
            var success = Result
                .Try(() => 1)
                .Catch((Exception _) => "failed");

            Assert.Equal(1, success);

            var failure = Result
                .Try<int>(() => throw new Exception("exception message"))
                .Catch((Exception e) => e.Message);

            Assert.Equal("exception message", failure);
        }

        [Fact]
        public async Task TryCatchAsync()
        {
            var success = await Result
                .TryAsync(async () => await Task.FromResult(1))
                .CatchAsync<Exception>();

            Assert.Equal(1, success);

            // Avoid "divide by constant zero" compiler error
            int zero = 0;

            var failure = await Result
                .TryAsync<double>(async () => await Task.FromResult(1) / zero) // throws DivideByZeroException
                .CatchAsync<DivideByZeroException>();

            Assert.False(failure.IsSuccessful);

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await Result
                    .TryAsync<double>(() => throw new ArgumentException())
                    .CatchAsync<DivideByZeroException>());
        }

        [Fact]
        public async Task TryCatchAsyncMap()
        {
            var success = await Result
                .TryAsync(async () => await Task.FromResult(1))
                .CatchAsync((Exception _) => Task.FromResult("failed"));

            Assert.Equal(1, success);

            var failure = await Result
                .TryAsync<int>(() => throw new Exception("exception message"))
                .CatchAsync((Exception e) => Task.FromResult(e.Message));

            Assert.Equal("exception message", failure);
        }

        [Fact]
        public void Flatten()
        {
            var doubleValue = new Result<Result<string, Exception>, Exception>("hello");
            Assert.Equal("hello", doubleValue.Flatten());

            var valueOfError = new Result<Result<string, Exception>, Exception>(new Exception());
            Assert.False(valueOfError.Flatten().IsSuccessful);
        }

        [Fact]
        public void Successes()
        {
            var collection = new Result<string, int>[]
            {
                string.Empty,
                "abc",
                1,
                "5",
                23,
                "hello world"
            };

            var values = new[]
            {
                string.Empty,
                "abc",
                "5",
                "hello world"
            };

            Assert.Equal(values, collection.Successes().ToArray());
        }

        [Fact]
        public void Failures()
        {
            var collection = new Result<string, int>[]
            {
                string.Empty,
                "abc",
                1,
                "5",
                23,
                "hello world"
            };

            var errors = new[]
            {
                1,
                23
            };

            Assert.Equal(errors, collection.Failures().ToArray());
        }
    }
}