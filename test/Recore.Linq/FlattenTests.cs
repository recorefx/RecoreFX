using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Linq.Tests
{
    [TestClass]
    public class FlattenTests
    {
        [TestMethod]
        public void ThrowsOnNull()
        {
            IEnumerable<IEnumerable<object>> nullEnumerable = null;
            Assert.ThrowsException<ArgumentNullException>(
                () => nullEnumerable.Flatten());
        }

        [TestMethod]
        public void FlattenEmpty()
        {
            var empty = Enumerable.Empty<IEnumerable<object>>();
            Assert.IsFalse(empty.Flatten().Any());
        }

        [TestMethod]
        public void FlattenList()
        {
            var listOfLists = new List<List<int>>
            {
                new List<int> { 1, 2, 3 },
                new List<int> { 4, 5, 6 },
                new List<int> { 7, 8 },
                new List<int> { 9 }
            };

            CollectionAssert.AreEqual(
                new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                listOfLists.Flatten().ToList());
        }

        [TestMethod]
        public void FlattenHeterogeneous()
        {
            var linkedList = new LinkedList<int>();
            linkedList.AddLast(7);
            linkedList.AddLast(8);

            IEnumerable<int> Generator()
            {
                yield return 9;
            }

            var listOfEnumerables = new List<IEnumerable<int>>
            {
                new List<int> { 1, 2, 3 },
                new int[] { 4, 5, 6 },
                linkedList,
                Generator()
            };

            CollectionAssert.AreEqual(
                new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                listOfEnumerables.Flatten().ToList());
        }

        [TestMethod]
        public void FlattenThreeLevels()
        {
            var listOfListsOfLists = new List<List<List<int>>>
            {
                new List<List<int>>
                {
                    new List<int> { 1, 2, 3 },
                    new List<int> { 4, 5, 6 }
                },
                new List<List<int>>
                {
                    new List<int> { 7, 8 }
                },
                new List<List<int>>
                {
                    new List<int> { 9 }
                }
            };

            var expected = new List<List<int>>
            {
                new List<int> { 1, 2, 3 },
                new List<int> { 4, 5, 6 },
                new List<int> { 7, 8 },
                new List<int> { 9 }
            };

            var actual = listOfListsOfLists.Flatten().ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            foreach (var (expectedSublist, actualSublist) in Renumerable.Zip(expected, actual))
            {
                CollectionAssert.AreEqual(expectedSublist, actualSublist);
            }
        }
    }
}
