using System;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

using Recore.Security.Cryptography;

namespace Recore.Text.Json.Serialization.Converters
{
    /// <summary>
    /// JSON converter factory for the open generic type <seealso cref="CiphertextConverter{THash}"/>.
    /// </summary>
    internal sealed class CiphertextConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(Ciphertext<>);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var innerType = typeToConvert.GetGenericArguments()[0];

            var converter = Activator.CreateInstance(
                type: typeof(CiphertextConverter<>).MakeGenericType(new[] { innerType }));

            if (converter is null)
            {
                // According to the docs, `CreateInstance` should return `null`
                // only if the type is some `Nullable<T>`
                throw new InvalidOperationException();
            }

            return (JsonConverter)converter;
        }
    }

    /// <summary>
    /// JSON converter for the generic type <seealso cref="CiphertextConverter{THash}"/>
    /// for a given <typeparamref name="THash"/>.
    /// </summary>
    internal sealed class CiphertextConverter<THash> : JsonConverter<Ciphertext<THash>> where THash : HashAlgorithm
    {
        public override Ciphertext<THash> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string path = reader.GetString() ?? throw new JsonException();
            return new Ciphertext<THash>(path);
        }

        public override void Write(Utf8JsonWriter writer, Ciphertext<THash> value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
