using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Recore.Linq.Tests
{
    public class NonNullTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            IEnumerable<object> nullEnumerable = null!;
            Assert.Throws<ArgumentNullException>(() => nullEnumerable.NonNull());
        }

        [Fact]
        public void EmptyEnumerable()
        {
            var empty = Enumerable.Empty<string>();
            Assert.True(empty.NonNull().SequenceEqual(empty));
        }
    }
}