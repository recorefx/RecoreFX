using Xunit;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class OfConverterTests
    {
        private class DerivedOfString : Of<string> { }

        [Fact]
        public void CanConvert()
        {
            var factory = new OfConverterFactory();

            Assert.True(factory.CanConvert(typeof(Of<>)));
            Assert.True(factory.CanConvert(typeof(Of<string>)));
            Assert.True(factory.CanConvert(typeof(DerivedOfString)));

            Assert.False(factory.CanConvert(typeof(string)));
            Assert.False(factory.CanConvert(typeof(object)));
        }
    }
}
