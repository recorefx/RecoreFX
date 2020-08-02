﻿using System.Linq;
using System.Text.Json;
using Xunit;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class OptionalConverterTests
    {
        class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

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
                actual: deserializedPerson.First().Name);

            Assert.Equal(
                expected: 42,
                actual: deserializedPerson.First().Age);

            Assert.Throws<JsonException>(
                () => JsonSerializer.Deserialize<Optional<int>>("hello"));

            Assert.Throws<JsonException>(
                () => JsonSerializer.Deserialize<Optional<string>>("12"));
        }
    }
}