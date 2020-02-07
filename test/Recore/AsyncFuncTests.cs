using System.Threading.Tasks;

using Xunit;

namespace Recore.Tests
{
    public class AsyncFuncTests
    {
        private class AsyncFoo
        {
            private readonly AsyncFunc<int> func;

            public AsyncFoo(AsyncFunc<int> func)
            {
                this.func = func;
            }

            public async Task<int> CallFuncAsync() => await func();
        }

        private class AsyncFooWithArgs
        {
            private readonly AsyncFunc<string, bool, int> func;

            public AsyncFooWithArgs(AsyncFunc<string, bool, int> func)
            {
                this.func = func;
            }

            public async Task<int> CallFuncAsync(string s, bool b) => await func(s, b);
        }

        private static Task SomeMethodAsync() => Task.CompletedTask;

        [Fact]
        public async Task AsyncFuncNoArgs()
        {
            bool calledFunc = false;
            var foo = new AsyncFoo(async () =>
            {
                await SomeMethodAsync();
                calledFunc = true;
                return 0;
            });

            Assert.False(calledFunc);
            Assert.Equal(0, await foo.CallFuncAsync());
            Assert.True(calledFunc);
        }

        [Fact]
        public async Task AsyncFuncWithArgs()
        {
            bool calledFunc = false;
            var foo = new AsyncFooWithArgs(async (string s, bool b) =>
            {
                await SomeMethodAsync();
                calledFunc = true;
                return s.Length + (b ? 1 : 0);
            });

            Assert.False(calledFunc);
            Assert.Equal(0, await foo.CallFuncAsync(string.Empty, false));
            Assert.True(calledFunc);
        }
    }
}
