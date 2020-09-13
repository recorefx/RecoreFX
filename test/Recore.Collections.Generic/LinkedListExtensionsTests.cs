using System;
using System.Collections.Generic;

using Xunit;

namespace Recore.Collections.Generic.Tests
{
    public class LinkedListExtensionsTests
    {
        [Fact]
        public void Append_NullList()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                LinkedList<string> list = null!;
                list.Append("hello");
            });
        }

        [Fact]
        public void Append_EmptyList()
        {
            var list = new LinkedList<string?>()
                .Append(null)
                .Append("hello");

            var expected = new LinkedList<string?>
            {
                null,
                "hello"
            };

            Assert.Equal(expected, list);
        }

        [Fact]
        public void Append_NonEmptyList()
        {
            var list = new LinkedList<string>
            {
                "abc"
            }.Append("hello");

            var expected = new LinkedList<string>
            {
                "abc",
                "hello"
            };

            Assert.Equal(expected, list);
        }

        [Fact]
        public void Append_ModifiesOriginalList()
        {
            var originalList = new LinkedList<string> { "abc" };
            var appendedList = originalList.Append("hello");

            Assert.Equal(originalList, appendedList);
        }
    }
}