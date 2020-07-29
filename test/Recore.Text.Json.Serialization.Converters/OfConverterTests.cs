using System.Security.Cryptography;
using Xunit;

using Recore.Security.Cryptography;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class OfConverterFactoryTests
    {
        private enum SomeEnum { Yes, No }
        private class DerivedOfString : Of<string> { }
        private class DerivedDerivedOfString : Of<string> { }

        [Fact]
        public void CanConvert()
        {
            var factory = new OfConverterFactory();

            Assert.True(factory.CanConvert(typeof(Of<>)));
            Assert.True(factory.CanConvert(typeof(Of<string>)));
            Assert.True(factory.CanConvert(typeof(Of<int>)));
            Assert.True(factory.CanConvert(typeof(Of<SomeEnum>)));
            Assert.True(factory.CanConvert(typeof(Of<Of<object>>)));
            Assert.True(factory.CanConvert(typeof(Token)));
            Assert.True(factory.CanConvert(typeof(Ciphertext<SHA1>)));
            Assert.True(factory.CanConvert(typeof(DerivedOfString)));
            Assert.True(factory.CanConvert(typeof(DerivedDerivedOfString)));

            Assert.False(factory.CanConvert(typeof(string)));
            Assert.False(factory.CanConvert(typeof(object)));
        }
    }
}
