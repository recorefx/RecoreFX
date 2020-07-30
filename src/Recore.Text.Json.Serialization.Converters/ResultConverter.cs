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
            if (valueConverter is null)
            {
                throw new ArgumentNullException(nameof(valueConverter));
            }

            if (errorConverter is null)
            {
                throw new ArgumentNullException(nameof(errorConverter));
            }

            this.valueConverter = (JsonConverter<TValue>)valueConverter;
            this.errorConverter = (JsonConverter<TError>)errorConverter;
        }

        public override Result<TValue, TError> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Using try-catch for control flow is an antipattern,
            // but it seems to be the only way in this case.
            try
            {
                return valueConverter.Read(ref reader, typeToConvert, options);
            }
            catch (InvalidOperationException)
            {
            }

            return errorConverter.Read(ref reader, typeToConvert, options);
        }

        public override void Write(Utf8JsonWriter writer, Result<TValue, TError> value, JsonSerializerOptions options)
            => value.Switch(
                v => valueConverter.Write(writer, v, options),
                e => errorConverter.Write(writer, e, options));
    }
}
