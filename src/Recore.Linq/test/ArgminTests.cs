using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Linq.Tests
{
    [TestClass]
    public class ArgminTests
    {
        [TestMethod]
        public void ThrowsOnNull()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => Renumerable.Argmin<object, int>(null, x => x.GetHashCode()));

            Assert.ThrowsException<ArgumentNullException>(
                () => new[] { 0 }.Argmin<int, int>(null));
        }

        [TestMethod]
        public void EmptyEnumerable()
        {
            // Nullable types return null
            Assert.IsNull(Enumerable.Empty<string>().Argmin(x => x.GetHashCode()));

            // Non-nullable types throw
            Assert.ThrowsException<InvalidOperationException>(
                () => Enumerable.Empty<int>().Argmin(x => x * x));
        }

        [TestMethod]
        public void IncomparableValues()
        {
            // Singleton collection returns its only element
            var singletonCollection = new[]
            {
                new { Age = 1 }
            };

            Assert.AreEqual(singletonCollection[0], singletonCollection.Argmin(x => x));

            // Multiple elements throws exception
            var collection = new[]
            {
                new { Age = 1 },
                new { Age = 2 }
            };

            Assert.ThrowsException<ArgumentException>(() => collection.Argmin(x => x));
        }

        [TestMethod]
        public void ArgminGeneric()
        {
            var collection = new[]
            {
                new { Age = 1 },
                new { Age = 3 },
                new { Age = 2 }
            };

            Assert.AreEqual(
                collection[0],
                collection.Argmin(x => x.Age));
        }
    }
}