// ------------------------------------------------------------------------------
// <copyright file="MapInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;

using War3Net.Build.Common;

namespace War3Net.Build.Info
{
    public sealed partial class MapInfo
    {
        public const string FileName = "war3map.w3i";

        private bool _skipData;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapInfo"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapInfo(MapInfoFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public MapInfoFormatVersion FormatVersion { get; set; }

        public int MapVersion { get; set; }

        public EditorVersion EditorVersion { get; set; }

        public Version? GameVersion { get; set; }

        public string? MapName { get; set; }

        public string? MapAuthor { get; set; }

        public string? MapDescription { get; set; }

        public string? RecommendedPlayers { get; set; }

        public float Unk1 { get; set; }

        public int Unk2 { get; set; }

        public float Unk3 { get; set; }

        public float Unk4 { get; set; }

        public float Unk5 { get; set; }

        public int Unk6 { get; set; }

        public Quadrilateral CameraBounds { get; set; }

        public RectangleMargins? CameraBoundsComplements { get; set; }

        // Equal to entire map width minus cameraBoundsComplement[0] and [1]
        public int PlayableMapAreaWidth { get; set; }

        // Equal to entire map minus height cameraBoundsComplement[2] and [3]
        public int PlayableMapAreaHeight { get; set; }

        public int Unk7 { get; set; }

        public MapFlags MapFlags { get; set; }

        public Tileset Tileset { get; set; }

        // RoC
        public int CampaignBackgroundNumber { get; set; }

        public int LoadingScreenBackgroundNumber { get; set; }

        public string? LoadingScreenPath { get; set; }

        public string? LoadingScreenText { get; set; }

        public string? LoadingScreenTitle { get; set; }

        public string? LoadingScreenSubtitle { get; set; }

        // RoC
        public int LoadingScreenNumber { get; set; }

        public GameDataSet GameDataSet { get; set; }

        public string? PrologueScreenPath { get; set; }

        public string? PrologueScreenText { get; set; }

        public string? PrologueScreenTitle { get; set; }

        public string? PrologueScreenSubtitle { get; set; }

        public FogStyle FogStyle { get; set; }

        public float FogStartZ { get; set; }

        public float FogEndZ { get; set; }

        public float FogDensity { get; set; }

        public Color FogColor { get; set; }

        public WeatherType GlobalWeather { get; set; }

        public string? SoundEnvironment { get; set; }

        public Tileset LightEnvironment { get; set; }

        public Color WaterTintingColor { get; set; }

        // Lua (1.31)
        public ScriptLanguage ScriptLanguage { get; set; }

        // Reforged (1.32)
        public SupportedModes SupportedModes { get; set; }

        // Reforged (1.32)
        public GameDataVersion GameDataVersion { get; set; }

        public List<PlayerData> Players { get; init; } = new();

        public List<ForceData> Forces { get; init; } = new();

        public List<UpgradeData> UpgradeData { get; init; } = new();

        public List<TechData> TechData { get; init; } = new();

        public List<RandomUnitTable>? RandomUnitTables { get; init; } = new();

        public List<RandomItemTable>? RandomItemTables { get; init; } = new();

        public override string ToString() => FileName;
    }
}