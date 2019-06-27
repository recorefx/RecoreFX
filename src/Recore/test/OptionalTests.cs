using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Tests
{
    [TestClass]
    public class OptionalTests
    {
        [TestMethod]
        public void Constructor()
        {
            var referenceOptional = new Optional<string>("Hello world");
            Assert.IsTrue(referenceOptional.HasValue);

            var valueOptional = new Optional<int>(123);
            Assert.IsTrue(valueOptional.HasValue);

            var nullOptional = new Optional<object>(null);
            Assert.IsFalse(nullOptional.HasValue);
        }

        [TestMethod]
        public void DefaultConstructor()
        {
            var referenceOptional = new Optional<object>();
            Assert.IsFalse(referenceOptional.HasValue);

            var valueOptional = new Optional<int>();
            Assert.IsFalse(valueOptional.HasValue);
        }

        [TestMethod]
        public void Empty()
        {
            var referenceOptional = Optional<object>.Empty;
            Assert.IsFalse(referenceOptional.HasValue);

            var valueOptional = Optional<int>.Empty;
            Assert.IsFalse(valueOptional.HasValue);
        }

        [TestMethod]
        public void SwitchFunc()
        {
            Optional<int> optional;
            int result;

            optional = 123;
            result = optional.Switch(
                x => 1,
                () => throw new Exception("Should not be called"));
            Assert.AreEqual(1, result);

            optional = Optional.Empty;
            result = optional.Switch(
                x => throw new Exception("Should not be called"),
                () => 10);
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void SwitchAction()
        {
            Optional<int> optional;
            bool called;

            optional = 123;
            called = false;
            optional.Switch(
                x => { called = true; },
                () => throw new Exception("Should not be called"));
            Assert.IsTrue(called);

            optional = Optional.Empty;
            called = false;
            optional.Switch(
                x => throw new Exception("Should not be called"),
                () => { called = true; });
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ValueOr()
        {
            Optional<string> optional;

            optional = "hello world";
            Assert.AreEqual("hello world", optional.ValueOr("xxx"));

            optional = null;
            Assert.AreEqual("xxx", optional.ValueOr("xxx"));
        }

        [TestMethod]
        public void OnValue()
        {
            Optional<int> optional;
            int Square(int n) => n * n;

            optional = 10;
            Assert.AreEqual(100, optional.OnValue(Square));

            optional = Optional.Empty;
            Assert.IsFalse(optional.OnValue(Square).HasValue);

            optional = 100;
            Assert.AreEqual("100", optional.OnValue(x => x.ToString()));
        }

        [TestMethod]
        public void IfValueIfEmpty()
        {
            Optional<int> optional;
            bool called;

            optional = 123;
            called = false;
            optional.IfValue(x => { called = true; });
            Assert.IsTrue(called);

            called = false;
            optional.IfEmpty(() => { called = true; });
            Assert.IsFalse(called);

            optional = Optional.Empty;
            called = false;
            optional.IfValue(x => { called = true; });
            Assert.IsFalse(called);

            called = false;
            optional.IfEmpty(() => { called = true; });
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void FunctorLaws()
        {
            // Identity
            Optional<string> referenceOptional = "hello world";
            Optional<int> valueOptional = 123;
            T Id<T>(T t) => t;

            Assert.AreEqual("hello world", referenceOptional.OnValue(Id));
            Assert.AreEqual(123, valueOptional.OnValue(Id));
            Assert.AreEqual(Optional.Empty, Optional<object>.Empty.OnValue(Id));

            // Composition
            int Square(int n) => n * n;
            int PlusTwo(int n) => n + 2;

            Assert.AreEqual(
                new Optional<int>(10).OnValue(Square).OnValue(PlusTwo),
                new Optional<int>(10).OnValue(x => PlusTwo(Square(x))));

            Assert.AreEqual(
                Optional<int>.Empty.OnValue(Square).OnValue(PlusTwo),
                Optional<int>.Empty.OnValue(x => PlusTwo(Square(x))));
        }

        [TestMethod]
        public void Then()
        {
            Optional<int> FindFirstSpace(string str)
            {
                var result = str.IndexOf(' ');
                if (result == -1)
                {
                    return Optional.Empty;
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
            Assert.AreEqual(5, actual);

            optionalString = "hello";
            actual = optionalString.Then(FindFirstSpace);
            Assert.IsFalse(actual.HasValue);

            optionalString = Optional.Empty;
            actual = optionalString.Then(FindFirstSpace);
            Assert.IsFalse(actual.HasValue);
        }

        [TestMethod]
        public void MonadLaws()
        {
            Optional<string> StringToOption(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return Optional.Empty;
                }
                else
                {
                    return str;
                }
            }

            // Left identity
            foreach (var value in new[] { "hello", string.Empty, null })
            {
                Assert.AreEqual(
                    StringToOption(value),
                    new Optional<string>(value).Then(StringToOption));
            }

            // Right identity
            foreach (var optional in new[] { "hello", null, Optional<string>.Empty })
            {
                Assert.AreEqual(
                    optional,
                    optional.Then(x => new Optional<string>(x)));
            }

            // Associativity
            Optional<int> NullsafeLength(string str)
            {
                if (str == null)
                {
                    return Optional.Empty;
                }
                else
                {
                    return str.Length;
                }
            }

            foreach (var optional in new[] { "hello", null, Optional<string>.Empty })
            {
                Assert.AreEqual(
                    optional.Then(StringToOption).Then(NullsafeLength),
                    optional.Then(x => StringToOption(x).Then(NullsafeLength)));
            }
        }

        [TestMethod]
        public void Flatten()
        {
            var doubleValue = new Optional<Optional<string>>("hello");
            Assert.AreEqual("hello", doubleValue.Flatten());

            var doubleNone = new Optional<Optional<string>>();
            Assert.IsFalse(doubleNone.Flatten().HasValue);

            var valueNone = new Optional<Optional<string>>(Optional.Empty);
            Assert.IsFalse(valueNone.Flatten().HasValue);
        }
    }
}
