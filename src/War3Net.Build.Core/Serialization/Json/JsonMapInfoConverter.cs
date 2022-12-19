// ------------------------------------------------------------------------------
// <copyright file="JsonMapInfoConverter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Extensions;
using War3Net.Build.Info;

namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonMapInfoConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapInfo);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapInfo>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapInfo();
            }

            public override void Write(Utf8JsonWriter writer, MapInfo value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}