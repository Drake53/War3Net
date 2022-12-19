// ------------------------------------------------------------------------------
// <copyright file="MapInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

using War3Net.Build.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class MapInfo
    {
        internal void ReadFrom(ref Utf8JsonReader reader)
        {
            throw new NotImplementedException();
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(FormatVersion), FormatVersion, options);

            if (FormatVersion >= MapInfoFormatVersion.RoC)
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
            else if (FormatVersion >= MapInfoFormatVersion.RoC)
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
                else if (FormatVersion >= MapInfoFormatVersion.RoC)
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

                    if (FormatVersion >= MapInfoFormatVersion.Tft)
                    {
                        writer.WriteObject(nameof(GlobalWeather), GlobalWeather, options);
                    }

                    writer.WriteString(nameof(SoundEnvironment), SoundEnvironment);
                    writer.WriteObject(nameof(LightEnvironment), LightEnvironment, options);

                    writer.Write(nameof(WaterTintingColor), WaterTintingColor);
                }

                if (FormatVersion >= MapInfoFormatVersion.Lua)
                {
                    writer.WriteObject(nameof(ScriptLanguage), ScriptLanguage, options);
                }

                if (FormatVersion >= MapInfoFormatVersion.Reforged)
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