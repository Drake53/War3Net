// ------------------------------------------------------------------------------
// <copyright file="MapDoodads.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Extensions;
using War3Net.Build.Serialization.Json;
using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    [JsonConverter(typeof(JsonMapDoodadsConverter))]
    public sealed partial class MapDoodads
    {
        internal MapDoodads(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapDoodads(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<MapWidgetsFormatVersion>(nameof(FormatVersion));
            SubVersion = jsonElement.GetInt32<MapWidgetsSubVersion>(nameof(SubVersion));
            UseNewFormat = jsonElement.GetBoolean(nameof(UseNewFormat));

            foreach (var element in jsonElement.EnumerateArray(nameof(Doodads)))
            {
                Doodads.Add(element.GetMapDoodadData(FormatVersion, SubVersion, UseNewFormat));
            }

            SpecialDoodadVersion = jsonElement.GetInt32<SpecialDoodadVersion>(nameof(SpecialDoodadVersion));

            foreach (var element in jsonElement.EnumerateArray(nameof(SpecialDoodads)))
            {
                SpecialDoodads.Add(element.GetMapSpecialDoodadData(FormatVersion, SubVersion, SpecialDoodadVersion));
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
            writer.WriteObject(nameof(SubVersion), SubVersion, options);
            writer.WriteBoolean(nameof(UseNewFormat), UseNewFormat);

            writer.WriteStartArray(nameof(Doodads));
            foreach (var doodad in Doodads)
            {
                writer.Write(doodad, options, FormatVersion, SubVersion, UseNewFormat);
            }

            writer.WriteEndArray();

            writer.WriteObject(nameof(SpecialDoodadVersion), SpecialDoodadVersion, options);

            writer.WriteStartArray(nameof(SpecialDoodads));
            foreach (var specialDoodad in SpecialDoodads)
            {
                writer.Write(specialDoodad, options, FormatVersion, SubVersion, SpecialDoodadVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}