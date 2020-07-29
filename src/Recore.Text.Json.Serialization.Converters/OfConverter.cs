using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Recore.Text.Json.Serialization.Converters
{
    internal class OfConverter : JsonConverterFactory
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

    internal class OfConverter<T> : JsonConverter<Of<T>>
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
}
