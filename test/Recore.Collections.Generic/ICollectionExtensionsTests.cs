using System;
using System.Collections.Generic;

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
        public void Append_EmptyCollection()
        {
            var collection = new List<string>()
                .Append(null)
                .Append("hello");

            var expected = new List<string>
            {
                null,
                "hello"
            };

            Assert.Equal(expected, collection);
        }

        [Fact]
        public void Append_NonEmptyCollection()
        {
            var collection = new List<string>
            {
                "abc"
            }.Append("hello");

            var expected = new List<string>
            {
                "abc",
                "hello"
            };

            Assert.Equal(expected, collection);
        }

        [Fact]
        public void Append_ModifiesOriginalCollection()
        {
            var originalCollection = new List<string> { "abc" };
            var appendedCollection = originalCollection.Append("hello");

            Assert.Equal(originalCollection, appendedCollection);
        }
    }
}