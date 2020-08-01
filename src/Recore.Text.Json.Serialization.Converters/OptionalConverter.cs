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
            var optionalConverter = Activator.CreateInstance(
                type: typeof(OptionalConverter<>).MakeGenericType(new[] { innerType }),
                args: new[] { innerConverter });

            return (JsonConverter)optionalConverter;
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
            if (reader.TokenType == JsonTokenType.Null)
            {
                return Optional<T>.Empty;
            }
            else
            {
                if (innerConverter != null)
                {
                    var innerValue = innerConverter.Read(ref reader, typeof(T), options);
                    return Optional.Of(innerValue);
                }
                else
                {
                    var innerValue = JsonSerializer.Deserialize<T>(ref reader, options);
                    return Optional.Of(innerValue);
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
            => value.Switch(
                innerValue =>
                {
                    if (innerConverter != null)
                    {
                        innerConverter.Write(writer, innerValue, options);
                    }
                    else
                    {
                        JsonSerializer.Serialize(writer, innerValue, options);
                    }
                },
                () => writer.WriteNullValue());
    }
}
