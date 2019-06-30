using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Tests
{
[TestClass]
    public class EitherTests
    {
        [TestMethod]
        public void Constructor()
        {
            var left = new Either<int, bool>(-5);
            Assert.IsTrue(left.IsLeft);
            Assert.IsFalse(left.IsRight);

            var right = new Either<int, bool>(false);
            Assert.IsFalse(right.IsLeft);
            Assert.IsTrue(right.IsRight);
        }

        [TestMethod]
        public void SwitchFunc()
        {
            Either<string, int> either;
            int result;

            either = "hello";
            result = either.Switch(
                left => 1,
                right => throw new Exception("Should not be called"));
            Assert.AreEqual(1, result);

            either = 12;
            result = either.Switch(
                left => throw new Exception("Should not be called"),
                right => right * 2);
            Assert.AreEqual(24, result);
        }

        [TestMethod]
        public void SwitchAction()
        {
            Either<string, int> either;
            bool called;

            either = "hello";
            called = false;
            either.Switch(
                left => { called = true; },
                right => throw new Exception("Should not be called"));
            Assert.IsTrue(called);

            either = 12;
            called = false;
            either.Switch(
                left => throw new Exception("Should not be called"),
                right => { called = true; });
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void GetLeftGetRight()
        {
            var left = new Either<int, string>(-5);
            Assert.AreEqual(-5, left.GetLeft());
            Assert.AreEqual(Optional<string>.Empty, left.GetRight());

            var right = new Either<int, string>("hello");
            Assert.AreEqual(Optional<int>.Empty, right.GetLeft());
            Assert.AreEqual("hello", right.GetRight());
        }

        [TestMethod]
        public void IfLeftIfRight()
        {
            Either<int, string> either;
            bool called;

            either = 123;
            called = false;
            either.IfLeft(x => { called = true; });
            Assert.IsTrue(called);

            called = false;
            either.IfRight(x => { called = true; });
            Assert.IsFalse(called);

            either = string.Empty;
            called = false;
            either.IfLeft(x => { called = true; });
            Assert.IsFalse(called);

            called = false;
            either.IfRight(x => { called = true; });
            Assert.IsTrue(called);
        }
    }
}