using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Recore.Text.Json.Serialization.Converters.Tests
{
    // Shared types for testing JSON serialization

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Address
    {
        public string Street { get; set; }
        public string Zip { get; set; }
    }

    [JsonConverter(typeof(TypeWithConverterConverter))]
    class TypeWithConverter
    {
        public int Age { get; set; }

        public string Name { get; set; }

        public string GetFullName() => $"{Name} {Name}son";
    }

    // Serialize as { "fullName": GetFullName(), "age": Age }
    class TypeWithConverterConverter : JsonConverter<TypeWithConverter>
    {
        public override TypeWithConverter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.Read();
            var propertyName = reader.GetString();
            if (propertyName != "fullName")
            {
                throw new JsonException($"Unexpected JSON token: {propertyName})");
            }

            reader.Read();
            var fullName = reader.GetString();

            reader.Read();
            propertyName = reader.GetString();
            if (propertyName != "age")
            {
                throw new JsonException($"Unexpected JSON token: {propertyName})");
            }

            reader.Read();
            var age = reader.GetInt32();

            // Invert the type's methods to get the original values
            var name = fullName.Split()[0];

            // Make sure you read to the end, or `JsonSerializer` will throw an exception
            reader.Read();
            return new TypeWithConverter
            {
                Name = name,
                Age = age
            };
        }

        public override void Write(Utf8JsonWriter writer, TypeWithConverter value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("fullName", value.GetFullName());
            writer.WriteNumber("age", value.Age);
            writer.WriteEndObject();
        }
    }
}
