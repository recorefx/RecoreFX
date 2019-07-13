using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Linq.Tests
{
    [TestClass]
    public class EnumerateTests
    {
        /// <summary>
        /// A method that uses <c>yield return</c> won't evaluate its body until its result is enumerated.
        /// </summary>
        private static void ForceExecution<T>(Func<IEnumerable<T>> generator)
        {
            foreach (var x in generator())
            {
            }
        }

        [TestMethod]
        public void ThrowsOnNull()
        {
            IEnumerable<object> nullEnumerable = null;
            Assert.ThrowsException<ArgumentNullException>(() => ForceExecution(nullEnumerable.Enumerate));
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
