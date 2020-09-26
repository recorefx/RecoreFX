using System.Threading.Tasks;
using Xunit;

namespace Recore.Tests
{
    public class FuncTests
    {
        [Fact]
        public void Invoke()
        {
            string? name = null;
            var result = Func.Invoke(() =>
            {
                if (name is null)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });

            Assert.Equal(-1, result);
        }

        [Fact]
        public void Fluent()
        {
            var obj = new object();
            var fluent = Func.Fluent((object _) => { });
            Assert.Same(obj, fluent(obj));
        }

        [Fact]
        public async Task AsyncFluent()
        {
            var obj = new object();
            var fluent = Func.AsyncFluent((object _) => Task.CompletedTask);
            Assert.Same(obj, await fluent(obj));
        }
    }
}
