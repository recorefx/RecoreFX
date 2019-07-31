using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Tests
{
    [TestClass]
    public class ResultTests
    {
        [TestMethod]
        public void Constructor()
        {
            var success = new Result<int, bool>(-5);
            Assert.IsTrue(success.IsSuccessful);

            var failure = new Result<int, bool>(false);
            Assert.IsFalse(failure.IsSuccessful);
        }

        [TestMethod]
        public void SwitchFunc()
        {
            Result<string, int> Result;
            int result;

            Result = "hello";
            result = Result.Switch(
                value => 1,
                error => throw new Exception("Should not be called"));
            Assert.AreEqual(1, result);

            Result = 12;
            result = Result.Switch(
                value => throw new Exception("Should not be called"),
                error => error * 2);
            Assert.AreEqual(24, result);

            Assert.ThrowsException<ArgumentNullException>(
                () => Result.Switch(
                    value => throw new Exception("Should not be called"),
                    null));

            Assert.ThrowsException<ArgumentNullException>(
                () => Result.Switch(
                    null,
                    error => throw new Exception("Should not be called")));
        }

        [TestMethod]
        public void SwitchAction()
        {
            Result<string, int> result;
            bool called;

            result = "hello";
            called = false;
            result.Switch(
                value => { called = true; },
                error => throw new Exception("Should not be called"));
            Assert.IsTrue(called);

            result = 12;
            called = false;
            result.Switch(
                value => throw new Exception("Should not be called"),
                error => { called = true; });
            Assert.IsTrue(called);

            Assert.ThrowsException<ArgumentNullException>(
                () => result.Switch(
                    value => throw new Exception("Should not be called"),
                    null));

            Assert.ThrowsException<ArgumentNullException>(
                () => result.Switch(
                    null,
                    error => throw new Exception("Should not be called")));
        }

        [TestMethod]
        public void SwitchThrow()
        {
            Result<int, Exception> result;

            result = 1;
            var actual = result.Switch(
                value => value,
                error => throw error);
            Assert.AreEqual(1, actual);

            result = new DivideByZeroException();
            Assert.ThrowsException<DivideByZeroException>(
                () => result.Switch(
                    value => value,
                    error => throw error));
        }

        [TestMethod]
        public void GetValueGetError()
        {
            var success = Result.Success<int, string>(-5);
            Assert.AreEqual(-5, success.GetValue());
            Assert.AreEqual(Optional<string>.Empty, success.GetError());

            var failure = Result.Failure<int, string>("hello");
            Assert.AreEqual(Optional<int>.Empty, failure.GetValue());
            Assert.AreEqual("hello", failure.GetError());
        }

        [TestMethod]
        public void IfValueIfError()
        {
            Result<int, string> result;
            bool called;

            result = 123;
            called = false;
            result.IfValue(x => { called = true; });
            Assert.IsTrue(called);

            called = false;
            result.IfError(x => { called = true; });
            Assert.IsFalse(called);

            result = string.Empty;
            called = false;
            result.IfValue(x => { called = true; });
            Assert.IsFalse(called);

            called = false;
            result.IfError(x => { called = true; });
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void Then()
        {
            Result<int, Exception> LengthOfString(string str)
            {
                if (str == null)
                {
                    return new ArgumentNullException();
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
            Assert.AreEqual(11, actual);

            resultString = null;
            actual = resultString.Then(LengthOfString);
            Assert.IsFalse(actual.IsSuccessful);
        }

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(1, "abc")]
        public void Equals(int value, string error)
        {
            // object.Equals
            Assert.IsTrue(
                Equals(new Result<int, string>(value), new Result<int, string>(value)));

            Assert.IsTrue(
                Equals(new Result<int, string>(error), new Result<int, string>(error)));

            Assert.IsFalse(
                Equals(new Result<int, string>(value), Optional<string>.Empty));

            Assert.IsFalse(
                Equals(Result.Success<int, int>(1), Result.Failure<int, int>(1)));

            // Result.Equals
            Assert.IsTrue(
                new Result<int, string>(value).Equals(new Result<int, string>(value)));

            Assert.IsTrue(
                new Result<int, string>(error).Equals(new Result<int, string>(error)));

            Assert.IsFalse(
                new Result<int, string>(error).Equals(new Result<int, string>(value)));
        }

        [TestMethod]
        public void EqualsWithNull()
        {
            Assert.IsFalse(
                new Result<int, string>("abc").Equals((Result<int, string>)null));

            Assert.IsFalse(
                new Result<int, string>("abc").Equals(new Result<int, string>(null)));

            Assert.IsTrue(
                new Result<int, string>(null).Equals(new Result<int, string>(null)));
        }

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(1, "abc")]
        public void EqualityOperators(int value, string error)
        {
            // operator==
            Assert.IsTrue(
                new Result<int, string>(value) == new Result<int, string>(value));

            Assert.IsTrue(
                new Result<int, string>(error) == new Result<int, string>(error));

            Assert.IsFalse(
                new Result<int, string>(value) == new Result<int, string>(error));

            // operator!=
            Assert.IsFalse(
                new Result<int, string>(value) != new Result<int, string>(value));

            Assert.IsFalse(
                new Result<int, string>(error) != new Result<int, string>(error));

            Assert.IsTrue(
                new Result<int, string>(value) != new Result<int, string>(error));
        }

        [TestMethod]
        public void EqualsEquivalenceRelation()
        {
            // Reflexive
            Result<int, Exception> a = 1;
            Assert.AreEqual(a, a);

            // Symmetric
            Result<int, Exception> b = 1;
            Assert.AreEqual(a, b);
            Assert.AreEqual(b, a);

            // Transitive
            Result<int, Exception> c = 1;
            Assert.AreEqual(b, c);
            Assert.AreEqual(a, c);
        }

        [TestMethod]
        public new void GetHashCode()
        {
            Result<string, double> Result = "hello";

            Assert.AreEqual(Result.GetHashCode(), Result.GetHashCode());
            Assert.AreEqual(Result.GetHashCode(), new Result<string, Uri>("he" + "llo").GetHashCode());
        }

        [TestMethod]
        public new void ToString()
        {
            var success = Result.Success<int, string>(-5);
            Assert.AreEqual("-5", success.ToString());

            var failure = Result.Failure<int, string>("hello");
            Assert.AreEqual("hello", failure.ToString());
        }

        [TestMethod]
        public void Flatten()
        {
            var doubleValue = new Result<Result<string, Exception>, Exception>("hello");
            Assert.AreEqual("hello", doubleValue.Flatten());

            var valueOfError = new Result<Result<string, Exception>, Exception>(new Exception());
            Assert.IsFalse(valueOfError.Flatten().IsSuccessful);
        }

        [TestMethod]
        public void Values()
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

            CollectionAssert.AreEqual(values, collection.Values().ToArray());
        }

    [   TestMethod]
        public void Errors()
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

            CollectionAssert.AreEqual(errors, collection.Errors().ToArray());
        }
    }
}