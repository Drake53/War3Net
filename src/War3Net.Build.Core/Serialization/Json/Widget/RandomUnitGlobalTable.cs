// ------------------------------------------------------------------------------
// <copyright file="RandomUnitGlobalTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed partial class RandomUnitGlobalTable : RandomUnitData
    {
        internal RandomUnitGlobalTable(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            GetFrom(jsonElement, formatVersion, subVersion, useNewFormat);
        }

        internal RandomUnitGlobalTable(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(ref reader, formatVersion, subVersion, useNewFormat);
        }

        internal void GetFrom(JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            TableId = jsonElement.GetInt32(nameof(TableId));
            Column = jsonElement.GetInt32(nameof(Column));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion, subVersion, useNewFormat);
        }

        internal override void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(TableId), TableId);
            writer.WriteNumber(nameof(Column), Column);

            writer.WriteEndObject();
        }
    }
}