using System.Linq;

using Xunit;

namespace Recore.Linq.Tests
{
    public class ZipTupleTests
    {
        [Fact]
        public void ZipSameLength()
        {
            var collection1 = new[] { "abc", string.Empty, "hello world" };
            var collection2 = new[] { 1, 2, 3 };

            var zipped = new[]
            {
                ("abc", 1),
                (string.Empty, 2),
                ("hello world", 3)
            };

            Assert.Equal(zipped, collection1.Zip(collection2).ToArray());
        }

        [Fact]
        public void ZipDifferentLength()
        {
            var empty = Enumerable.Empty<string>();
            var collection = new[] { 1, 2, 3 };

            Assert.Equal(
                Enumerable.Empty<(string, int)>().ToArray(),
                empty.Zip(collection).ToArray());

            Assert.Equal(
                Enumerable.Empty<(int, string)>().ToArray(),
                collection.Zip(empty).ToArray());
        }
    }
}