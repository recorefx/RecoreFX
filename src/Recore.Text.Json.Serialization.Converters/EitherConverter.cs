using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Recore.Text.Json.Serialization.Converters
{
    /// <summary>
    /// JSON converter factory for the open generic type <seealso cref="Either{TLeft, TRight}"/>.
    /// </summary>
    internal sealed class EitherConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(Either<,>);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var leftType = typeToConvert.GetGenericArguments()[0];
            var leftConverter = options.GetConverter(leftType);

            var rightType = typeToConvert.GetGenericArguments()[1];
            var rightConverter = options.GetConverter(rightType);

            var eitherConverter = Activator.CreateInstance(
                type: typeof(EitherConverter<,>).MakeGenericType(new[] { leftType, rightType }),
                args: new[] { leftConverter, rightConverter });

            return (JsonConverter)eitherConverter;
        }
    }

    /// <summary>
    /// JSON converter for the generic type <seealso cref="Either{TLeft, TRight}"/>
    /// for given <typeparamref name="TLeft"/> and <typeparamref name="TRight"/>.
    /// </summary>
    internal sealed class EitherConverter<TLeft, TRight> : JsonConverter<Either<TLeft, TRight>>
    {
        private readonly JsonConverter<TLeft> leftConverter;
        private readonly JsonConverter<TRight> rightConverter;

        public EitherConverter(JsonConverter leftConverter, JsonConverter rightConverter)
        {
            if (leftConverter is null)
            {
                throw new ArgumentNullException(nameof(leftConverter));
            }

            if (rightConverter is null)
            {
                throw new ArgumentNullException(nameof(rightConverter));
            }

            this.leftConverter = (JsonConverter<TLeft>)leftConverter;
            this.rightConverter = (JsonConverter<TRight>)rightConverter;
        }

        public override Either<TLeft, TRight> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Using try-catch for control flow is an antipattern,
            // but it seems to be the only way in this case.
            try
            {
                return leftConverter.Read(ref reader, typeToConvert, options);
            }
            catch (InvalidOperationException)
            {
            }

            return rightConverter.Read(ref reader, typeToConvert, options);
        }

        public override void Write(Utf8JsonWriter writer, Either<TLeft, TRight> value, JsonSerializerOptions options)
            => value.Switch(
                left => leftConverter.Write(writer, left, options),
                right => rightConverter.Write(writer, right, options));
    }
}
