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
            var optionalConverter = Activator.CreateInstance(
                type: typeof(OptionalConverter<>).MakeGenericType(new[] { innerType }));

            if (optionalConverter is null)
            {
                // According to the docs, `CreateInstance` should return `null`
                // only if the type is some `Nullable<T>`
                throw new InvalidOperationException();
            }

            return (JsonConverter)optionalConverter;
        }
    }

    /// <summary>
    /// JSON converter for the generic type <seealso cref="Optional{T}"/>
    /// for a given <typeparamref name="T"/>.
    /// </summary>
    internal sealed class OptionalConverter<T> : JsonConverter<Optional<T>> where T : notnull
    {
        public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Optional<T>.Empty;
            }
            else
            {
                var innerValue = JsonSerializer.Deserialize<T>(ref reader, options)!;
                return Optional.Of(innerValue);
            }
        }

        public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
            => value.Switch(
                innerValue => JsonSerializer.Serialize(writer, innerValue, options),
                () => writer.WriteNullValue());
    }
}
