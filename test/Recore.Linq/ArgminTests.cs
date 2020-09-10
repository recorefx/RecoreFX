using System;
using System.Collections.Generic;
using System.Linq;

using FsCheck;
using FsCheck.Xunit;
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
            // With no selector, always throws
            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<int>().Argmin().Argmin);

            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<string>().Argmin().Argmin);

            // When one or both of TSource and TResult is non-nullable, throws
            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<string>().Argmin(x => x.Length).Argmin);

            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<int>().Argmin(x => string.Empty).Argmin);

            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<int>().Argmin(x => x * x));

            // When both of TSource and TResult are non-nullable, returns null
            Assert.Null(Enumerable.Empty<string>().Argmin(x => x.ToUpper()).Argmin);
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
        public void ArgminByIndex()
        {
            var collection = new[] { 1, 3, 4, 1 };

            Assert.Equal(
                (Argmin: 0, Min: 1),
                collection.Argmin());
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

        [Property]
        public void ArgminEqualsMinForIdentityFunction(List<int> xs)
        {
            if (xs is null || xs.Count == 0)
            {
                return;
            }

            var (argmin, min) = xs.Argmin(x => x);
            Assert.Equal(argmin, min);
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Int32(List<int> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => collection.Min());
                Assert.Throws<InvalidOperationException>(() => collection.Argmin());
            }
            else
            {
                Assert.Equal(
                    collection.Min(),
                    collection.Argmin().Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_NullableInt32(List<int?> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Min());
                Assert.Throws<InvalidOperationException>(() => collection.Argmin());
            }
            else
            {
                Assert.Equal(
                    collection.Min(),
                    collection.Argmin().Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Int64(List<long> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => collection.Min());
                Assert.Throws<InvalidOperationException>(() => collection.Argmin());
            }
            else
            {
                Assert.Equal(
                    collection.Min(),
                    collection.Argmin().Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_NullableInt64(List<long?> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Min());
                Assert.Throws<InvalidOperationException>(() => collection.Argmin());
            }
            else
            {
                Assert.Equal(
                    collection.Min(),
                    collection.Argmin().Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Double(List<double> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => collection.Min());
                Assert.Throws<InvalidOperationException>(() => collection.Argmin());
            }
            else
            {
                Assert.Equal(
                    collection.Min(),
                    collection.Argmin().Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_NullableDouble(List<double?> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Min());
                Assert.Throws<InvalidOperationException>(() => collection.Argmin());
            }
            else
            {
                Assert.Equal(
                    collection.Min(),
                    collection.Argmin().Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Single(List<float> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => collection.Min());
                Assert.Throws<InvalidOperationException>(() => collection.Argmin());
            }
            else
            {
                Assert.Equal(
                    collection.Min(),
                    collection.Argmin().Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_NullableSingle(List<float?> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Min());
                Assert.Throws<InvalidOperationException>(() => collection.Argmin());
            }
            else
            {
                Assert.Equal(
                    collection.Min(),
                    collection.Argmin().Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Decimal(List<decimal> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => collection.Min());
                Assert.Throws<InvalidOperationException>(() => collection.Argmin());
            }
            else
            {
                Assert.Equal(
                    collection.Min(),
                    collection.Argmin().Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_NullableDecimal(List<decimal?> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Min());
                Assert.Throws<InvalidOperationException>(() => collection.Argmin());
            }
            else
            {
                Assert.Equal(
                    collection.Min(),
                    collection.Argmin().Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_String(List<string> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Min());
                Assert.Throws<InvalidOperationException>(() => collection.Argmin());
            }
            else
            {
                Assert.Equal(
                    collection.Min(),
                    collection.Argmin().Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Selector_Int32(List<int> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => source.Min(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmin(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Min(x => projected[x]),
                    source.Argmin(x => projected[x]).Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Selector_NullableInt32(List<int?> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Min(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmin(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Min(x => projected[x]),
                    source.Argmin(x => projected[x]).Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Selector_Int64(List<long> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => source.Min(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmin(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Min(x => projected[x]),
                    source.Argmin(x => projected[x]).Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Selector_NullableInt64(List<long?> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Min(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmin(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Min(x => projected[x]),
                    source.Argmin(x => projected[x]).Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Selector_Double(List<double> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => source.Min(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmin(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Min(x => projected[x]),
                    source.Argmin(x => projected[x]).Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Selector_NullableDouble(List<double?> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Min(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmin(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Min(x => projected[x]),
                    source.Argmin(x => projected[x]).Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Selector_Single(List<float> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => source.Min(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmin(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Min(x => projected[x]),
                    source.Argmin(x => projected[x]).Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Selector_NullableSingle(List<float?> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Min(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmin(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Min(x => projected[x]),
                    source.Argmin(x => projected[x]).Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Selector_Decimal(List<decimal> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => source.Min(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmin(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Min(x => projected[x]),
                    source.Argmin(x => projected[x]).Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Selector_NullableDecimal(List<decimal?> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Min(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmin(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Min(x => projected[x]),
                    source.Argmin(x => projected[x]).Min);
            }
        }

        [Property]
        public void MinAlwaysEqualsEnumerableMin_Selector_String(List<string> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Min(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmin(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Min(x => projected[x]),
                    source.Argmin(x => projected[x]).Min);
            }
        }
    }
}