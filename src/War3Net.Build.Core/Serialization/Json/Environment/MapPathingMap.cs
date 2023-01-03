// ------------------------------------------------------------------------------
// <copyright file="MapPathingMap.cs" company="Drake53">
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
    [JsonConverter(typeof(JsonMapPathingMapConverter))]
    public sealed partial class MapPathingMap
    {
        internal MapPathingMap(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapPathingMap(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<MapPathingMapFormatVersion>(nameof(FormatVersion));

            // Width and height should be four times the size in .w3e file (since MapTile is 128x128, and cells in PathingMap are 32x32).
            Width = jsonElement.GetUInt32(nameof(Width));
            Height = jsonElement.GetUInt32(nameof(Height));

            foreach (var element in jsonElement.EnumerateArray(nameof(Cells)))
            {
                Cells.Add(element.GetByte<PathingType>());
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
            writer.WriteNumber(nameof(Width), Width);
            writer.WriteNumber(nameof(Height), Height);
            writer.WriteObject(nameof(Cells), Cells, options);

            writer.WriteEndObject();
        }
    }
}