﻿using System.Text.Json;
using Xunit;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    public class EitherConverterTests
    {
        [Fact]
        public void ToJson()
        {
            {
                Either<int, string> either;

                either = 12;
                Assert.Equal(
                    expected: "12",
                    actual: JsonSerializer.Serialize(either));

                either = "hello";
                Assert.Equal(
                    expected: "\"hello\"",
                    actual: JsonSerializer.Serialize(either));
            }
            {
                Either<string, int> either;

                either = 12;
                Assert.Equal(
                    expected: "12",
                    actual: JsonSerializer.Serialize(either));

                either = "hello";
                Assert.Equal(
                    expected: "\"hello\"",
                    actual: JsonSerializer.Serialize(either));
            }
            {
                Either<string, Person> either;

                either = new Person { Name = "Mario", Age = 42 };
                Assert.Equal(
                    expected: "{\"Name\":\"Mario\",\"Age\":42}",
                    actual: JsonSerializer.Serialize(either));
            }
            {
                Either<Person, string> either;

                either = new Person { Name = "Mario", Age = 42 };
                Assert.Equal(
                    expected: "{\"Name\":\"Mario\",\"Age\":42}",
                    actual: JsonSerializer.Serialize(either));
            }
        }

        [Fact]
        public void ToJsonWithSpecialConverter()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialStringConverter());

            Either<int, string> either = "hello";

            Assert.Equal(
                expected: "{\"value\":\"hello\",\"length\":5}",
                actual: JsonSerializer.Serialize(either, options));
        }

        [Fact]
        public void FromJson()
        {
            Assert.Equal(
                expected: new Either<int, string>(12),
                actual: JsonSerializer.Deserialize<Either<int, string>>("12"));

            Assert.Equal(
                expected: new Either<int, string>("hello"),
                actual: JsonSerializer.Deserialize<Either<int, string>>("\"hello\""));

            Assert.Equal(
                expected: new Either<string, int>(12),
                actual: JsonSerializer.Deserialize<Either<string, int>>("12"));

            Assert.Equal(
                expected: new Either<string, int>("hello"),
                actual: JsonSerializer.Deserialize<Either<string, int>>("\"hello\""));

            var deserializedPerson = JsonSerializer.Deserialize<Either<Person, string>>("{\"Name\":\"Mario\",\"Age\":42}")!;
            Assert.Equal(
                expected: "Mario",
                actual: deserializedPerson.GetLeft().OnValue(x => x.Name));

            Assert.Equal(
                expected: 42,
                actual: deserializedPerson.GetLeft().OnValue(x => x.Age));
        }

        [Fact]
        public void FromJsonWithSpecialConverter()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialStringConverter());

            Assert.Equal(
                expected: new Either<int, string>("hello"),
                actual: JsonSerializer.Deserialize<Either<int, string>>("{\"value\":\"hello\",\"length\":5}", options));
        }

        [Fact]
        public void FromJsonBothRecordTypesAlwaysDeserializesLeft()
        {
            {
                var deserializedPerson = JsonSerializer.Deserialize<Either<Person, Address>>("{\"Name\":\"Mario\",\"Age\":42}")!;
                Assert.Equal(
                    expected: "Mario",
                    actual: deserializedPerson.GetLeft().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 42,
                    actual: deserializedPerson.GetLeft().OnValue(x => x.Age));

                // This will deserialize as Person instead of Address because TLeft = Person.
                var deserializedAddress = JsonSerializer.Deserialize<Either<Person, Address>>("{\"Street\":\"123 Main St\",\"Zip\":\"12345\"}")!;
                Assert.True(deserializedAddress.IsLeft);

                Assert.False(
                    deserializedAddress.GetLeft().OnValue(x => x.Name).HasValue);

                Assert.Equal(
                    expected: (int)default,
                    actual: deserializedAddress.GetLeft().OnValue(x => x.Age));
            }
        }

        [Fact]
        public void FromJsonObjectTypesWithConverter()
        {
            { // This will always deserialize the left type because it is a POCO
                var deserializedPerson = JsonSerializer.Deserialize<Either<Person, TypeWithConverter>>("{\"Name\":\"Mario\",\"Age\":42}")!;
                Assert.Equal(
                    expected: "Mario",
                    actual: deserializedPerson.GetLeft().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 42,
                    actual: deserializedPerson.GetLeft().OnValue(x => x.Age));

                var deserializedTypeWithConverter = JsonSerializer.Deserialize<Either<Person, TypeWithConverter>>("{\"fullName\":\"Alice X\",\"age\":28}")!;
                Assert.True(deserializedTypeWithConverter.IsLeft);

                Assert.False(
                    deserializedTypeWithConverter.GetLeft().OnValue(x => x.Name).HasValue);

                Assert.Equal(
                    expected: (int)default,
                    actual: deserializedTypeWithConverter.GetLeft().OnValue(x => x.Age));
            }
            { // This will try to convert with the left type, and then convert to the right type if it fails
                var deserializedTypeWithConverter = JsonSerializer.Deserialize<Either<TypeWithConverter, Person>>("{\"fullName\":\"Alice X\",\"age\":28}")!;
                Assert.Equal(
                    expected: "Alice",
                    actual: deserializedTypeWithConverter.GetLeft().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 28,
                    actual: deserializedTypeWithConverter.GetLeft().OnValue(x => x.Age));

                var deserializedPerson = JsonSerializer.Deserialize<Either<TypeWithConverter, Person>>("{\"Name\":\"Mario\",\"Age\":42}")!;
                Assert.Equal(
                    expected: "Mario",
                    actual: deserializedPerson.GetRight().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 42,
                    actual: deserializedPerson.GetRight().OnValue(x => x.Age));
            }
        }

        [Fact]
        public void FromJsonBothRecordTypesWithCustomDeserializer()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new OverrideEitherConverter<Person, Address>(
                deserializeAsLeft: json => json.TryGetProperty("Name", out JsonElement _)));
            options.Converters.Add(new OverrideEitherConverter<Person, TypeWithConverter>(
                deserializeAsLeft: json => json.TryGetProperty("Name", out JsonElement _)));
            options.Converters.Add(new OverrideEitherConverter<TypeWithConverter, Person>(
                deserializeAsLeft: json => json.TryGetProperty("fullName", out JsonElement _)));

            {
                var deserializedPerson = JsonSerializer.Deserialize<Either<Person, Address>>("{\"Name\":\"Mario\",\"Age\":42}", options)!;
                Assert.Equal(
                    expected: "Mario",
                    actual: deserializedPerson.GetLeft().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 42,
                    actual: deserializedPerson.GetLeft().OnValue(x => x.Age));

                var deserializedAddress = JsonSerializer.Deserialize<Either<Person, Address>>("{\"Street\":\"123 Main St\",\"Zip\":\"12345\"}", options)!;
                Assert.Equal(
                    expected: "123 Main St",
                    actual: deserializedAddress.GetRight().OnValue(x => x.Street));

                Assert.Equal(
                    expected: "12345",
                    actual: deserializedAddress.GetRight().OnValue(x => x.Zip));
            }
            {
                var deserializedPerson = JsonSerializer.Deserialize<Either<Person, TypeWithConverter>>("{\"Name\":\"Mario\",\"Age\":42}", options)!;
                Assert.Equal(
                    expected: "Mario",
                    actual: deserializedPerson.GetLeft().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 42,
                    actual: deserializedPerson.GetLeft().OnValue(x => x.Age));

                var deserializedAddress = JsonSerializer.Deserialize<Either<TypeWithConverter, Person>>("{\"fullName\":\"Alice X\",\"age\":28}", options)!;
                Assert.Equal(
                    expected: "Alice",
                    actual: deserializedAddress.GetLeft().OnValue(x => x.Name));

                Assert.Equal(
                    expected: 28,
                    actual: deserializedAddress.GetLeft().OnValue(x => x.Age));
            }
        }
    }
}
