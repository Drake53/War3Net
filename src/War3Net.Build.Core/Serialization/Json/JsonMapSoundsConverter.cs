// ------------------------------------------------------------------------------
// <copyright file="JsonMapSoundsConverter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Audio;
using War3Net.Build.Extensions;

namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonMapSoundsConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapSounds);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapSounds>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapSounds? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapSounds();
            }

            public override void Write(Utf8JsonWriter writer, MapSounds value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}