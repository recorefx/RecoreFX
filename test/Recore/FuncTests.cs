using Xunit;

namespace Recore.Tests
{
    public class FuncTests
    {
        [Fact]
        public void Invoke()
        {
            string name = null;
            var result = Func.Invoke(() =>
            {
                if (name == null)
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
    }
}
