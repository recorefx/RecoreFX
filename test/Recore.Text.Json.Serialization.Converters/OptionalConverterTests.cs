using System.Text.Json;
using Xunit;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class OptionalConverterTests
    {
        [Fact]
        public void ToJson()
        {
            Assert.Equal(
                expected: "12",
                actual: JsonSerializer.Serialize(Optional.Of(12)));

            Assert.Equal(
               expected: "null",
               actual: JsonSerializer.Serialize(Optional<int>.Empty));

            Assert.Equal(
                expected: "\"hello\"",
                actual: JsonSerializer.Serialize(Optional.Of("hello")));

            Assert.Equal(
               expected: "null",
               actual: JsonSerializer.Serialize(Optional<string>.Empty));

            Assert.Equal(
                expected: "{\"Name\":\"Mario\",\"Age\":42}",
                actual: JsonSerializer.Serialize(Optional.Of(new Person { Name = "Mario", Age = 42 })));
        }

        [Fact]
        public void ToJsonWithSpecialConverter()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialStringConverter());

            Assert.Equal(
                expected: "{\"value\":\"hello\",\"length\":5}",
                actual: JsonSerializer.Serialize(Optional.Of("hello"), options));
        }

        [Fact]
        public void FromJson()
        {
            Assert.Equal(
                expected: Optional.Of(12),
                actual: JsonSerializer.Deserialize<Optional<int>>("12"));

            Assert.Equal(
                expected: Optional<int>.Empty,
                actual: JsonSerializer.Deserialize<Optional<int>>("null"));

            Assert.Equal(
                expected: Optional.Of("hello"),
                actual: JsonSerializer.Deserialize<Optional<string>>("\"hello\""));

            Assert.Equal(
                expected: Optional<string>.Empty,
                actual: JsonSerializer.Deserialize<Optional<string>>("null"));

            var deserializedPerson = JsonSerializer.Deserialize<Optional<Person>>("{\"Name\":\"Mario\",\"Age\":42}");
            Assert.Equal(
                expected: "Mario",
                actual: deserializedPerson.OnValue(x => x.Name));

            Assert.Equal(
                expected: 42,
                actual: deserializedPerson.OnValue(x => x.Age));

            Assert.Throws<JsonException>(
                () => JsonSerializer.Deserialize<Optional<int>>("hello"));

            Assert.Throws<JsonException>(
                () => JsonSerializer.Deserialize<Optional<string>>("12"));
        }

        [Fact]
        public void FromJsonWithSpecialConverter()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialStringConverter());

            Assert.Equal(
                expected: Optional.Of("hello"),
                actual: JsonSerializer.Deserialize<Optional<string>>("{\"value\":\"hello\",\"length\":5}", options));
        }
    }
}
