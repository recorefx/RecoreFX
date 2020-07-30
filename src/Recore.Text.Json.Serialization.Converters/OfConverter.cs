using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Recore.Text.Json.Serialization.Converters
{
    /// <summary>
    /// JSON converter factory for the open generic type <seealso cref="Of{T}"/>.
    /// </summary>
    internal sealed class OfConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => IsOfType(typeToConvert) || IsOfSubtype(typeToConvert);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (IsOfType(typeToConvert))
            {
                var innerType = typeToConvert.GetGenericArguments()[0];
                var innerConverter = options.GetConverter(innerType);
                var ofConverter = Activator.CreateInstance(
                    type: typeof(OfConverter<>).MakeGenericType(new[] { innerType }),
                    args: new[] { innerConverter });

                return (JsonConverter)ofConverter;
            }
            else
            {
                var ofType = GetTypeHierarchy(typeToConvert).First(IsOfType);
                return CreateConverter(ofType, options);
            }
        }

        private static bool IsOfType(Type type)
            => type.IsGenericType
            && type.GetGenericTypeDefinition() == typeof(Of<>);

        private static bool IsOfSubtype(Type type)
            => GetTypeHierarchy(type).Any(IsOfType);

        private static IEnumerable<Type> GetTypeHierarchy(Type type)
        {
            var baseType = type.BaseType;
            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.BaseType;
            }
        }
    }

    /// <summary>
    /// JSON converter for the generic type <seealso cref="Of{T}"/>
    /// for a given <typeparamref name="T"/>.
    /// </summary>
    internal sealed class OfConverter<T> : JsonConverter<Of<T>>
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

    /// <summary>
    /// JSON converter for subtypes of <seealso cref="Of{T}"/>.
    /// </summary>
    /// <remarks>
    /// Subtypes can use this instead of calling <seealso cref="OfConverter"/> directly
    /// to get a statically-typed return value from <see cref="Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/>.
    ///
    /// Note that <c>System.Text.Json</c> requires reference types to have a parameterless constructor
    /// to be deserializable.
    /// </remarks>
    internal sealed class OfConverter<TOf, TInner> : JsonConverter<TOf> where TOf : Of<TInner>, new()
    {
        private readonly OfConverter converterFactory = new OfConverter();

        public override bool CanConvert(Type typeToConvert) => converterFactory.CanConvert(typeToConvert);

        public override TOf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var converter = (OfConverter<TInner>)converterFactory.CreateConverter(typeToConvert, options);
            var result = converter.Read(ref reader, typeToConvert, options);
            return result.To<TOf>();
        }

        public override void Write(Utf8JsonWriter writer, TOf value, JsonSerializerOptions options)
        {
            var converter = (OfConverter<TInner>)converterFactory.CreateConverter(typeof(TOf), options);
            converter.Write(writer, value, options);
        }
    }
}
