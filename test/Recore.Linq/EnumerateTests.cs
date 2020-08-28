using System;
using System.Collections.Generic;
using System.Linq;

using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace Recore.Linq.Tests
{
    public class EnumerateTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            IEnumerable<object> nullEnumerable = null;
            Assert.Throws<ArgumentNullException>(
                () => TestHelpers.ForceExecution(nullEnumerable.Enumerate));
        }

        [Fact]
        public void EmptyEnumerable()
        {
            var result = Enumerable.Empty<object>().Enumerate();
            Assert.False(result.Any());
        }

        [Fact]
        public void NullElement()
        {
            var enumerable = new string[] { null };

            var expected = new (int, string)[]
            {
                (0, null)
            };

            var result = enumerable.Enumerate();
            Assert.Equal(expected, result.ToList());
        }

        [Fact]
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
            Assert.Equal(expected, result.ToList());
        }

        [Property]
        public Property FirstIndexIsAlwaysZero()
        {
            return Prop.ForAll((List<string> source) =>
            {
                if (source is null)
                {
                    // Skip
                    return true;
                }

                if (source.Count == 0)
                {
                    // Skip
                    return true;
                }

                var result = source.Enumerate();
                return result.First().index == 0;
            });
        }

        [Property]
        public Property LastIndexIsAlwaysLength()
        {
            return Prop.ForAll((List<string> source) =>
            {
                if (source is null)
                {
                    // Skip
                    return true;
                }

                if (source.Count == 0)
                {
                    // Skip
                    return true;
                }

                var result = source.Enumerate();
                return result.Last().index == source.Count - 1;
            });
        }

        [Property]
        public Property IndicesAreAlwaysSequential()
        {
            return Prop.ForAll((List<string> source) =>
            {
                if (source is null)
                {
                    // Skip
                    return true;
                }

                var result = source.Enumerate().ToList();

                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].index != i)
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        [Property]
        public Property IsAlwaysOriginalSequence()
        {
            return Prop.ForAll((List<string> source) =>
            {
                if (source is null)
                {
                    // Skip
                    return true;
                }

                var result = source.Enumerate().ToList();

                if (source.Count != result.Count)
                {
                    return false;
                }

                for (var i = 0; i < source.Count; i++)
                {
                    if (source[i] != result[i].item)
                    {
                        return false;
                    }
                }

                return true;
            });
        }
    }
}
