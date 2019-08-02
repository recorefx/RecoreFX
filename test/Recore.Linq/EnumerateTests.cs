using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Linq.Tests
{
    [TestClass]
    public class EnumerateTests
    {
        [TestMethod]
        public void ThrowsOnNull()
        {
            IEnumerable<object> nullEnumerable = null;
            Assert.ThrowsException<ArgumentNullException>(
                () => TestHelpers.ForceExecution(nullEnumerable.Enumerate));
        }

        [TestMethod]
        public void EmptyEnumerable()
        {
            var result = Enumerable.Empty<object>().Enumerate();
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
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
            CollectionAssert.AreEqual(expected, result.ToList());
        }
    }
}
