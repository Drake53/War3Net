// ------------------------------------------------------------------------------
// <copyright file="JsonMapShadowMapConverter.cs" company="Drake53">
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
    internal sealed class JsonMapShadowMapConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapShadowMap);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapShadowMap>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapShadowMap? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapShadowMap();
            }

            public override void Write(Utf8JsonWriter writer, MapShadowMap value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}