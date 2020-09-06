using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Recore.Collections.Generic.Tests
{
    public class ListExtensionsTests
    {
        [Fact]
        public void AppendRange_NullList()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                List<string> list = null;
                list.AppendRange(new[] { "hello" });
            });
        }

        [Fact]
        public void AppendRange_NullArgument()
        {
            Assert.Throws<ArgumentNullException>(
                () => new List<string>().AppendRange(null));
        }

        [Fact]
        public void AppendRange_EmptyCollection()
        {
            var list = new List<int>()
                .AppendRange(Enumerable.Range(1, 5))
                .AppendRange(new[] { 10 });

            var expected = new List<int> { 1, 2, 3, 4, 5, 10};

            Assert.Equal(expected, list);
        }

        [Fact]
        public void AppendRange_NonEmptyCollection()
        {
            var list = new List<int> { 1, 2, 3 }
                .AppendRange(new[] { 1, 2, 3 });

            var expected = new List<int> { 1, 2, 3, 1, 2, 3 };

            Assert.Equal(expected, list);
        }

        [Fact]
        public void AppendRange_ModifiesOriginalCollection()
        {
            var originalCollection = new List<string> { "abc" };
            var appendedCollection = originalCollection.AppendRange(new[] { "hello" });

            Assert.Equal(originalCollection, appendedCollection);
        }
    }
}