using System;
using System.Collections.Generic;

using Xunit;

namespace Recore.Collections.Generic.Tests
{
    public class IDictionaryExtensionsTests
    {
        [Fact]
        public void Append_NullDictionary()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                IDictionary<string, int> dictionary = null;
                dictionary.Append("hello", 1);
            });
        }

        [Fact]
        public void Append_EmptyDictionary()
        {
            var dictionary = new Dictionary<string, string>()
                .Append("abc", null)
                .Append("hello", "world");

            var expected = new Dictionary<string, string>
            {
                ["abc"] = null,
                ["hello"] = "world"
            };

            Assert.Equal(expected, dictionary);
        }

        [Fact]
        public void Append_NonEmptyDictionary()
        {
            var dictionary = new Dictionary<string, int>
            {
                ["abc"] = 12
            }.Append("hello", -1);

            var expected = new Dictionary<string, int>
            {
                ["abc"] = 12,
                ["hello"] = -1
            };

            Assert.Equal(expected, dictionary);
        }

        [Fact]
        public void Append_ModifiesOriginalDictionary()
        {
            var originalDictionary = new Dictionary<string, int> { ["abc"] = 10 };
            var appendedDictionary = originalDictionary.Append("hello", 42);

            Assert.Equal(originalDictionary, appendedDictionary);
        }
    }
}
