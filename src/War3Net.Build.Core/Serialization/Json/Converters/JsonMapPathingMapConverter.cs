// ------------------------------------------------------------------------------
// <copyright file="JsonMapPathingMapConverter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Environment;
using War3Net.Build.Extensions;

namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonMapPathingMapConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapPathingMap);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapPathingMap>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapPathingMap? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapPathingMap();
            }

            public override void Write(Utf8JsonWriter writer, MapPathingMap value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}