using System;
using System.Linq;

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
            Assert.AreEqual(Optional.Empty<string>(), left.GetRight());

            var right = new Either<int, string>("hello");
            Assert.AreEqual(Optional.Empty<int>(), right.GetLeft());
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

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(1, "abc")]
        public void Equals(int left, string right)
        {
            // object.Equals
            Assert.IsTrue(
                Equals(new Either<int, string>(left), new Either<int, string>(left)));

            Assert.IsTrue(
                Equals(new Either<int, string>(right), new Either<int, string>(right)));

            Assert.IsFalse(
                Equals(new Either<int, string>(left), Optional.Empty<string>()));

            // Either.Equals
            Assert.IsTrue(
                new Either<int, string>(left).Equals(new Either<int, string>(left)));

            Assert.IsTrue(
                new Either<int, string>(right).Equals(new Either<int, string>(right)));

            Assert.IsFalse(
                new Either<int, string>(right).Equals(new Either<int, string>(left)));
        }

        [TestMethod]
        public void EqualsWithNull()
        {
            Assert.IsFalse(
                new Either<int, string>("abc").Equals((Either<int, string>)null));

            Assert.IsFalse(
                new Either<int, string>("abc").Equals(new Either<int, string>(null)));

            Assert.IsTrue(
                new Either<int, string>(null).Equals(new Either<int, string>(null)));
        }

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(1, "abc")]
        public void SwapEquals(int left, string right)
        {
            Assert.IsTrue(
                Equals(new Either<int, string>(left), new Either<string, int>(left).Swap()));

            Assert.IsTrue(
                new Either<int, string>(left).Equals(new Either<string, int>(left).Swap()));

            Assert.IsTrue(
                new Either<string, int>(left).Equals(new Either<int, string>(left).Swap()));
        }

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(1, "abc")]
        public void EqualityOperators(int left, string right)
        {
            // operator==
            Assert.IsTrue(
                new Either<int, string>(left) == new Either<int, string>(left));

            Assert.IsTrue(
                new Either<int, string>(right) == new Either<int, string>(right));

            Assert.IsFalse(
                new Either<int, string>(left) == new Either<int, string>(right));

            // operator!=
            Assert.IsFalse(
                new Either<int, string>(left) != new Either<int, string>(left));

            Assert.IsFalse(
                new Either<int, string>(right) != new Either<int, string>(right));

            Assert.IsTrue(
                new Either<int, string>(left) != new Either<int, string>(right));
        }

        [TestMethod]
        public void EqualsEquivalenceRelation()
        {
            // Reflexive
            var a = new Either<int, int>(left: 1);
            Assert.AreEqual(a, a);

            // Symmetric
            var b = new Either<string, int>(1).OnLeft(x => x.Length);
            Assert.AreNotEqual(a, b); // a is left and b is right

            b = b.Swap();
            Assert.AreEqual(a, b);
            Assert.AreEqual(b, a);

            // Transitive
            var c = new Either<int, Uri>(1).OnRight(x => x.Port);
            Assert.AreEqual(b, c);
            Assert.AreEqual(a, c);
        }

        [TestMethod]
        public new void GetHashCode()
        {
            Either<string, double> either = "hello";

            Assert.AreEqual(either.GetHashCode(), either.GetHashCode());
            Assert.AreEqual(either.GetHashCode(), new Either<string, Uri>("he" + "llo").GetHashCode());
        }

        [TestMethod]
        public new void ToString()
        {
            var left = new Either<int, string>(-5);
            Assert.AreEqual("-5", left.ToString());

            var right = new Either<int, string>("hello");
            Assert.AreEqual("hello", right.ToString());
        }

        [TestMethod]
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

            CollectionAssert.AreEqual(lefts, collection.Lefts().ToArray());
        }

    [   TestMethod]
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

            CollectionAssert.AreEqual(rights, collection.Rights().ToArray());
        }
    }
}