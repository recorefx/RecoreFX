using System;
using System.Collections.Generic;
using System.Linq;
using Recore.Collections.Generic;

using Xunit;

namespace Recore.Linq.Tests
{
    public class ToLinkedListTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => Renumerable.ToLinkedList<object>(null));
        }

        [Fact]
        public void ToLinkedList()
        {
            var actual = Enumerable.Range(1, 5).ToLinkedList();

            Assert.Equal(new LinkedList<int> { 1, 2, 3, 4, 5 }, actual);
        }
    }
}
