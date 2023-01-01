// ------------------------------------------------------------------------------
// <copyright file="MapUnits.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class MapUnits
    {
        internal MapUnits(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapUnits(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<MapWidgetsFormatVersion>(nameof(FormatVersion));
            SubVersion = jsonElement.GetInt32<MapWidgetsSubVersion>(nameof(SubVersion));
            UseNewFormat = jsonElement.GetBoolean(nameof(UseNewFormat));

            foreach (var element in jsonElement.EnumerateArray(nameof(Units)))
            {
                Units.Add(element.GetMapUnitData(FormatVersion, SubVersion, UseNewFormat));
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

            writer.WriteStartArray(nameof(Units));
            foreach (var unit in Units)
            {
                writer.Write(unit, options, FormatVersion, SubVersion, UseNewFormat);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}