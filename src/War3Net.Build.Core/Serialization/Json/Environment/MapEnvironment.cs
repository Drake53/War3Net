// ------------------------------------------------------------------------------
// <copyright file="MapEnvironment.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Build.Serialization.Json;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    [JsonConverter(typeof(JsonMapEnvironmentConverter))]
    public sealed partial class MapEnvironment
    {
        internal MapEnvironment(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapEnvironment(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<MapEnvironmentFormatVersion>(nameof(FormatVersion));
            Tileset = jsonElement.GetByte<Tileset>(nameof(Tileset));
            IsCustomTileset = jsonElement.GetBoolean(nameof(IsCustomTileset));

            foreach (var element in jsonElement.EnumerateArray(nameof(TerrainTypes)))
            {
                TerrainTypes.Add(element.GetInt32<TerrainType>());
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(CliffTypes)))
            {
                CliffTypes.Add(element.GetInt32<CliffType>());
            }

            Width = jsonElement.GetUInt32(nameof(Width)) - 1;
            Height = jsonElement.GetUInt32(nameof(Height)) - 1;
            Left = jsonElement.GetSingle(nameof(Left));
            Bottom = jsonElement.GetSingle(nameof(Bottom));

            foreach (var element in jsonElement.EnumerateArray(nameof(TerrainTiles)))
            {
                TerrainTiles.Add(element.GetTerrainTile(FormatVersion));
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
            writer.WriteObject(nameof(Tileset), Tileset, options);
            writer.WriteBoolean(nameof(IsCustomTileset), IsCustomTileset);

            writer.WriteObject(nameof(TerrainTypes), TerrainTypes, options);
            writer.WriteObject(nameof(CliffTypes), CliffTypes, options);

            writer.WriteNumber(nameof(Width), Width + 1);
            writer.WriteNumber(nameof(Height), Height + 1);
            writer.WriteNumber(nameof(Left), Left);
            writer.WriteNumber(nameof(Bottom), Bottom);

            writer.WriteStartArray(nameof(TerrainTiles));
            foreach (var terrainTile in TerrainTiles)
            {
                writer.Write(terrainTile, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}