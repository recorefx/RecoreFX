using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xunit;

namespace Recore.Collections.Generic.Tests
{
    public class IReadOnlyDictionaryExtensionsTests
    {
        // This should now be testing the standard library's method,
        // but we want to make sure there's no compile-time ambiguity.
        [Fact]
        public void GetValueOrDefault_HasValue()
        {
            var dictionary = new ReadOnlyDictionary<string, int>(new Dictionary<string, int>
            {
                ["abc"] = 12
            });

            Assert.Equal(12, dictionary.GetValueOrDefault("abc"));
        }
    }
}
