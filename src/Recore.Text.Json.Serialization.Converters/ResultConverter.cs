using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Recore.Text.Json.Serialization.Converters
{
    /// <summary>
    /// JSON converter factory for the open generic type <seealso cref="Result{TValue, TError}"/>.
    /// </summary>
    internal sealed class ResultConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(Result<,>);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var valueType = typeToConvert.GetGenericArguments()[0];
            var valueConverter = options.GetConverter(valueType);

            var errorConverterType = typeToConvert.GetGenericArguments()[1];
            var errorConverter = options.GetConverter(errorConverterType);

            var resultConverter = Activator.CreateInstance(
                type: typeof(ResultConverter<,>).MakeGenericType(new[] { valueType, errorConverterType }),
                args: new[] { valueConverter, errorConverter });

            return (JsonConverter)resultConverter;
        }
    }

    /// <summary>
    /// JSON converter for the generic type <seealso cref="Result{TValue, TError}"/>
    /// for given <typeparamref name="TValue"/> and <typeparamref name="TError"/>.
    /// </summary>
    internal sealed class ResultConverter<TValue, TError> : JsonConverter<Result<TValue, TError>>
    {
        private readonly JsonConverter<TValue> valueConverter;
        private readonly JsonConverter<TError> errorConverter;

        public ResultConverter(JsonConverter valueConverter, JsonConverter errorConverter)
        {
            this.valueConverter = (JsonConverter<TValue>)valueConverter;
            this.errorConverter = (JsonConverter<TError>)errorConverter;
        }

        public override Result<TValue, TError> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read the whole string in as JSON to avoid the case where the first converter partially succeeds
            // and then the reader is stuck in the middle of the JSON.
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var json = jsonDocument.RootElement.ToString();

            // `ToString()` for a string type will return the string *without* quotes,
            // which won't deserialize correctly.
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.String)
            {
                json = $"\"{json}\"";
            }

            // Using try-catch for control flow is an antipattern,
            // but it seems to be the only way in this case.
            try
            {
                return JsonSerializer.Deserialize<TValue>(json, options);
            }
            catch (JsonException)
            {
                return JsonSerializer.Deserialize<TError>(json, options);
            }
        }

        public override void Write(Utf8JsonWriter writer, Result<TValue, TError> value, JsonSerializerOptions options)
            => value.Switch(
                val =>
                {
                    if (valueConverter != null)
                    {
                        valueConverter.Write(writer, val, options);
                    }
                    else
                    {
                        JsonSerializer.Serialize(writer, val, options);
                    }
                },
                err =>
                {
                    if (errorConverter != null)
                    {
                        errorConverter.Write(writer, err, options);
                    }
                    else
                    {
                        JsonSerializer.Serialize(writer, err, options);
                    }
                });
    }

    /// <summary>
    /// Converts <seealso cref="Result{TValue, TError}"/> to and from JSON.
    /// Register this converter to override the default deserialization behavior.
    /// </summary>
    /// <remarks>
    /// This converter is made to be used with a closed type and registered through <seealso cref="JsonSerializerOptions.Converters"/>.
    /// It is not returned by <seealso cref="ResultConverter.CreateConverter(Type, JsonSerializerOptions)"/>.
    /// </remarks>
    public sealed class OverrideResultConverter<TValue, TError> : JsonConverter<Result<TValue, TError>>
    {
        private readonly Func<JsonElement, bool> deserializeAsValue;

        /// <summary>
        /// Initializes an instance of <see cref="OverrideResultConverter{TValue, TError}"/>.
        /// </summary>
        /// <param name="deserializeAsValue">
        /// A delegate that takes a <seealso cref="JsonElement"/> representing some JSON and returns
        /// whether it should be deserialized as <typeparamref name="TValue"/> or <typeparamref name="TError"/>.
        /// It should return <c>true</c> for <typeparamref name="TValue"/> and <c>false</c> for <typeparamref name="TError"/>.
        /// </param>
        public OverrideResultConverter(
            Func<JsonElement, bool> deserializeAsValue)
        {
            this.deserializeAsValue = deserializeAsValue;
        }

        /// <summary>
        /// Deserializes JSON into <seealso cref="Result{TValue, TError}"/>.
        /// This method will call the delegate passed to the constructor to determine how to deserialize the JSON.
        /// </summary>
        public override Result<TValue, TError> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var json = jsonDocument.RootElement.ToString();

            // `ToString()` for a string type will return the string *without* quotes,
            // which won't deserialize correctly.
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.String)
            {
                json = $"\"{json}\"";
            }

            if (deserializeAsValue(jsonDocument.RootElement))
            {
                return JsonSerializer.Deserialize<TValue>(json, options);
            }
            else
            {
                return JsonSerializer.Deserialize<TError>(json, options);
            }
        }

        /// <summary>
        /// Serializes <seealso cref="Result{TValue, TError}"/> as JSON.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, Result<TValue, TError> value, JsonSerializerOptions options)
        {
            var valueConverter = options.GetConverter(typeof(TValue));
            var errorConverter = options.GetConverter(typeof(TError));

            var eitherConverter = new ResultConverter<TValue, TError>(valueConverter, errorConverter);
            eitherConverter.Write(writer, value, options);
        }
    }
}
