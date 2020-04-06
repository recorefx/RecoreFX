using System;
using System.Collections.Generic;

using Xunit;

namespace Recore.Collections.Generic.Tests
{
    public class IDictionaryExtensionsTests
    {
        [Fact]
        public void ValueOrDefault_NullDictionary()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                IDictionary<string, int> dictionary = null;
                dictionary.ValueOrDefault("hello");
            });
        }

        [Fact]
        public void ValueOrDefault_HasValue()
        {
            var dictionary = new Dictionary<string, int>
            {
                ["abc"] = 12
            };

            Assert.Equal(12, dictionary.ValueOrDefault("abc"));
        }

        [Fact]
        public void ValueOrDefault_Default()
        {
            var dictionary = new Dictionary<string, int>
            {
                ["abc"] = 12
            };

            Assert.Equal(default, dictionary.ValueOrDefault("xyz"));
        }

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

        [Fact]
        public void GetOrAdd_NullDictionary()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                IDictionary<string, int> dictionary = null;
                dictionary.GetOrAdd("hello", 1);
            });
        }

        [Fact]
        public void GetOrAdd_Get()
        {
            var dictionary = new Dictionary<string, int>
            {
                ["abc"] = 12
            };

            Assert.Equal(12, dictionary.GetOrAdd("abc", -1));
        }

        [Fact]
        public void GetOrAdd_Add()
        {
            var dictionary = new Dictionary<string, int>
            {
                ["abc"] = 12
            };

            Assert.Equal(-1, dictionary.GetOrAdd("xyz", -1));
        }
    }
}
