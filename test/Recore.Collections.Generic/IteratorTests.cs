using System;
using System.Linq;

using Xunit;

namespace Recore.Collections.Generic.Tests
{
    public class IteratorTests
    {
        [Fact]
        public void EmptyEnumerable()
        {
            var empty = Enumerable.Empty<string>();
            var iterator = Iterator.FromEnumerable(empty);

            Assert.False(iterator.HasNext);
        }

        [Fact]
        public void IterateOverCollection()
        {
            var strings = new[]
            {
                "hello",
                "world",
                "goodbye"
            };

            var iterator = Iterator.FromEnumerable(strings);

            var count = 0;
            while (iterator.HasNext)
            {
                Assert.Equal(strings[count++], iterator.Next());
            }

            // Make sure we iterated over the whole collection.
            Assert.Equal(strings.Length, count);
            Assert.Throws<InvalidOperationException>(() => iterator.Next());
        }
    }
}