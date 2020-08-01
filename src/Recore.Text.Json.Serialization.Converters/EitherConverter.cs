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
            this.leftConverter = (JsonConverter<TLeft>)leftConverter;
            this.rightConverter = (JsonConverter<TRight>)rightConverter;
        }

        public override Either<TLeft, TRight> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // The the thing that makes this tricky is that `JsonSerializer.Deserialize<TLeft>()`'s
            // default behavior (when no converter is defined)
            // is that it will just leave any fields null that it can't find on the object.
            // So, if you have
            //   class A { string Foo; }
            //   class B { string Bar; }
            // then
            //   JsonSerializer.Deserialize<Either<A, B>>()
            // will always return an instance of `A`, assuming we try to deserialize `A` first.
            if (leftConverter != null)
            {
                try
                {
                    return leftConverter.Read(ref reader, typeof(TLeft), options);
                }
                catch (InvalidOperationException)
                {
                }
            }
            else if (rightConverter != null)
            {
                try
                {
                    return rightConverter.Read(ref reader, typeof(TRight), options);
                }
                catch (InvalidOperationException)
                {
                }
            }

            // Neither the left nor the right type has defined a converter.
            try
            {
                return JsonSerializer.Deserialize<TLeft>(ref reader, options);
            }
            catch (InvalidOperationException)
            {
            }
            catch (JsonException)
            {
            }

            return JsonSerializer.Deserialize<TRight>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, Either<TLeft, TRight> value, JsonSerializerOptions options)
            => value.Switch(
                left =>
                {
                    if (leftConverter != null)
                    {
                        leftConverter.Write(writer, left, options);
                    }
                    else
                    {
                        JsonSerializer.Serialize(writer, left, options);
                    }
                },
                right =>
                {
                    if (rightConverter != null)
                    {
                        rightConverter.Write(writer, right, options);
                    }
                    else
                    {
                        JsonSerializer.Serialize(writer, right, options);
                    }
                });
    }
}
