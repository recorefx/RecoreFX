using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Recore.Text.Json.Serialization.Converters
{
    /// <summary>
    /// JSON converter factory for the open generic type <seealso cref="Optional{T}"/>.
    /// </summary>
    internal sealed class OptionalConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var innerType = typeToConvert.GetGenericArguments()[0];
            var innerConverter = options.GetConverter(innerType);
            var ofConverter = Activator.CreateInstance(
                type: typeof(OptionalConverter<>).MakeGenericType(new[] { innerType }),
                args: new[] { innerConverter });

            return (JsonConverter)ofConverter;
        }
    }

    /// <summary>
    /// JSON converter for the generic type <seealso cref="Optional{T}"/>
    /// for a given <typeparamref name="T"/>.
    /// </summary>
    internal sealed class OptionalConverter<T> : JsonConverter<Optional<T>>
    {
        private readonly JsonConverter<T> innerConverter;

        public OptionalConverter(JsonConverter innerConverter)
        {
            this.innerConverter = (JsonConverter<T>)innerConverter;
        }

        public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = innerConverter.Read(ref reader, typeof(T), options);
            return Optional.Of(value);
        }

        public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
            => value.Switch(
                x => innerConverter.Write(writer, x, options),
                () => writer.WriteNullValue());
    }
}
