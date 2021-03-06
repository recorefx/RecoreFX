﻿using System;
using System.Text.Json;
using Xunit;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class ResultConverterTests
    {
        [Fact]
        public void ToJson()
        {
            {
                Result<int, string> result;

                result = 12;
                Assert.Equal(
                    expected: "12",
                    actual: JsonSerializer.Serialize(result));

                result = "hello";
                Assert.Equal(
                    expected: "\"hello\"",
                    actual: JsonSerializer.Serialize(result));
            }
            {
                Result<Person, Exception> result;

                result = new Person { Name = "Mario", Age = 42 };
                Assert.Equal(
                    expected: "{\"Name\":\"Mario\",\"Age\":42}",
                    actual: JsonSerializer.Serialize(result));
            }
        }

        [Fact]
        public void ToJsonWithSpecialConverter()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialStringConverter());

            Result<string, Exception> result = "hello";

            Assert.Equal(
                expected: "{\"value\":\"hello\",\"length\":5}",
                actual: JsonSerializer.Serialize(result, options));
        }

        [Fact]
        public void FromJson()
        {
            Assert.Equal(
                expected: new Result<int, string>(12),
                actual: JsonSerializer.Deserialize<Result<int, string>>("12"));

            Assert.Equal(
                expected: new Result<int, string>("hello"),
                actual: JsonSerializer.Deserialize<Result<int, string>>("\"hello\""));

            Assert.Equal(
                expected: new Result<string, int>(12),
                actual: JsonSerializer.Deserialize<Result<string, int>>("12"));

            Assert.Equal(
                expected: new Result<string, int>("hello"),
                actual: JsonSerializer.Deserialize<Result<string, int>>("\"hello\""));

            var deserializedPerson = JsonSerializer.Deserialize<Result<Person, Exception>>("{\"Name\":\"Mario\",\"Age\":42}")!;
            Assert.Equal(
                expected: "Mario",
                actual: deserializedPerson.GetValue().OnValue(x => x.Name));

            Assert.Equal(
                expected: 42,
                actual: deserializedPerson.GetValue().OnValue(x => x.Age));
        }

        [Fact]
        public void FromJsonWithSpecialConverter()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialStringConverter());

            Assert.Equal(
                expected: new Result<string, Exception>("hello"),
                actual: JsonSerializer.Deserialize<Result<string, Exception>>("{\"value\":\"hello\",\"length\":5}", options));
        }

        [Fact]
        public void FromJsonBothRecordTypesAlwaysDeserializesLeft()
        {
            {
                var deserializedPerson = JsonSerializer.Deserialize<Result<Person, Address>>("{\"Name\":\"Mario\",\"Age\":42}")!;
                Assert.Equal(
                    expected: "Mario",
                    actual: deserializedPerson.GetValue().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 42,
                    actual: deserializedPerson.GetValue().OnValue(x => x.Age));

                // This will deserialize as Person instead of Address because TValue = Person.
                var deserializedAddress = JsonSerializer.Deserialize<Result<Person, Address>>("{\"Street\":\"123 Main St\",\"Zip\":\"12345\"}")!;
                Assert.True(deserializedAddress.IsSuccessful);

                Assert.False(
                    deserializedAddress.GetValue().OnValue(x => x.Name).HasValue);

                Assert.Equal(
                    expected: (int)default,
                    actual: deserializedAddress.GetValue().OnValue(x => x.Age));
            }
        }

        [Fact]
        public void FromJsonObjectTypesWithConverter()
        {
            { // This will always deserialize the left type because it is a POCO
                var deserializedPerson = JsonSerializer.Deserialize<Result<Person, TypeWithConverter>>("{\"Name\":\"Mario\",\"Age\":42}")!;
                Assert.Equal(
                    expected: "Mario",
                    actual: deserializedPerson.GetValue().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 42,
                    actual: deserializedPerson.GetValue().OnValue(x => x.Age));

                var deserializedTypeWithConverter = JsonSerializer.Deserialize<Result<Person, TypeWithConverter>>("{\"fullName\":\"Alice X\",\"age\":28}")!;
                Assert.True(deserializedTypeWithConverter.IsSuccessful);

                Assert.False(
                    deserializedTypeWithConverter.GetValue().OnValue(x => x.Name).HasValue);

                Assert.Equal(
                    expected: (int)default,
                    actual: deserializedTypeWithConverter.GetValue().OnValue(x => x.Age));
            }
            { // This will try to convert with the left type, and then convert to the right type if it fails
                var deserializedAddress = JsonSerializer.Deserialize<Result<TypeWithConverter, Person>>("{\"fullName\":\"Alice X\",\"age\":28}")!;
                Assert.Equal(
                    expected: "Alice",
                    actual: deserializedAddress.GetValue().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 28,
                    actual: deserializedAddress.GetValue().OnValue(x => x.Age));

                var deserializedPerson = JsonSerializer.Deserialize<Result<TypeWithConverter, Person>>("{\"Name\":\"Mario\",\"Age\":42}")!;
                Assert.Equal(
                    expected: "Mario",
                    actual: deserializedPerson.GetError().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 42,
                    actual: deserializedPerson.GetError().OnValue(x => x.Age));
            }
        }

        [Fact]
        public void FromJsonBothRecordTypesWithCustomDeserializer()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new OverrideResultConverter<Person, Address>(
                deserializeAsValue: json => json.TryGetProperty("Name", out JsonElement _)));
            options.Converters.Add(new OverrideResultConverter<Person, TypeWithConverter>(
                deserializeAsValue: json => json.TryGetProperty("Name", out JsonElement _)));
            options.Converters.Add(new OverrideResultConverter<TypeWithConverter, Person>(
                deserializeAsValue: json => json.TryGetProperty("fullName", out JsonElement _)));

            {
                var deserializedPerson = JsonSerializer.Deserialize<Result<Person, Address>>("{\"Name\":\"Mario\",\"Age\":42}", options)!;
                Assert.Equal(
                    expected: "Mario",
                    actual: deserializedPerson.GetValue().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 42,
                    actual: deserializedPerson.GetValue().OnValue(x => x.Age));

                var deserializedAddress = JsonSerializer.Deserialize<Result<Person, Address>>("{\"Street\":\"123 Main St\",\"Zip\":\"12345\"}", options)!;
                Assert.Equal(
                    expected: "123 Main St",
                    actual: deserializedAddress.GetError().OnValue(x => x.Street));

                Assert.Equal(
                    expected: "12345",
                    actual: deserializedAddress.GetError().OnValue(x => x.Zip));
            }
            {
                var deserializedPerson = JsonSerializer.Deserialize<Result<Person, TypeWithConverter>>("{\"Name\":\"Mario\",\"Age\":42}", options)!;
                Assert.Equal(
                    expected: "Mario",
                    actual: deserializedPerson.GetValue().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 42,
                    actual: deserializedPerson.GetValue().OnValue(x => x.Age));

                var deserializedAddress = JsonSerializer.Deserialize<Result<TypeWithConverter, Person>>("{\"fullName\":\"Alice X\",\"age\":28}", options)!;
                Assert.Equal(
                    expected: "Alice",
                    actual: deserializedAddress.GetValue().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 28,
                    actual: deserializedAddress.GetValue().OnValue(x => x.Age));
            }
        }
    }
}
