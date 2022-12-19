// ------------------------------------------------------------------------------
// <copyright file="RandomItemTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class RandomItemTable
    {
        internal RandomItemTable(JsonElement jsonElement, MapInfoFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal RandomItemTable(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapInfoFormatVersion formatVersion)
        {
            Index = jsonElement.GetInt32(nameof(Index));
            Name = jsonElement.GetString(nameof(Name));

            foreach (var element in jsonElement.EnumerateArray(nameof(ItemSets)))
            {
                ItemSets.Add(element.GetRandomItemSet(formatVersion));
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

            writer.WriteStartArray(nameof(ItemSets));
            foreach (var itemSet in ItemSets)
            {
                writer.Write(itemSet, options, formatVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}