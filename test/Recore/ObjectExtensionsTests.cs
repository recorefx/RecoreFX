using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Recore.Tests
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void StaticCast()
        {
            var xs = new[] { 0, 1, 2, 3 };
            Assert.Same(xs, xs.StaticCast<IEnumerable<int>>());
        }

        [Fact]
        public void StaticCast_Null()
        {
            int[]? xs = null;
            Assert.Same(xs, xs.StaticCast<IEnumerable<int>?>());
        }

        [Fact]
        public void Apply_ThrowsOnNull()
        {
            Func<string, int> func = null!;
            Assert.Throws<ArgumentNullException>(() => "hello".Apply(func));
        }

        [Fact]
        public void Apply()
        {
            var result = "hello"
                .Apply(x => x.Length)
                .Apply(IsEven);

            Assert.False(result);
        }

        [Fact]
        public async Task Apply_Task()
        {
            var result = await "hello"
                .Apply(x => Task.FromResult(x.Length))
                .Apply(async x => IsEven(await x));

            Assert.False(result);
        }

        [Fact]
        public async Task ApplyAsync_ThrowsOnNull()
        {
            AsyncFunc<string, int> func = null!;
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Task.FromResult("hello").ApplyAsync(func);
            });
        }

        [Fact]
        public async Task ApplyAsync()
        {
            var result = await "hello"
                .Apply(x => Task.FromResult(x.Length))
                .ApplyAsync(IsEvenAsync);

            Assert.False(result);
        }

        private static bool IsEven(int n) => n % 2 == 0;
        private static Task<bool> IsEvenAsync(int n) => Task.FromResult(n % 2 == 0);
    }
}
