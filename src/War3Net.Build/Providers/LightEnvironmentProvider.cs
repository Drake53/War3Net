// ------------------------------------------------------------------------------
// <copyright file="LightEnvironmentProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Common;

namespace War3Net.Build.Providers
{
    public static class LightEnvironmentProvider
    {
        public static string GetTerrainLightEnvironmentModel(Tileset tileset)
        {
            switch (tileset)
            {
                case Tileset.Ashenvale: return @"Environment\\DNC\\DNCAshenvale\\DNCAshenvaleTerrain\\DNCAshenvaleTerrain.mdl";
                case Tileset.Barrens: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";
                case Tileset.BlackCitadel: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";
                case Tileset.Cityscape: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";
                case Tileset.Dalaran: return @"Environment\\DNC\\DNCDalaran\\DNCDalaranTerrain\\DNCDalaranTerrain.mdl";
                case Tileset.DalaranRuins: return @"Environment\\DNC\\DNCDalaran\\DNCDalaranTerrain\\DNCDalaranTerrain.mdl";
                case Tileset.Dungeon: return @"Environment\\DNC\\DNCDungeon\\DNCDungeonTerrain\\DNCDungeonTerrain.mdl";
                case Tileset.Felwood: return @"Environment\\DNC\\DNCFelwood\\DNCFelwoodTerrain\\DNCFelwoodTerrain.mdl";
                case Tileset.IcecrownGlacier: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";
                case Tileset.LordaeronFall: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";
                case Tileset.LordaeronSummer: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";
                case Tileset.LordaeronWinter: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";
                case Tileset.Northrend: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";
                case Tileset.Outland: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";
                case Tileset.SunkenRuins: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";
                case Tileset.Underground: return @"Environment\\DNC\\DNCUnderground\\DNCUndergroundTerrain\\DNCUndergroundTerrain.mdl";
                case Tileset.Village: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";
                case Tileset.VillageFall: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl";

                default: return string.Empty;
            }
        }

        public static string GetUnitLightEnvironmentModel(Tileset tileset)
        {
            switch (tileset)
            {
                case Tileset.Ashenvale: return @"Environment\\DNC\\DNCAshenvale\\DNCAshenvaleUnit\\DNCAshenvaleUnit.mdl";
                case Tileset.Barrens: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";
                case Tileset.BlackCitadel: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";
                case Tileset.Cityscape: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";
                case Tileset.Dalaran: return @"Environment\\DNC\\DNCDalaran\\DNCDalaranUnit\\DNCDalaranUnit.mdl";
                case Tileset.DalaranRuins: return @"Environment\\DNC\\DNCDalaran\\DNCDalaranUnit\\DNCDalaranUnit.mdl";
                case Tileset.Dungeon: return @"Environment\\DNC\\DNCDungeon\\DNCDungeonUnit\\DNCDungeonUnit.mdl";
                case Tileset.Felwood: return @"Environment\\DNC\\DNCFelwood\\DNCFelwoodUnit\\DNCFelwoodUnit.mdl";
                case Tileset.IcecrownGlacier: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";
                case Tileset.LordaeronFall: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";
                case Tileset.LordaeronSummer: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";
                case Tileset.LordaeronWinter: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";
                case Tileset.Northrend: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";
                case Tileset.Outland: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";
                case Tileset.SunkenRuins: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";
                case Tileset.Underground: return @"Environment\\DNC\\DNCUnderground\\DNCUndergroundUnit\\DNCUndergroundUnit.mdl";
                case Tileset.Village: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";
                case Tileset.VillageFall: return @"Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl";

                default: return string.Empty;
            }
        }
    }
}