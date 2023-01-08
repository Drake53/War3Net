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
            Height = jsonElement.GetSingle(nameof(Height));
            WaterHeight = jsonElement.GetSingle(nameof(WaterHeight));
            IsEdgeTile = jsonElement.GetBoolean(nameof(IsEdgeTile));
            Texture = jsonElement.GetInt32(nameof(Texture));
            IsRamp = jsonElement.GetBoolean(nameof(IsRamp));
            IsBlighted = jsonElement.GetBoolean(nameof(IsBlighted));
            IsWater = jsonElement.GetBoolean(nameof(IsWater));
            IsBoundary = jsonElement.GetBoolean(nameof(IsBoundary));
            Variation = jsonElement.GetInt32(nameof(Variation));
            CliffVariation = jsonElement.GetInt32(nameof(CliffVariation));
            CliffLevel = jsonElement.GetInt32(nameof(CliffLevel));
            CliffTexture = jsonElement.GetInt32(nameof(CliffTexture));
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