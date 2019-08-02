using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Linq.Tests
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void ThrowsOnNull()
        {
            IEnumerable<object> nullEnumerable = null;
            Assert.ThrowsException<ArgumentNullException>(
                () => TestHelpers.ForceExecution(() => nullEnumerable.Product(new[] { 0 })));

            Assert.ThrowsException<ArgumentNullException>(
                () => TestHelpers.ForceExecution(() => new[] { 0 }.Product(nullEnumerable)));
        }

        [TestMethod]
        public void ProductSingleton()
        {
            var collection = new[] { 0 };

            var result = new[]
            {
                (0, 0)
            };

            CollectionAssert.AreEqual(result, collection.Product(collection).ToArray());
        }

        [TestMethod]
        public void ProductSameLength()
        {
            var collection1 = new[] { "abc", string.Empty, "hello world" };
            var collection2 = new[] { 1, 2, 3 };

            var result = new[]
            {
                ("abc", 1),
                ("abc", 2),
                ("abc", 3),
                (string.Empty, 1),
                (string.Empty, 2),
                (string.Empty, 3),
                ("hello world", 1),
                ("hello world", 2),
                ("hello world", 3)
            };

            CollectionAssert.AreEqual(result, collection1.Product(collection2).ToArray());
        }

        [TestMethod]
        public void ProductDifferentLength()
        {
            var collection1 = new[] { "abc" };
            var collection2 = new[] { 1, 2, 3 };

            var result12 = new[]
            {
                ("abc", 1),
                ("abc", 2),
                ("abc", 3)
            };

            CollectionAssert.AreEqual(result12, collection1.Product(collection2).ToArray());

            var result21 = new[]
            {
                (1, "abc"),
                (2, "abc"),
                (3, "abc")
            };

            CollectionAssert.AreEqual(result21, collection2.Product(collection1).ToArray());
        }

        [TestMethod]
        public void ProductEmpty()
        {
            var empty = Enumerable.Empty<string>();
            var collection = new[] { 1, 2, 3 };

            CollectionAssert.AreEqual(
                Enumerable.Empty<(string, int)>().ToArray(),
                empty.Product(collection).ToArray());

            CollectionAssert.AreEqual(
                Enumerable.Empty<(int, string)>().ToArray(),
                collection.Product(empty).ToArray());


            CollectionAssert.AreEqual(
                Enumerable.Empty<(string, string)>().ToArray(),
                empty.Product(empty).ToArray());
        }

        // FsCheck properties:
        // - product with empty -> empty
        // - product with length 1 -> same length
        // - product length m with length n -> length m * n
        // - each element of original sequence in new sequence
        // - a.Product(b).SortBy(x => x.first) == b.Product(a).Select(Swap).SortBy(x => x.first)
    }
}