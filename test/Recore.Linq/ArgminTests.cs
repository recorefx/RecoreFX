using System;
using System.Linq;

using Xunit;

namespace Recore.Linq.Tests
{
    public class ArgminTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => Renumerable.Argmin<object, int>(null, x => x.GetHashCode()));

            Assert.Throws<ArgumentNullException>(
                () => new[] { 0 }.Argmin<int, int>(null));
        }

        [Fact]
        public void EmptyEnumerable()
        {
            // Nullable types return null
            Assert.Null(Enumerable.Empty<string>().Argmin(x => x.GetHashCode()).Argmin);

            // Non-nullable types throw
            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<int>().Argmin(x => x * x));
        }

        [Fact]
        public void IncomparableValues()
        {
            // Singleton collection returns its only element
            var singletonCollection = new[]
            {
                new { Age = 1 }
            };

            Assert.Equal(singletonCollection[0], singletonCollection.Argmin(x => x).Argmin);

            // Multiple elements throws exception
            var collection = new[]
            {
                new { Age = 1 },
                new { Age = 2 }
            };

            Assert.Throws<ArgumentException>(() => collection.Argmin(x => x).Argmin);
        }

        [Fact]
        public void ArgminObject()
        {
            var collection = new[]
            {
                new { Age = 1 },
                new { Age = 3 },
                new { Age = 2 }
            };

            Assert.Equal(
                (Argmin: collection[0], Min: 1),
                collection.Argmin(x => x.Age));
        }

        [Fact]
        public void ArgminInt32()
        {
            var collection = new[] { 1, 3, 4, 1 };

            Assert.Equal(
                (Argmin: 1, Min: 1),
                collection.Argmin(x => x));
        }
    }
}