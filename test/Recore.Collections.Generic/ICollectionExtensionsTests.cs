using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Recore.Collections.Generic.Tests
{
    public class ICollectionExtensionsTests
    {
        [Fact]
        public void Append_NullCollection()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                ICollection<string> collection = null;
                collection.Append("hello");
            });
        }

        [Fact]
        public void Append_NullItem()
        {
            var collection = new List<string>()
                .Append(null);

            var expected = new List<string>
            {
                null
            };

            Assert.True(collection.SequenceEqual(expected));
        }
    }
}