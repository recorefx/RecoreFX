using System;
using System.Text.Json;
using Xunit;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class AbsoluteUriConverterTests
    {
        [Fact]
        public void ToJson()
        {
            var uriString = "https://example.com/index.html";
            var absoluteUri = new AbsoluteUri(uriString);

            Assert.Equal(
                expected: $"\"{uriString}\"",
                actual: JsonSerializer.Serialize(absoluteUri));

            // Spec check: make sure Uri serializes the same way
            Assert.Equal(
                expected: $"\"{uriString}\"",
                actual: JsonSerializer.Serialize(absoluteUri.StaticCast<Uri>()));
        }

        [Fact]
        public void FromJson()
        {
            var uriString = "https://example.com/index.html";

            Assert.Equal(
                expected: new AbsoluteUri(uriString),
                actual: JsonSerializer.Deserialize<AbsoluteUri>($"\"{uriString}\""));

            // Spec check: make sure Uri deserializes the same way
            Assert.Equal(
                expected: new Uri(uriString),
                actual: JsonSerializer.Deserialize<Uri>($"\"{uriString}\""));
        }
    }
}