using System;
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

            // Composition
            int Square(int n) => n * n;
            int PlusTwo(int n) => n + 2;
            Assert.AreEqual(
                new Option<int>(10).Try(Square).Try(PlusTwo),
                new Option<int>(10).Try(x => PlusTwo(Square(x))));
        }

        // TODO
        //[TestMethod]
        //public void Then()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void MonadLaws()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void Join()
        //{
        //    Assert.Fail();
        //}
    }
}
