using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace Recore.Linq.Tests
{
    public class ToAsyncEnumerableTests
    {
        [Fact]
        public async Task ThrowsOnNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                Task<IEnumerable<object>> taskEnumerable = null!;
                await foreach (var x in taskEnumerable.ToAsyncEnumerable())
                {
                }
            });

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                IEnumerable<Task<object>> enumerableTask = null!;
                await foreach (var x in enumerableTask.ToAsyncEnumerable())
                {
                }
            });
        }

        [Fact]
        public async Task ToAsyncEnumerable_TaskEnumerable()
        {
            var expected = new[]
            {
                "hello",
                "world"
            };

            Task<IEnumerable<string>> GetStringsAsync()
                => Task.FromResult<IEnumerable<string>>(expected);

            var i = 0;
            await foreach (var s in GetStringsAsync().ToAsyncEnumerable())
            {
                Assert.Equal(expected[i++], s);
            }
        }

        [Fact]
        public async Task ToAsyncEnumerable_EnumerableTask()
        {
            var expected = new[]
            {
                "hello",
                "world"
            };

            IEnumerable<Task<string>> GetStrings()
                => expected.Select(Task.FromResult<string>);

            var i = 0;
            await foreach (var s in GetStrings().ToAsyncEnumerable())
            {
                Assert.Equal(expected[i++], s);
            }
        }
    }
}
