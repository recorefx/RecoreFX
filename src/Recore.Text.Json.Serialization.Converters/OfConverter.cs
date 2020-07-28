using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Recore.Text.Json.Serialization.Converters
{
    internal class OfConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
           => typeToConvert.IsGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(Of<>);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var innerType = typeToConvert.GetGenericArguments()[0];
            var innerConverter = options.GetConverter(innerType);
            var ofConverter = Activator.CreateInstance(
                type: typeof(OfConverter<>).MakeGenericType(new[] { innerType }),
                args: new[] { innerConverter });

            return (JsonConverter)ofConverter;
        }
    }

    internal class OfConverter<T> : JsonConverter<Of<T>>
    {
        private class JsonOf : Of<T> { }

        private readonly JsonConverter<T> innerConverter;

        public OfConverter(JsonConverter innerConverter)
        {
            this.innerConverter = (JsonConverter<T>)innerConverter;
        }

        public override Of<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = innerConverter.Read(ref reader, typeof(T), options);
            return new JsonOf { Value = value };
        }

        public override void Write(Utf8JsonWriter writer, Of<T> value, JsonSerializerOptions options)
            => innerConverter.Write(writer, value.Value, options);
    }
}
