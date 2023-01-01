// ------------------------------------------------------------------------------
// <copyright file="RandomUnitCustomTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class RandomUnitCustomTable : RandomUnitData
    {
        internal RandomUnitCustomTable(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            GetFrom(jsonElement, formatVersion, subVersion, useNewFormat);
        }

        internal RandomUnitCustomTable(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(ref reader, formatVersion, subVersion, useNewFormat);
        }

        internal void GetFrom(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            foreach (var element in jsonElement.EnumerateArray(nameof(RandomUnits)))
            {
                RandomUnits.Add(element.GetRandomUnitTableUnit(formatVersion, subVersion, useNewFormat));
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion, subVersion, useNewFormat);
        }

        internal override void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.WriteStartObject();

            writer.WriteStartArray(nameof(RandomUnits));
            foreach (var randomUnit in RandomUnits)
            {
                writer.Write(randomUnit, options, formatVersion, subVersion, useNewFormat);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}