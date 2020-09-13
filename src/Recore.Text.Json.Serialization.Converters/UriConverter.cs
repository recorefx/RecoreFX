// Adapted from https://github.com/dotnet/runtime/blob/de1f8d847ee3ac635affa5296b163691ef8612f6/src/libraries/System.Text.Json/src/System/Text/Json/Serialization/Converters/Value/UriConverter.cs

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Recore.Text.Json.Serialization.Converters
{
    internal sealed class AbsoluteUriConverter : JsonConverter<AbsoluteUri>
    {
        public override AbsoluteUri Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string uriString = reader.GetString();
            if (AbsoluteUri.TryCreate(uriString, out AbsoluteUri? value))
            {
                return value;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, AbsoluteUri value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.OriginalString);
        }
    }

    internal sealed class RelativeUriConverter : JsonConverter<RelativeUri>
    {
        public override RelativeUri Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string uriString = reader.GetString();
            if (RelativeUri.TryCreate(uriString, out RelativeUri? value))
            {
                return value;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, RelativeUri value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.OriginalString);
        }
    }
}