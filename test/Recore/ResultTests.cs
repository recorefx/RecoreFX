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
            Result<string, int> Result;
            bool called;

            Result = "hello";
            called = false;
            Result.Switch(
                value => { called = true; },
                error => throw new Exception("Should not be called"));
            Assert.IsTrue(called);

            Result = 12;
            called = false;
            Result.Switch(
                value => throw new Exception("Should not be called"),
                error => { called = true; });
            Assert.IsTrue(called);

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
        [DataRow(0, "")]
        [DataRow(1, "abc")]
        public void Equals(int left, string right)
        {
            // object.Equals
            Assert.IsTrue(
                Equals(new Result<int, string>(left), new Result<int, string>(left)));

            Assert.IsTrue(
                Equals(new Result<int, string>(right), new Result<int, string>(right)));

            Assert.IsFalse(
                Equals(new Result<int, string>(left), Optional<string>.Empty));

            Assert.IsFalse(
                Equals(Result.Success<int, int>(1), Result.Failure<int, int>(1)));

            // Result.Equals
            Assert.IsTrue(
                new Result<int, string>(left).Equals(new Result<int, string>(left)));

            Assert.IsTrue(
                new Result<int, string>(right).Equals(new Result<int, string>(right)));

            Assert.IsFalse(
                new Result<int, string>(right).Equals(new Result<int, string>(left)));
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
        public void EqualityOperators(int left, string right)
        {
            // operator==
            Assert.IsTrue(
                new Result<int, string>(left) == new Result<int, string>(left));

            Assert.IsTrue(
                new Result<int, string>(right) == new Result<int, string>(right));

            Assert.IsFalse(
                new Result<int, string>(left) == new Result<int, string>(right));

            // operator!=
            Assert.IsFalse(
                new Result<int, string>(left) != new Result<int, string>(left));

            Assert.IsFalse(
                new Result<int, string>(right) != new Result<int, string>(right));

            Assert.IsTrue(
                new Result<int, string>(left) != new Result<int, string>(right));
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