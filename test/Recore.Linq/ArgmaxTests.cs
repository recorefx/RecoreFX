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
            // Non-nullable TSource throws
            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<int>().Argmax().Argmax);

            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<string>().Argmax().Argmax);

            // When one or both of TSource and TResult is non-nullable, throws
            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<string>().Argmax(x => x.Length).Argmax);

            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<int>().Argmax(x => string.Empty).Argmax);

            Assert.Throws<InvalidOperationException>(
                () => Enumerable.Empty<int>().Argmax(x => x * x));

            // When both of TSource and TResult are non-nullable, returns null
            Assert.Null(Enumerable.Empty<string>().Argmax(x => x.ToUpper()).Argmax);
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
        public void ArgmaxInt32()
        {
            var collection = new[] { 1, 3, 4, 1 };

            Assert.Equal(
                (Argmax: 4, Max: 4),
                collection.Argmax(x => x));
        }

        [Fact]
        public void ArgmaxGeneric()
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
        public void ArgmaxGenericWithNull()
        {
            var collection = new[]
            {
                "abc",
                null,
                "hello world"
            };

            Assert.Equal(
                (Argmax: collection[2], Max: 11),
                collection.Argmax(x => x?.Length));
        }

        [Fact]
        public void ArgmaxGenericAllNull()
        {
            var collection = new string?[]
            {
                null,
                null
            };

            Assert.Equal(
                (Argmax: null, Max: null),
                collection.Argmax(x => x?.Length));
        }

        [Fact]
        public void ArgmaxGenericNRE()
        {
            var collection = new string[]
            {
            };

            var argmax = collection.Argmax(x => x?.Length).Argmax;
            Assert.Throws<NullReferenceException>(() => argmax.Length);
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
            else
            {
                Assert.Equal(
                    collection.Max(),
                    collection.Argmax().Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_NullableInt32(List<int?> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Max());
                Assert.Throws<InvalidOperationException>(() => collection.Argmax());
            }
            else
            {
                Assert.Equal(
                    collection.Max(),
                    collection.Argmax().Max);
            }
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
            else
            {
                Assert.Equal(
                    collection.Max(),
                    collection.Argmax().Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_NullableInt64(List<long?> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Max());
                Assert.Throws<InvalidOperationException>(() => collection.Argmax());
            }
            else
            {
                Assert.Equal(
                    collection.Max(),
                    collection.Argmax().Max);
            }
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
            else
            {
                Assert.Equal(
                    collection.Max(),
                    collection.Argmax().Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_NullableDouble(List<double?> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Max());
                Assert.Throws<InvalidOperationException>(() => collection.Argmax());
            }
            else
            {
                Assert.Equal(
                    collection.Max(),
                    collection.Argmax().Max);
            }
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
            else
            {
                Assert.Equal(
                    collection.Max(),
                    collection.Argmax().Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_NullableSingle(List<float?> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Max());
                Assert.Throws<InvalidOperationException>(() => collection.Argmax());
            }
            else
            {
                Assert.Equal(
                    collection.Max(),
                    collection.Argmax().Max);
            }
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
            else
            {
                Assert.Equal(
                    collection.Max(),
                    collection.Argmax().Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_NullableDecimal(List<decimal?> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Max());
                Assert.Throws<InvalidOperationException>(() => collection.Argmax());
            }
            else
            {
                Assert.Equal(
                    collection.Max(),
                    collection.Argmax().Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_String(List<string> collection)
        {
            if (collection is null)
            {
                return;
            }

            if (collection.Count == 0)
            {
                Assert.Null(collection.Max());
                Assert.Throws<InvalidOperationException>(() => collection.Argmax());
            }
            else
            {
                Assert.Equal(
                    collection.Max(),
                    collection.Argmax().Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Selector_Int32(List<int> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => source.Max(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmax(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Max(x => projected[x]),
                    source.Argmax(x => projected[x]).Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Selector_NullableInt32(List<int?> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Max(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmax(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Max(x => projected[x]),
                    source.Argmax(x => projected[x]).Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Selector_Int64(List<long> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => source.Max(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmax(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Max(x => projected[x]),
                    source.Argmax(x => projected[x]).Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Selector_NullableInt64(List<long?> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Max(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmax(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Max(x => projected[x]),
                    source.Argmax(x => projected[x]).Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Selector_Double(List<double> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => source.Max(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmax(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Max(x => projected[x]),
                    source.Argmax(x => projected[x]).Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Selector_NullableDouble(List<double?> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Max(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmax(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Max(x => projected[x]),
                    source.Argmax(x => projected[x]).Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Selector_Single(List<float> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => source.Max(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmax(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Max(x => projected[x]),
                    source.Argmax(x => projected[x]).Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Selector_NullableSingle(List<float?> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Max(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmax(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Max(x => projected[x]),
                    source.Argmax(x => projected[x]).Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Selector_Decimal(List<decimal> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Throws<InvalidOperationException>(() => source.Max(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmax(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Max(x => projected[x]),
                    source.Argmax(x => projected[x]).Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Selector_NullableDecimal(List<decimal?> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Max(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmax(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Max(x => projected[x]),
                    source.Argmax(x => projected[x]).Max);
            }
        }

        [Property]
        public void MaxAlwaysEqualsEnumerableMax_Selector_String(List<string> projected)
        {
            if (projected is null)
            {
                return;
            }

            var source = Enumerable.Range(0, projected.Count).ToList();
            if (source.Count == 0)
            {
                Assert.Null(source.Max(x => projected[x]));
                Assert.Throws<InvalidOperationException>(() => source.Argmax(x => projected[x]));
            }
            else
            {
                Assert.Equal(
                    source.Max(x => projected[x]),
                    source.Argmax(x => projected[x]).Max);
            }
        }
    }
}