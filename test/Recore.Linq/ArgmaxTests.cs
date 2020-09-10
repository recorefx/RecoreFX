using System;
using System.Collections.Generic;
using System.Linq;

using FsCheck;
using FsCheck.Xunit;
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
        public void ArgmaxByIndex()
        {
            var collection = new[] { 1, 3, 4, 1 };

            Assert.Equal(
                (Argmax: 2, Max: 4),
                collection.Argmax());
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

        [Property]
        public void ArgmaxEqualsMaxForIdentityFunction(List<int> xs)
        {
            if (xs is null || xs.Count == 0)
            {
                return;
            }

            var (argmax, max) = xs.Argmax(x => x);
            Assert.Equal(max, argmax);
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Int32(List<int> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => collection.Max());
                Assert.Throws<InvalidOperationException>(() => collection.Argmax());
            }

            Assert.Equal(
                collection.Max(),
                collection.Argmax().Max);
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_NullableInt32(List<int?> collection)
        {
            if (collection is null)
            {
                return;
            }

            Assert.Equal(
                collection.Max(),
                collection.Argmax().Max);
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Int64(List<long> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => collection.Max());
                Assert.Throws<InvalidOperationException>(() => collection.Argmax());
            }

            Assert.Equal(
                collection.Max(),
                collection.Argmax().Max);
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_NullableInt64(List<long?> collection)
        {
            if (collection is null)
            {
                return;
            }

            Assert.Equal(
                collection.Max(),
                collection.Argmax().Max);
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Double(List<double> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => collection.Max());
                Assert.Throws<InvalidOperationException>(() => collection.Argmax());
            }

            Assert.Equal(
                collection.Max(),
                collection.Argmax().Max);
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_NullableDouble(List<double?> collection)
        {
            if (collection is null)
            {
                return;
            }

            Assert.Equal(
                collection.Max(),
                collection.Argmax().Max);
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Single(List<float> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => collection.Max());
                Assert.Throws<InvalidOperationException>(() => collection.Argmax());
            }

            Assert.Equal(
                collection.Max(),
                collection.Argmax().Max);
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_NullableSingle(List<float?> collection)
        {
            if (collection is null)
            {
                return;
            }

            Assert.Equal(
                collection.Max(),
                collection.Argmax().Max);
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Decimal(List<decimal> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => collection.Max());
                Assert.Throws<InvalidOperationException>(() => collection.Argmax());
            }

            Assert.Equal(
                collection.Max(),
                collection.Argmax().Max);
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_NullableDecimal(List<decimal?> collection)
        {
            if (collection is null)
            {
                return;
            }

            Assert.Equal(
                collection.Max(),
                collection.Argmax().Max);
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_String(List<string> collection)
        {
            if (collection is null)
            {
                return;
            }

            Assert.Equal(
                collection.Max(),
                collection.Argmax().Max);
        }
    }
}