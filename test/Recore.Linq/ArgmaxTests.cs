using System;
using System.Linq;

using Xunit;

namespace Recore.Linq.Tests
{
    public class ArgmaxTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => Renumerable.Argmax<object, int>(null, x => x.GetHashCode()));

            Assert.Throws<ArgumentNullException>(
                () => new[] { 0 }.Argmax<int, int>(null));
        }

        [Fact]
        public void EmptyEnumerable()
        {
            // Nullable types return null
            Assert.Null(Enumerable.Empty<string>().Argmax(x => x.GetHashCode()).Argmax);

            // Non-nullable types throw
            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<int>().Argmax(x => x * x));
        }

        [Fact]
        public void IncomparableValues()
        {
            // Singleton collection returns its only element
            var singletonCollection = new[]
            {
                new { Age = 1 }
            };

            Assert.Equal(singletonCollection[0], singletonCollection.Argmax(x => x).Argmax);

            // Multiple elements throws exception
            var collection = new[]
            {
                new { Age = 1 },
                new { Age = 2 }
            };

            Assert.Throws<ArgumentException>(() => collection.Argmax(x => x));
        }

        [Fact]
        public void ArgmaxObject()
        {
            var collection = new[]
            {
                new { Age = 1 },
                new { Age = 3 },
                new { Age = 2 }
            };

            Assert.Equal(
                (Argmax: collection[1], Max: 3),
                collection.Argmax(x => x.Age));
        }

        [Fact]
        public void ArgmaxInt32()
        {
            var collection = new[] { 1, 3, 4, 1 };

            Assert.Equal(
                (Argmax: 4, Max: 4),
                collection.Argmax(x => x));
        }
    }
}