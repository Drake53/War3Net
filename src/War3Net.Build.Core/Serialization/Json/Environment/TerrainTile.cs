// ------------------------------------------------------------------------------
// <copyright file="TerrainTile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class TerrainTile
    {
        internal TerrainTile(JsonElement jsonElement, MapEnvironmentFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal TerrainTile(ref Utf8JsonReader reader, MapEnvironmentFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapEnvironmentFormatVersion formatVersion)
        {
            jsonElement.GetSingle(nameof(Height));
            jsonElement.GetSingle(nameof(WaterHeight));
            jsonElement.GetBoolean(nameof(IsEdgeTile));
            jsonElement.GetInt32(nameof(Texture));
            jsonElement.GetBoolean(nameof(IsRamp));
            jsonElement.GetBoolean(nameof(IsBlighted));
            jsonElement.GetBoolean(nameof(IsWater));
            jsonElement.GetBoolean(nameof(IsBoundary));
            jsonElement.GetInt32(nameof(Variation));
            jsonElement.GetInt32(nameof(CliffVariation));
            jsonElement.GetInt32(nameof(CliffLevel));
            jsonElement.GetInt32(nameof(CliffTexture));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapEnvironmentFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapEnvironmentFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Height), Height);
            writer.WriteNumber(nameof(WaterHeight), WaterHeight);
            writer.WriteBoolean(nameof(IsEdgeTile), IsEdgeTile);
            writer.WriteNumber(nameof(Texture), Texture);
            writer.WriteBoolean(nameof(IsRamp), IsRamp);
            writer.WriteBoolean(nameof(IsBlighted), IsBlighted);
            writer.WriteBoolean(nameof(IsWater), IsWater);
            writer.WriteBoolean(nameof(IsBoundary), IsBoundary);
            writer.WriteNumber(nameof(Variation), Variation);
            writer.WriteNumber(nameof(CliffVariation), CliffVariation);
            writer.WriteNumber(nameof(CliffLevel), CliffLevel);
            writer.WriteNumber(nameof(CliffTexture), CliffTexture);

            writer.WriteEndObject();
        }
    }
}