using System;
using System.Text.Json;
using Xunit;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class RelativeUriConverterTests
    {
        [Fact]
        public void ToJson()
        {
            var uriString = "index.html";
            var relativeUri = new RelativeUri(uriString);

            Assert.Equal(
                expected: $"\"{uriString}\"",
                actual: JsonSerializer.Serialize(relativeUri));

            // Spec check: make sure Uri serializes the same way
            Assert.Equal(
                expected: $"\"{uriString}\"",
                actual: JsonSerializer.Serialize(relativeUri.StaticCast<Uri>()));
        }

        [Fact]
        public void FromJson()
        {
            var uriString = "index.html";

            Assert.Equal(
                expected: new RelativeUri(uriString),
                actual: JsonSerializer.Deserialize<RelativeUri>($"\"{uriString}\""));

            // Spec check: make sure Uri deserializes the same way
            Assert.Equal(
                expected: new Uri(uriString, UriKind.Relative),
                actual: JsonSerializer.Deserialize<Uri>($"\"{uriString}\""));
        }
    }
}