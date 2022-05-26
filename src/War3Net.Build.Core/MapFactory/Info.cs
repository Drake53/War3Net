// ------------------------------------------------------------------------------
// <copyright file="Info.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;

using War3Net.Build.Common;
using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Providers;

namespace War3Net.Build
{
    public static partial class MapFactory
    {
        public static MapInfo Info(
            int mapWidth = 64,
            int mapHeight = 64,
            Tileset tileset = Tileset.LordaeronSummer,
            ScriptLanguage scriptLanguage = ScriptLanguage.Lua)
        {
            if (mapWidth < 32 || mapWidth > 480 || (mapWidth % 32) != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(mapWidth));
            }

            if (mapHeight < 32 || mapHeight > 480 || (mapHeight % 32) != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(mapHeight));
            }

            if (tileset == Tileset.Unspecified || !Enum.IsDefined(tileset))
            {
                throw new InvalidEnumArgumentException(nameof(tileset), (int)tileset, typeof(Tileset));
            }

            if (!Enum.IsDefined(scriptLanguage))
            {
                throw new InvalidEnumArgumentException(nameof(scriptLanguage), (int)scriptLanguage, typeof(ScriptLanguage));
            }

            const int LeftBound = 6;
            const int RightBound = 6;
            const int BottomBound = 4;
            const int TopBound = 8;

            var horizontalOffset = (mapWidth / 2) - 4;
            var verticalOffset = (mapHeight / 2) - 2;

            var cameraBounds = new Quadrilateral(
                TerrainTile.TileWidth * -(horizontalOffset - LeftBound),
                TerrainTile.TileWidth * (horizontalOffset - RightBound),
                TerrainTile.TileWidth * (verticalOffset - TopBound),
                TerrainTile.TileWidth * -(verticalOffset - BottomBound));

            var mapFlags
                = MapFlags.UseItemClassificationSystem
                | MapFlags.ShowWaterWavesOnRollingShores
                | MapFlags.ShowWaterWavesOnCliffShores
                | MapFlags.MeleeMap
                | MapFlags.MaskedAreasArePartiallyVisible
                | MapFlags.HasMapPropertiesMenuBeenOpened;

            var gameBuild = GameBuildsProvider.GetGameBuilds(GamePatch.v1_31_1)[0];

            var mapInfo = new MapInfo(MapInfoFormatVersion.Lua)
            {
                MapVersion = 1,
                EditorVersion = gameBuild.EditorVersion.Value,
                GameVersion = gameBuild.Version,

                MapName = "Just another Warcraft III map",
                MapAuthor = "Unknown",
                MapDescription = "Nondescript",
                RecommendedPlayers = "Any",

                CameraBounds = cameraBounds,
                CameraBoundsComplements = new(LeftBound, RightBound, BottomBound, TopBound),
                PlayableMapAreaWidth = mapWidth - LeftBound - RightBound,
                PlayableMapAreaHeight = mapHeight - BottomBound - TopBound,

                MapFlags = mapFlags,
                Tileset = tileset,

                LoadingScreenBackgroundNumber = -1,
                LoadingScreenPath = string.Empty,
                LoadingScreenText = string.Empty,
                LoadingScreenTitle = string.Empty,
                LoadingScreenSubtitle = string.Empty,

                GameDataSet = GameDataSet.Default,

                PrologueScreenPath = string.Empty,
                PrologueScreenText = string.Empty,
                PrologueScreenTitle = string.Empty,
                PrologueScreenSubtitle = string.Empty,

                FogStyle = FogStyle.Linear,
                FogStartZ = 3000f,
                FogEndZ = 5000f,
                FogDensity = 0.5f,
                FogColor = Color.Black,

                GlobalWeather = WeatherType.None,
                SoundEnvironment = string.Empty,
                LightEnvironment = Tileset.Unspecified,
                WaterTintingColor = Color.White,

                ScriptLanguage = scriptLanguage,

                SupportedModes = SupportedModes.SD | SupportedModes.HD,
                GameDataVersion = GameDataVersion.RoC,
            };

            mapInfo.Players.Add(new PlayerData
            {
                Id = 0,
                Controller = PlayerController.User,
                Race = PlayerRace.Human,
                Flags = 0,
                Name = "Player 1",
                StartPosition = Vector2.Zero,
                AllyLowPriorityFlags = new Bitmask32(0),
                AllyHighPriorityFlags = new Bitmask32(0),
            });

            mapInfo.Forces.Add(new ForceData
            {
                Flags = 0,
                Players = new Bitmask32(),
                Name = "Force 1",
            });

            return mapInfo;
        }
    }
}