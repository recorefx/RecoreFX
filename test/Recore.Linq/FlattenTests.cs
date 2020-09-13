using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Recore.Linq.Tests
{
    public class FlattenTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            IEnumerable<IEnumerable<object>> nullEnumerable = null!;
            Assert.Throws<ArgumentNullException>(
                () => nullEnumerable.Flatten());
        }

        [Fact]
        public void FlattenEmpty()
        {
            var empty = Enumerable.Empty<IEnumerable<object>>();
            Assert.False(empty.Flatten().Any());
        }

        [Fact]
        public void FlattenList()
        {
            var listOfLists = new List<List<int>>
            {
                new List<int> { 1, 2, 3 },
                new List<int> { 4, 5, 6 },
                new List<int> { 7, 8 },
                new List<int> { 9 }
            };

            Assert.Equal(
                new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                listOfLists.Flatten().ToList());
        }

        [Fact]
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

            Assert.Equal(
                new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                listOfEnumerables.Flatten().ToList());
        }

        [Fact]
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

            Assert.Equal(expected.Count, actual.Count);
            foreach (var (expectedSublist, actualSublist) in Renumerable.Zip(expected, actual))
            {
                Assert.Equal(expectedSublist, actualSublist);
            }
        }
    }
}
