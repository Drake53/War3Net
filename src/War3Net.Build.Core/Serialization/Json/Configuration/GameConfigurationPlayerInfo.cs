// ------------------------------------------------------------------------------
// <copyright file="GameConfigurationPlayerInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Configuration
{
    public sealed partial class GameConfigurationPlayerInfo
    {
        internal GameConfigurationPlayerInfo(JsonElement jsonElement, GameConfigurationFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal GameConfigurationPlayerInfo(ref Utf8JsonReader reader, GameConfigurationFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, GameConfigurationFormatVersion formatVersion)
        {
            PlayerSlotId = jsonElement.GetInt32(nameof(PlayerSlotId));
            ForceId = jsonElement.GetInt32(nameof(ForceId));
            PlayerRace = jsonElement.GetInt32<GameConfigurationPlayerRace>(nameof(PlayerRace));
            PlayerColor = jsonElement.GetInt32<KnownPlayerColor>(nameof(PlayerColor));
            Handicap = jsonElement.GetInt32(nameof(Handicap));
            PlayerInfoFlags = jsonElement.GetInt32<GameConfigurationPlayerInfoFlags>(nameof(PlayerInfoFlags));
            AIDifficulty = jsonElement.GetInt32<AIDifficulty>(nameof(AIDifficulty));
            CustomAIFilePath = jsonElement.GetString(nameof(CustomAIFilePath));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, GameConfigurationFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, GameConfigurationFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(PlayerSlotId), PlayerSlotId);
            writer.WriteNumber(nameof(ForceId), ForceId);
            writer.WriteObject(nameof(PlayerRace), PlayerRace, options);
            writer.WriteObject(nameof(PlayerColor), PlayerColor, options);
            writer.WriteNumber(nameof(Handicap), Handicap);
            writer.WriteObject(nameof(PlayerInfoFlags), PlayerInfoFlags, options);
            writer.WriteObject(nameof(AIDifficulty), AIDifficulty, options);
            writer.WriteString(nameof(CustomAIFilePath), CustomAIFilePath);

            writer.WriteEndObject();
        }
    }
}