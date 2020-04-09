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
    }
}