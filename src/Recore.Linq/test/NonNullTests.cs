using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Linq.Tests
{
    [TestClass]
    public class NonNullTests
    {
        [TestMethod]
        public void NullSource()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IEnumerable<string> source = null;
                return source.NonNull();
            });
        }

        [TestMethod]
        public void EmptyEnumerable()
        {
            var empty = Enumerable.Empty<string>();
            Assert.IsTrue(empty.NonNull().SequenceEqual(empty));
        }
    }
}