using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Linq.Tests
{
    [TestClass]
    public class ZipTupleTests
    {
        [TestMethod]
        public void ZipSameLength()
        {
            var collection1 = new[] { "abc", string.Empty, "hello world" };
            var collection2 = new[] { 1, 2, 3 };

            var zipped = new[]
            {
                ("abc", 1),
                (string.Empty, 2),
                ("hello world", 3)
            };

            CollectionAssert.AreEqual(zipped, collection1.Zip(collection2).ToArray());
        }

        [TestMethod]
        public void ZipDifferentLength()
        {
            var empty = Enumerable.Empty<string>();
            var collection = new[] { 1, 2, 3 };

            CollectionAssert.AreEqual(
                Enumerable.Empty<(string, int)>().ToArray(),
                empty.Zip(collection).ToArray());

            CollectionAssert.AreEqual(
                Enumerable.Empty<(int, string)>().ToArray(),
                collection.Zip(empty).ToArray());
        }
    }
}