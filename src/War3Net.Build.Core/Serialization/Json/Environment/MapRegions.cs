// ------------------------------------------------------------------------------
// <copyright file="MapRegions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Extensions;
using War3Net.Build.Serialization.Json;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    [JsonConverter(typeof(JsonMapRegionsConverter))]
    public sealed partial class MapRegions
    {
        internal MapRegions(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapRegions(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<MapRegionsFormatVersion>(nameof(FormatVersion));

            var regionsElement = jsonElement.GetProperty(nameof(Regions));
            if (regionsElement.ValueKind == JsonValueKind.Null)
            {
                Protected = true;
            }
            else
            {
                foreach (var element in regionsElement.EnumerateArray())
                {
                    Regions.Add(element.GetRegion(FormatVersion));
                }
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(FormatVersion), FormatVersion, options);

            if (Protected)
            {
                writer.WriteNull(nameof(Regions));
            }
            else
            {
                writer.WriteStartArray(nameof(Regions));
                foreach (var region in Regions)
                {
                    writer.Write(region, options, FormatVersion);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }
    }
}