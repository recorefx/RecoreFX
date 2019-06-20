using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Tests
{
    [TestClass]
    public class OptionTests
    {
        [TestMethod]
        public void CreateOptionWithValue()
        {
            var referenceOption = new Option<string>("Hello world");
            Assert.IsTrue(referenceOption.HasValue);

            var valueOption = new Option<int>(123);
            Assert.IsTrue(valueOption.HasValue);

            var nullOption = new Option<object>(null);
            Assert.IsFalse(nullOption.HasValue);
        }

        [TestMethod]
        public void DefaultConstructor()
        {
            var referenceOption = new Option<object>();
            Assert.IsFalse(referenceOption.HasValue);

            var valueOption = new Option<int>();
            Assert.IsFalse(valueOption.HasValue);
        }

        [TestMethod]
        public void None()
        {
            var referenceOption = Option<object>.None;
            Assert.IsFalse(referenceOption.HasValue);

            var valueOption = Option<int>.None;
            Assert.IsFalse(valueOption.HasValue);
        }

        [TestMethod]
        public void SwitchFunc()
        {
            Option<int> option;
            int result;

            option = 123;
            result = option.Switch(
                x => 1,
                () => throw new Exception("Should not be called"));
            Assert.AreEqual(1, result);

            option = Option<int>.None;
            result = option.Switch(
                x => throw new Exception("Should not be called"),
                () => 10);
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void SwitchAction()
        {
            Option<int> option;
            int result;

            option = 123;
            result = -1;
            option.Switch(
                x => { result = 10; },
                () => throw new Exception("Should not be called"));
            Assert.AreEqual(10, result);

            option = Option<int>.None;
            result = -1;
            option.Switch(
                x => throw new Exception("Should not be called"),
                () => { result = 20; });
            Assert.AreEqual(20, result);
        }

        [TestMethod]
        public void ValueOr()
        {
            Option<string> option;

            option = "hello world";
            Assert.AreEqual("hello world", option.ValueOr("xxx"));

            option = null;
            Assert.AreEqual("xxx", option.ValueOr("xxx"));
        }

        [TestMethod]
        public void IfValue()
        {
            Option<int> option;
            bool called;

            option = 123;
            called = false;
            option.IfValue(x => { called = true; });
            Assert.IsTrue(called);

            option = Option<int>.None;
            called = false;
            option.IfValue(x => { called = true; });
            Assert.IsFalse(called);
        }

        [TestMethod]
        public void IfNone()
        {
            Option<int> option;
            bool called;

            option = 123;
            called = false;
            option.IfNone(() => { called = true; });
            Assert.IsFalse(called);

            option = Option<int>.None;
            called = false;
            option.IfNone(() => { called = true; });
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void Try()
        {
            Option<int> option;
            int Square(int n) => n * n;

            option = 10;
            Assert.AreEqual(100, option.Try(Square));

            option = Option<int>.None;
            Assert.IsFalse(option.Try(Square).HasValue);

            option = 100;
            Assert.AreEqual("100", option.Try(x => x.ToString()));
        }

        [TestMethod]
        public void FunctorLaws()
        {
            // Identity
            Option<string> referenceOption = "hello world";
            Option<int> valueOption = 123;
            T Id<T>(T t) => t;

            Assert.AreEqual("hello world", referenceOption.Try(Id));
            Assert.AreEqual(123, valueOption.Try(Id));
            Assert.AreEqual(Option<object>.None, Option<object>.None.Try(Id));

            // Composition
            int Square(int n) => n * n;
            int PlusTwo(int n) => n + 2;

            Assert.AreEqual(
                new Option<int>(10).Try(Square).Try(PlusTwo),
                new Option<int>(10).Try(x => PlusTwo(Square(x))));

            Assert.AreEqual(
                Option<int>.None.Try(Square).Try(PlusTwo),
                Option<int>.None.Try(x => PlusTwo(Square(x))));
        }

        [TestMethod]
        public void Then()
        {
            Option<int> FindFirstSpace(string str)
            {
                var result = str.IndexOf(' ');
                if (result == -1)
                {
                    return Option<int>.None;
                }
                else
                {
                    return result;
                }
            }

            Option<string> maybeStr;
            Option<int> actual;

            maybeStr = "hello world";
            actual = maybeStr.Then(FindFirstSpace);
            Assert.AreEqual(5, actual);

            maybeStr = "hello";
            actual = maybeStr.Then(FindFirstSpace);
            Assert.IsFalse(actual.HasValue);

            maybeStr = Option<string>.None;
            actual = maybeStr.Then(FindFirstSpace);
            Assert.IsFalse(actual.HasValue);
        }

        [TestMethod]
        public void MonadLaws()
        {
            Option<string> StringToOption(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return Option<string>.None;
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
                    new Option<string>(value).Then(StringToOption));
            }

            // Right identity
            foreach (var option in new[] { "hello", null, Option<string>.None })
            {
                Assert.AreEqual(
                    option,
                    option.Then(x => new Option<string>(x)));
            }

            // Associativity
            Option<int> NullsafeLength(string str)
            {
                if (str == null)
                {
                    return Option<int>.None;
                }
                else
                {
                    return str.Length;
                }
            }

            foreach (var option in new[] { "hello", null, Option<string>.None })
            {
                Assert.AreEqual(
                    option.Then(StringToOption).Then(NullsafeLength),
                    option.Then(x => StringToOption(x).Then(NullsafeLength)));
            }
        }

        [TestMethod]
        public void Flatten()
        {
            var doubleValue = new Option<Option<string>>("hello");
            Assert.AreEqual("hello", doubleValue.Flatten());

            var doubleNone = Option<Option<string>>.None;
            Assert.IsFalse(doubleNone.Flatten().HasValue);

            var valueNone = new Option<Option<string>>(Option<string>.None);
            Assert.IsFalse(valueNone.Flatten().HasValue);
        }
    }
}
