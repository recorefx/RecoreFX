using System.Text.Json;
using Xunit;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class TokenConverterTests
    {
        [Fact]
        public void ToJson()
        {
            var tokenString = "hello";
            var token = new Token(tokenString);

            Assert.Equal(
                expected: $"\"{tokenString}\"",
                actual: JsonSerializer.Serialize(token));
        }

        [Fact]
        public void FromJson()
        {
            var tokenString = "hello";

            Assert.Equal(
                expected: new Token(tokenString),
                actual: JsonSerializer.Deserialize<Token>($"\"{tokenString}\""));
        }
    }
}