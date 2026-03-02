namespace War3Net.Build.Configuration
{
    public sealed partial class GameConfiguration
    {
        internal GameConfiguration(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal GameConfiguration(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<GameConfigurationFormatVersion>(nameof(FormatVersion));
            Flags = jsonElement.GetInt32<GameConfigurationFlags>(nameof(Flags));
            GameSpeedMultiplier = jsonElement.GetUInt32(nameof(GameSpeedMultiplier));
            MapPath = jsonElement.GetString(nameof(MapPath));

            foreach (var element in jsonElement.EnumerateArray(nameof(PlayerInfo)))
            {
                PlayerInfo.Add(element.GetGameConfigurationPlayerInfo(FormatVersion));
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
            writer.WriteObject(nameof(Flags), Flags, options);
            writer.WriteNumber(nameof(GameSpeedMultiplier), GameSpeedMultiplier);
            writer.WriteString(nameof(MapPath), MapPath);

            writer.WriteStartArray(nameof(PlayerInfo));
            foreach (var playerInfo in PlayerInfo)
            {
                writer.Write(playerInfo, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}