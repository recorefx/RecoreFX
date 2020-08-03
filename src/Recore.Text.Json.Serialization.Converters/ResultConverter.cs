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
            // Using try-catch for control flow is an antipattern,
            // but it seems to be the only way in this case.
            try
            {
                if (valueConverter != null)
                {
                    return valueConverter.Read(ref reader, typeToConvert, options);
                }
                else
                {
                    return JsonSerializer.Deserialize<TValue>(ref reader, options);
                }
            }
            catch (InvalidOperationException)
            {
                if (errorConverter != null)
                {
                    return errorConverter.Read(ref reader, typeToConvert, options);
                }
                else
                {
                    return JsonSerializer.Deserialize<TError>(ref reader, options);
                }
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
    /// <example>
    /// The default deserialization behavior can get confused, for instance,
    /// when both <typeparamref name="TValue"/> and <typeparamref name="TError"/> are POCOs.
    /// In that case, it will always deserialize as <typeparamref name="TValue"/>,
    /// filling in default values for any missing properties.
    /// For example:
    /// 
    /// <code>
    /// &lt;code&gt;
    /// class Person
    /// {
    ///     public string Name { get; set; }
    ///     public int Age { get; set; }
    /// }
    /// 
    /// class Address
    /// {
    ///     public string Street { get; set; }
    ///     public string Zip { get; set; }
    /// }
    /// 
    /// // Deserializes as a `Person`!
    /// JsonSerializer.Deserialize&lt;Result&lt;Person, Address&gt;&gt;("{\"Street\":\"123 Main St\",\"Zip\":\"12345\"}")
    /// 
    /// // Look at the JSON to decide which type we have
    /// options.Converters.Add(new OverrideResultConverter&lt;Person, Address&gt;(
    ///     deserializeAsValue: json =&gt; json.TryGetProperty("Street", out JsonElement _)));
    /// 
    /// // Deserializes correctly
    /// JsonSerializer.Deserialize&lt;Result&lt;Person, Address&gt;&gt;("{\"Street\":\"123 Main St\",\"Zip\":\"12345\"}", options)
    /// &lt;/code&gt;
    /// </code>
    /// </example>
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
