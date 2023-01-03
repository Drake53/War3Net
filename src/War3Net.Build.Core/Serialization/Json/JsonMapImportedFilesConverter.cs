// ------------------------------------------------------------------------------
// <copyright file="JsonMapImportedFilesConverter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Extensions;
using War3Net.Build.Import;

namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonMapImportedFilesConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MapImportedFiles);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<MapImportedFiles>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override MapImportedFiles? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadMapImportedFiles();
            }

            public override void Write(Utf8JsonWriter writer, MapImportedFiles value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}