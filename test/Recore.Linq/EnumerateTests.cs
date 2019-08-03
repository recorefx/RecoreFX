using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Recore.Linq.Tests
{
    public class EnumerateTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            IEnumerable<object> nullEnumerable = null;
            Assert.Throws<ArgumentNullException>(
                () => TestHelpers.ForceExecution(nullEnumerable.Enumerate));
        }

        [Fact]
        public void EmptyEnumerable()
        {
            var result = Enumerable.Empty<object>().Enumerate();
            Assert.False(result.Any());
        }

        [Fact]
        public void AddsIndexToElements()
        {
            var enumerable = new[]
            {
                "hello",
                "world",
                "asdf",
                "...xxx !"
            };

            var expected = new[]
            {
                (0, "hello"),
                (1, "world"),
                (2, "asdf"),
                (3, "...xxx !")
            };

            var result = enumerable.Enumerate();
            Assert.Equal(expected, result.ToList());
        }
    }
}
