// ------------------------------------------------------------------------------
// <copyright file="MapInfo.cs" company="Drake53">
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

namespace War3Net.Build.Info
{
    [JsonConverter(typeof(JsonMapInfoConverter))]
    public sealed partial class MapInfo
    {
        internal MapInfo(JsonElement jsonElement)
        {
            GetFrom(jsonElement);
        }

        internal MapInfo(ref Utf8JsonReader reader)
        {
            ReadFrom(ref reader);
        }

        internal void GetFrom(JsonElement jsonElement)
        {
            FormatVersion = jsonElement.GetInt32<MapInfoFormatVersion>(nameof(FormatVersion));
            if (FormatVersion >= MapInfoFormatVersion.v18)
            {
                MapVersion = jsonElement.GetInt32(nameof(MapVersion));
                EditorVersion = jsonElement.GetInt32<EditorVersion>(nameof(EditorVersion));

                if (FormatVersion >= MapInfoFormatVersion.v27)
                {
                    GameVersion = jsonElement.GetVersion(nameof(GameVersion));
                }
            }

            MapName = jsonElement.GetString(nameof(MapName));
            MapAuthor = jsonElement.GetString(nameof(MapAuthor));
            MapDescription = jsonElement.GetString(nameof(MapDescription));
            RecommendedPlayers = jsonElement.GetString(nameof(RecommendedPlayers));

            if (FormatVersion == MapInfoFormatVersion.v8)
            {
                Unk1 = jsonElement.GetSingle(nameof(Unk1));
                Unk2 = jsonElement.GetInt32(nameof(Unk2));
                Unk3 = jsonElement.GetSingle(nameof(Unk3));
                Unk4 = jsonElement.GetSingle(nameof(Unk4));
                Unk5 = jsonElement.GetSingle(nameof(Unk5));
                Unk6 = jsonElement.GetInt32(nameof(Unk6));
            }

            CameraBounds = jsonElement.GetQuadrilateral(nameof(CameraBounds));
            if (FormatVersion >= MapInfoFormatVersion.v15)
            {
                CameraBoundsComplements = jsonElement.GetRectangleMargins(nameof(CameraBoundsComplements));
            }

            PlayableMapAreaWidth = jsonElement.GetInt32(nameof(PlayableMapAreaWidth));
            PlayableMapAreaHeight = jsonElement.GetInt32(nameof(PlayableMapAreaHeight));

            if (FormatVersion == MapInfoFormatVersion.v8)
            {
                Unk7 = jsonElement.GetInt32(nameof(Unk7));
            }

            MapFlags = jsonElement.GetInt32<MapFlags>(nameof(MapFlags));
            Tileset = jsonElement.GetByte<Tileset>(nameof(Tileset));

            if (FormatVersion >= MapInfoFormatVersion.v23)
            {
                LoadingScreenBackgroundNumber = jsonElement.GetInt32(nameof(LoadingScreenBackgroundNumber));
                LoadingScreenPath = jsonElement.GetString(nameof(LoadingScreenPath));
            }
            else if (FormatVersion >= MapInfoFormatVersion.v18)
            {
                CampaignBackgroundNumber = jsonElement.GetInt32(nameof(CampaignBackgroundNumber));
            }
            else if (FormatVersion >= MapInfoFormatVersion.v15)
            {
                LoadingScreenPath = jsonElement.GetString(nameof(LoadingScreenPath));
            }

            if (FormatVersion >= MapInfoFormatVersion.v10)
            {
                LoadingScreenText = jsonElement.GetString(nameof(LoadingScreenText));
                LoadingScreenTitle = jsonElement.GetString(nameof(LoadingScreenTitle));
                if (FormatVersion >= MapInfoFormatVersion.v15)
                {
                    LoadingScreenSubtitle = jsonElement.GetString(nameof(LoadingScreenSubtitle));
                }

                if (FormatVersion >= MapInfoFormatVersion.v23)
                {
                    GameDataSet = jsonElement.GetInt32<GameDataSet>(nameof(GameDataSet));
                    PrologueScreenPath = jsonElement.GetString(nameof(PrologueScreenPath));
                }
                else if (FormatVersion >= MapInfoFormatVersion.v18)
                {
                    LoadingScreenNumber = jsonElement.GetInt32(nameof(LoadingScreenNumber));
                }
                else if (FormatVersion >= MapInfoFormatVersion.v15)
                {
                    PrologueScreenPath = jsonElement.GetString(nameof(PrologueScreenPath));
                }

                if (FormatVersion >= MapInfoFormatVersion.v11)
                {
                    PrologueScreenText = jsonElement.GetString(nameof(PrologueScreenText));
                    PrologueScreenTitle = jsonElement.GetString(nameof(PrologueScreenTitle));
                    if (FormatVersion >= MapInfoFormatVersion.v15)
                    {
                        PrologueScreenSubtitle = jsonElement.GetString(nameof(PrologueScreenSubtitle));
                    }
                }

                if (FormatVersion >= MapInfoFormatVersion.v23)
                {
                    FogStyle = jsonElement.GetInt32<FogStyle>(nameof(FogStyle));
                    FogStartZ = jsonElement.GetSingle(nameof(FogStartZ));
                    FogEndZ = jsonElement.GetSingle(nameof(FogEndZ));
                    FogDensity = jsonElement.GetSingle(nameof(FogDensity));
                    FogColor = jsonElement.GetColor(nameof(FogColor));

                    if (FormatVersion >= MapInfoFormatVersion.v25)
                    {
                        GlobalWeather = jsonElement.GetInt32<WeatherType>(nameof(GlobalWeather));
                    }

                    SoundEnvironment = jsonElement.GetString(nameof(SoundEnvironment));
                    LightEnvironment = jsonElement.GetByte<Tileset>(nameof(LightEnvironment));
                    WaterTintingColor = jsonElement.GetColor(nameof(WaterTintingColor));
                }

                if (FormatVersion >= MapInfoFormatVersion.v28)
                {
                    ScriptLanguage = jsonElement.GetInt32<ScriptLanguage>(nameof(ScriptLanguage));
                }

                if (FormatVersion >= MapInfoFormatVersion.v31)
                {
                    SupportedModes = jsonElement.GetInt32<SupportedModes>(nameof(SupportedModes));
                    GameDataVersion = jsonElement.GetInt32<GameDataVersion>(nameof(GameDataVersion));
                }
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(Players)))
            {
                Players.Add(element.GetPlayerData(FormatVersion));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(Forces)))
            {
                Forces.Add(element.GetForceData(FormatVersion));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(UpgradeData)))
            {
                UpgradeData.Add(element.GetUpgradeData(FormatVersion));
            }

            foreach (var element in jsonElement.EnumerateArray(nameof(TechData)))
            {
                TechData.Add(element.GetTechData(FormatVersion));
            }

            if (FormatVersion >= MapInfoFormatVersion.v15)
            {
                foreach (var element in jsonElement.EnumerateArray(nameof(RandomUnitTables)))
                {
                    RandomUnitTables.Add(element.GetRandomUnitTable(FormatVersion));
                }
            }

            if (FormatVersion >= MapInfoFormatVersion.v24)
            {
                foreach (var element in jsonElement.EnumerateArray(nameof(RandomItemTables)))
                {
                    RandomItemTables.Add(element.GetRandomItemTable(FormatVersion));
                }
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

            if (FormatVersion >= MapInfoFormatVersion.v18)
            {
                writer.WriteNumber(nameof(MapVersion), MapVersion);
                writer.WriteObject(nameof(EditorVersion), EditorVersion, options);

                if (FormatVersion >= MapInfoFormatVersion.v27)
                {
                    writer.WriteObject(nameof(GameVersion), GameVersion, options);
                }
            }

            writer.WriteString(nameof(MapName), MapName);
            writer.WriteString(nameof(MapAuthor), MapAuthor);
            writer.WriteString(nameof(MapDescription), MapDescription);
            writer.WriteString(nameof(RecommendedPlayers), RecommendedPlayers);

            if (FormatVersion == MapInfoFormatVersion.v8)
            {
                writer.WriteNumber(nameof(Unk1), Unk1);
                writer.WriteNumber(nameof(Unk2), Unk2);
                writer.WriteNumber(nameof(Unk3), Unk3);
                writer.WriteNumber(nameof(Unk4), Unk4);
                writer.WriteNumber(nameof(Unk5), Unk5);
                writer.WriteNumber(nameof(Unk6), Unk6);
            }

            writer.Write(nameof(CameraBounds), CameraBounds, options);
            if (FormatVersion >= MapInfoFormatVersion.v15)
            {
                writer.Write(nameof(CameraBoundsComplements), CameraBoundsComplements, options);
            }

            writer.WriteNumber(nameof(PlayableMapAreaWidth), PlayableMapAreaWidth);
            writer.WriteNumber(nameof(PlayableMapAreaHeight), PlayableMapAreaHeight);

            if (FormatVersion == MapInfoFormatVersion.v8)
            {
                writer.WriteNumber(nameof(Unk7), Unk7);
            }

            writer.WriteObject(nameof(MapFlags), MapFlags, options);
            writer.WriteObject(nameof(Tileset), Tileset, options);

            if (FormatVersion >= MapInfoFormatVersion.v23)
            {
                writer.WriteNumber(nameof(LoadingScreenBackgroundNumber), LoadingScreenBackgroundNumber);
                writer.WriteString(nameof(LoadingScreenPath), LoadingScreenPath);
            }
            else if (FormatVersion >= MapInfoFormatVersion.v18)
            {
                writer.WriteNumber(nameof(CampaignBackgroundNumber), CampaignBackgroundNumber);
            }
            else if (FormatVersion >= MapInfoFormatVersion.v15)
            {
                writer.WriteString(nameof(LoadingScreenPath), LoadingScreenPath);
            }

            if (FormatVersion >= MapInfoFormatVersion.v10)
            {
                writer.WriteString(nameof(LoadingScreenText), LoadingScreenText);
                writer.WriteString(nameof(LoadingScreenTitle), LoadingScreenTitle);
                if (FormatVersion >= MapInfoFormatVersion.v15)
                {
                    writer.WriteString(nameof(LoadingScreenSubtitle), LoadingScreenSubtitle);
                }

                if (FormatVersion >= MapInfoFormatVersion.v23)
                {
                    writer.WriteObject(nameof(GameDataSet), GameDataSet, options);
                    writer.WriteString(nameof(PrologueScreenPath), PrologueScreenPath);
                }
                else if (FormatVersion >= MapInfoFormatVersion.v18)
                {
                    writer.WriteNumber(nameof(LoadingScreenNumber), LoadingScreenNumber);
                }
                else if (FormatVersion >= MapInfoFormatVersion.v15)
                {
                    writer.WriteString(nameof(PrologueScreenPath), PrologueScreenPath);
                }

                if (FormatVersion >= MapInfoFormatVersion.v11)
                {
                    writer.WriteString(nameof(PrologueScreenText), PrologueScreenText);
                    writer.WriteString(nameof(PrologueScreenTitle), PrologueScreenTitle);
                    if (FormatVersion >= MapInfoFormatVersion.v15)
                    {
                        writer.WriteString(nameof(PrologueScreenSubtitle), PrologueScreenSubtitle);
                    }
                }

                if (FormatVersion >= MapInfoFormatVersion.v23)
                {
                    writer.WriteObject(nameof(FogStyle), FogStyle, options);
                    writer.WriteNumber(nameof(FogStartZ), FogStartZ);
                    writer.WriteNumber(nameof(FogEndZ), FogEndZ);
                    writer.WriteNumber(nameof(FogDensity), FogDensity);
                    writer.Write(nameof(FogColor), FogColor);

                    if (FormatVersion >= MapInfoFormatVersion.v25)
                    {
                        writer.WriteObject(nameof(GlobalWeather), GlobalWeather, options);
                    }

                    writer.WriteString(nameof(SoundEnvironment), SoundEnvironment);
                    writer.WriteObject(nameof(LightEnvironment), LightEnvironment, options);

                    writer.Write(nameof(WaterTintingColor), WaterTintingColor);
                }

                if (FormatVersion >= MapInfoFormatVersion.v28)
                {
                    writer.WriteObject(nameof(ScriptLanguage), ScriptLanguage, options);
                }

                if (FormatVersion >= MapInfoFormatVersion.v31)
                {
                    writer.WriteObject(nameof(SupportedModes), SupportedModes, options);
                    writer.WriteObject(nameof(GameDataVersion), GameDataVersion, options);
                }
            }

            writer.WriteStartArray(nameof(Players));
            foreach (var player in Players)
            {
                writer.Write(player, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteStartArray(nameof(Forces));
            foreach (var force in Forces)
            {
                writer.Write(force, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteStartArray(nameof(UpgradeData));
            foreach (var upgrade in UpgradeData)
            {
                writer.Write(upgrade, options, FormatVersion);
            }

            writer.WriteEndArray();

            writer.WriteStartArray(nameof(TechData));
            foreach (var tech in TechData)
            {
                writer.Write(tech, options, FormatVersion);
            }

            writer.WriteEndArray();

            if (FormatVersion >= MapInfoFormatVersion.v15)
            {
                if (RandomUnitTables is null)
                {
                    writer.WriteNull(nameof(RandomUnitTables));
                }
                else
                {
                    writer.WriteStartArray(nameof(RandomUnitTables));
                    foreach (var unitTable in RandomUnitTables)
                    {
                        writer.Write(unitTable, options, FormatVersion);
                    }

                    writer.WriteEndArray();
                }
            }

            if (FormatVersion >= MapInfoFormatVersion.v24)
            {
                if (RandomItemTables is null)
                {
                    writer.WriteNull(nameof(RandomItemTables));
                }
                else
                {
                    writer.WriteStartArray(nameof(RandomItemTables));
                    foreach (var itemTable in RandomItemTables)
                    {
                        writer.Write(itemTable, options, FormatVersion);
                    }

                    writer.WriteEndArray();
                }
            }

            writer.WriteEndObject();
        }
    }
}