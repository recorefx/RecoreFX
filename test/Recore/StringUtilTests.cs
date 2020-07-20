using Xunit;

namespace Recore.Tests
{
    public class StringUtilTests
    {
        [Fact]
        public void JoinLines()
        {
            var result = StringUtil.JoinLines(
                "hello",
                "world");

            var expected = @"hello
world";

            Assert.Equal(expected, result);
        }
    }
}