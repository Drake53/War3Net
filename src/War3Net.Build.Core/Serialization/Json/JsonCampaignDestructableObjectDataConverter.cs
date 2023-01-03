// ------------------------------------------------------------------------------
// <copyright file="JsonCampaignDestructableObjectDataConverter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Extensions;
using War3Net.Build.Object;

namespace War3Net.Build.Serialization.Json
{
    internal sealed class JsonCampaignDestructableObjectDataConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(CampaignDestructableObjectData);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new Converter(options);
        }

        private class Converter : JsonConverter<CampaignDestructableObjectData>
        {
            public Converter(JsonSerializerOptions options)
            {
            }

            public override CampaignDestructableObjectData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.ReadCampaignDestructableObjectData();
            }

            public override void Write(Utf8JsonWriter writer, CampaignDestructableObjectData value, JsonSerializerOptions options)
            {
                writer.Write(value, options);
            }
        }
    }
}