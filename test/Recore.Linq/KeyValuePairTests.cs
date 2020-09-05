using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Recore.Linq.Tests
{
    public class KeyValuePairTests
    {
        [Fact]
        public void ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => Renumerable.ToDictionary<string, int>(null));

            Assert.Throws<ArgumentNullException>(
                () => Renumerable.OnKeys<string, int, int>(null, x => 1));

            Assert.Throws<ArgumentNullException>(
                () => Enumerable.Empty<KeyValuePair<string, int>>().OnKeys<string, int, int>(null));

            Assert.Throws<ArgumentNullException>(
                () => Renumerable.OnValues<string, int, int>(null, x => 1));

            Assert.Throws<ArgumentNullException>(
                () => Enumerable.Empty<KeyValuePair<string, int>>().OnValues<string, int, int>(null));
        }

        [Fact]
        public void ToDictionary()
        {
            var enumerable = new List<KeyValuePair<string, int>>
            {
                KeyValuePair.Create("burrito", 8),
                KeyValuePair.Create("taco", 6),
                KeyValuePair.Create("margarita", 7),
            };

            var dictionary = new Dictionary<string, int>
            {
                ["burrito"] = 8,
                ["taco"] = 6,
                ["margarita"] = 7
            };

            Assert.Equal(dictionary, enumerable.ToDictionary());
        }

        [Fact]
        public void OnKeys()
        {
            var dictionary = new Dictionary<string, int>
            {
                ["burrito"] = 8,
                ["taco"] = 6,
                ["margarita"] = 7,
            };

            var actual = dictionary
                .OnKeys(x => x.ToUpper())
                .ToDictionary();

            var expected = new Dictionary<string, int>
            {
                ["BURRITO"] = 8,
                ["TACO"] = 6,
                ["MARGARITA"] = 7,
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OnValues()
        {
            var dictionary = new Dictionary<string, int>
            {
                ["burrito"] = 8,
                ["taco"] = 6,
                ["margarita"] = 7,
            };

            var actual = dictionary
                .OnValues(x => 2 * x)
                .ToDictionary();

            var expected = new Dictionary<string, int>
            {
                ["burrito"] = 16,
                ["taco"] = 12,
                ["margarita"] = 14
            };

            Assert.Equal(expected, actual);
        }
    }
}
