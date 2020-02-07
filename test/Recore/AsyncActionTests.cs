using System.Threading.Tasks;

using Xunit;

namespace Recore.Tests
{
    public class AsyncActionTests
    {
        private class AsyncFoo
        {
            private readonly AsyncAction action;

            public AsyncFoo(AsyncAction action)
            {
                this.action = action;
            }

            public async Task CallActionAsync() => await action();
        }

        private class AsyncFooWithArgs
        {
            private readonly AsyncAction<string, bool> action;

            public AsyncFooWithArgs(AsyncAction<string, bool> action)
            {
                this.action = action;
            }

            public async Task CallActionAsync(string s, bool b) => await action(s, b);
        }

        private static Task SomeMethodAsync() => Task.CompletedTask;

        [Fact]
        public async Task AsyncActionNoArgs()
        {
            bool calledAction = false;
            var foo = new AsyncFoo(async () =>
            {
                await SomeMethodAsync();
                calledAction = true;
            });

            Assert.False(calledAction);
            await foo.CallActionAsync();
            Assert.True(calledAction);
        }

        [Fact]
        public async Task AsyncActionWithArgs()
        {
            bool calledAction = false;
            var foo = new AsyncFooWithArgs(async (string s, bool b) =>
            {
                await SomeMethodAsync();
                calledAction = true;
            });

            Assert.False(calledAction);
            await foo.CallActionAsync(string.Empty, false);
            Assert.True(calledAction);
        }
    }
}
