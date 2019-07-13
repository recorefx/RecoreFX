using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Linq.Tests
{
    [TestClass]
    public class ArgmaxTests
    {
        [TestMethod]
        public void ThrowsOnNull()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => Renumerable.Argmax<object, int>(null, x => x.GetHashCode()));

            Assert.ThrowsException<ArgumentNullException>(
                () => new[] { 0 }.Argmax<int, int>(null));
        }

        [TestMethod]
        public void EmptyEnumerable()
        {
            // Nullable types return null
            Assert.IsNull(Enumerable.Empty<string>().Argmax(x => x.GetHashCode()));

            // Non-nullable types throw
            Assert.ThrowsException<InvalidOperationException>(
                () => Enumerable.Empty<int>().Argmax(x => x * x));
        }

        [TestMethod]
        public void IncomparableValues()
        {
            // Singleton collection returns its only element
            var singletonCollection = new[]
            {
                new { Age = 1 }
            };

            Assert.AreEqual(singletonCollection[0], singletonCollection.Argmax(x => x));

            // Multiple elements throws exception
            var collection = new[]
            {
                new { Age = 1 },
                new { Age = 2 }
            };

            Assert.ThrowsException<ArgumentException>(() => collection.Argmax(x => x));
        }

        [TestMethod]
        public void ArgmaxGeneric()
        {
            var collection = new[]
            {
                new { Age = 1 },
                new { Age = 3 },
                new { Age = 2 }
            };

            Assert.AreEqual(
                collection[1],
                collection.Argmax(x => x.Age));
        }
    }
}