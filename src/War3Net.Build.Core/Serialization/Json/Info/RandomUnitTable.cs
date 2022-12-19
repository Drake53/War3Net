// ------------------------------------------------------------------------------
// <copyright file="RandomUnitTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class RandomUnitTable
    {
        internal RandomUnitTable(JsonElement jsonElement, MapInfoFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal RandomUnitTable(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapInfoFormatVersion formatVersion)
        {
            Index = jsonElement.GetInt32(nameof(Index));
            Name = jsonElement.GetString(nameof(Name));

            foreach (var element in jsonElement.EnumerateArray(nameof(Types)))
            {
                Types.Add(element.GetInt32<WidgetType>());
            }

            var typeCount = Types.Count;
            foreach (var element in jsonElement.EnumerateArray(nameof(UnitSets)))
            {
                UnitSets.Add(element.GetRandomUnitSet(formatVersion, typeCount));
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapInfoFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Index), Index);
            writer.WriteString(nameof(Name), Name);

            writer.WriteObject(nameof(Types), Types, options);

            writer.WriteStartArray(nameof(UnitSets));
            foreach (var unitSet in UnitSets)
            {
                writer.Write(unitSet, options, formatVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}