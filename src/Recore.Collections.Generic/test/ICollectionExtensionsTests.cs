using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recore.Collections.Generic.Tests
{
    [TestClass]
    public class ICollectionExtensionsTests
    {
        [TestMethod]
        public void Append_NullCollection()
        {
            Assert.ThrowsException<NullReferenceException>(() =>
            {
                ICollection<string> collection = null;
                collection.Append("hello");
            });
        }

        [TestMethod]
        public void Append_NullItem()
        {
            var collection = new List<string>()
                .Append(null);

            var expected = new List<string>
            {
                null
            };

            Assert.IsTrue(collection.SequenceEqual(expected));
        }
    }
}