// ------------------------------------------------------------------------------
// <copyright file="JsonMapRegionsConverter.cs" company="Drake53">
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
    internal sealed class JsonMapRegionsConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapRegions);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapRegions>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapRegions? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapRegions();
            }

            public override void Write(Utf8JsonWriter writer, MapRegions value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}