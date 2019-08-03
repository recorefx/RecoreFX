using Xunit;

namespace Recore.Tests
{
    public class UnitTests
    {
        [Fact]
        public void Close()
        {
            bool called = false;

            var result = Unit.Close(() =>
            {
                called = true;
            })();

            Assert.True(called);
            Assert.Equal(new Unit(), result);
        }
    }
}