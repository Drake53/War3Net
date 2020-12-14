// ------------------------------------------------------------------------------
// <copyright file="TerrainTypeProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

using War3Net.Build.Common;
using War3Net.Build.Environment;

namespace War3Net.Build.Providers
{
    public static class TerrainTypeProvider
    {
        public static IEnumerable<TerrainType> GetTerrainTypes(Tileset tileset)
        {
            switch (tileset)
            {
                case Tileset.LordaeronSummer:
                    yield return TerrainType.L_Dirt;
                    yield return TerrainType.L_RoughDirt;
                    yield return TerrainType.L_GrassyDirt;
                    yield return TerrainType.L_Rock;
                    yield return TerrainType.L_Grass;
                    yield return TerrainType.L_DarkGrass;
                    yield break;

                case Tileset.LordaeronFall:
                    yield return TerrainType.F_Dirt;
                    yield return TerrainType.F_RoughDirt;
                    yield return TerrainType.F_GrassyDirt;
                    yield return TerrainType.F_Rock;
                    yield return TerrainType.F_Grass;
                    yield return TerrainType.F_DarkGrass;
                    yield break;

                case Tileset.LordaeronWinter:
                    yield return TerrainType.W_Dirt;
                    yield return TerrainType.W_RoughDirt;
                    yield return TerrainType.W_GrassySnow;
                    yield return TerrainType.W_Rock;
                    yield return TerrainType.W_Grass;
                    yield return TerrainType.W_Snow;
                    yield break;

                case Tileset.Barrens:
                    yield return TerrainType.B_Dirt;
                    yield return TerrainType.B_RoughDirt;
                    yield return TerrainType.B_Pebbles;
                    yield return TerrainType.B_GrassyDirt;
                    yield return TerrainType.B_Desert;
                    yield return TerrainType.B_DarkDesert;
                    yield return TerrainType.B_Rock;
                    yield return TerrainType.B_Grass;
                    yield break;

                case Tileset.Ashenvale:
                    yield return TerrainType.A_Dirt;
                    yield return TerrainType.A_RoughDirt;
                    yield return TerrainType.A_Grass;
                    yield return TerrainType.A_Rock;
                    yield return TerrainType.A_LumpyGrass;
                    yield return TerrainType.A_Vines;
                    yield return TerrainType.A_GrassyDirt;
                    yield return TerrainType.A_Leaves;
                    yield break;

                case Tileset.Felwood:
                    yield return TerrainType.C_Dirt;
                    yield return TerrainType.C_RoughDirt;
                    yield return TerrainType.C_Poison;
                    yield return TerrainType.C_Rock;
                    yield return TerrainType.C_Vines;
                    yield return TerrainType.C_Grass;
                    yield return TerrainType.C_Leaves;
                    yield break;

                case Tileset.Northrend:
                    yield return TerrainType.N_Dirt;
                    yield return TerrainType.N_DarkDirt;
                    yield return TerrainType.N_Rock;
                    yield return TerrainType.N_Grass;
                    yield return TerrainType.N_Ice;
                    yield return TerrainType.N_Snow;
                    yield return TerrainType.N_PockySnow;
                    yield break;

                case Tileset.Cityscape:
                    yield return TerrainType.Y_Dirt;
                    yield return TerrainType.Y_RoughDirt;
                    yield return TerrainType.Y_BlackMarble;
                    yield return TerrainType.Y_Brick;
                    yield return TerrainType.Y_SquareTiles;
                    yield return TerrainType.Y_RoundTiles;
                    yield return TerrainType.Y_Grass;
                    yield return TerrainType.Y_GrassTrim;
                    yield return TerrainType.Y_WhiteMarble;
                    yield break;

                case Tileset.Village:
                    yield return TerrainType.V_Dirt;
                    yield return TerrainType.V_RoughDirt;
                    yield return TerrainType.V_Crops;
                    yield return TerrainType.V_CobblePath;
                    yield return TerrainType.V_StonePath;
                    yield return TerrainType.V_ShortGrass;
                    yield return TerrainType.V_Grass;
                    yield return TerrainType.V_ThickGrass;
                    yield break;

                case Tileset.VillageFall:
                    yield return TerrainType.Q_Dirt;
                    yield return TerrainType.Q_RoughDirt;
                    yield return TerrainType.Q_Crops;
                    yield return TerrainType.Q_CobblePath;
                    yield return TerrainType.Q_StonePath;
                    yield return TerrainType.Q_ShortGrass;
                    yield return TerrainType.Q_Rocks;
                    yield return TerrainType.Q_ThickGrass;
                    yield break;

                case Tileset.Dalaran:
                    yield return TerrainType.X_Dirt;
                    yield return TerrainType.X_RoughDirt;
                    yield return TerrainType.X_BlackMarble;
                    yield return TerrainType.X_BrickTiles;
                    yield return TerrainType.X_SquareTiles;
                    yield return TerrainType.X_RoundTiles;
                    yield return TerrainType.X_Grass;
                    yield return TerrainType.X_TrimGrass;
                    yield return TerrainType.X_WhiteMarble;
                    yield break;

                case Tileset.Dungeon:
                    yield return TerrainType.D_Dirt;
                    yield return TerrainType.D_Brick;
                    yield return TerrainType.D_RedStones;
                    yield return TerrainType.D_LavaCracks;
                    yield return TerrainType.D_Lava;
                    yield return TerrainType.D_DarkRocks;
                    yield return TerrainType.D_GreyStones;
                    yield return TerrainType.D_SquareTiles;
                    yield break;

                case Tileset.Underground:
                    yield return TerrainType.G_Dirt;
                    yield return TerrainType.G_Brick;
                    yield return TerrainType.G_RedStones;
                    yield return TerrainType.G_IceChunks;
                    yield return TerrainType.G_Ice;
                    yield return TerrainType.G_DarkRocks;
                    yield return TerrainType.G_GreyStones;
                    yield return TerrainType.G_SquareTiles;
                    yield break;

                case Tileset.SunkenRuins:
                    yield return TerrainType.Z_Dirt;
                    yield return TerrainType.Z_RoughDirt;
                    yield return TerrainType.Z_GrassyDirt;
                    yield return TerrainType.Z_SmallBricks;
                    yield return TerrainType.Z_Sand;
                    yield return TerrainType.Z_LargeBricks;
                    yield return TerrainType.Z_RoundTiles;
                    yield return TerrainType.Z_Grass;
                    yield return TerrainType.Z_DarkGrass;
                    yield break;

                case Tileset.IcecrownGlacier:
                    yield return TerrainType.I_Dirt;
                    yield return TerrainType.I_RoughDirt;
                    yield return TerrainType.I_DarkIce;
                    yield return TerrainType.I_BlackBricks;
                    yield return TerrainType.I_RuneBricks;
                    yield return TerrainType.I_TiledBricks;
                    yield return TerrainType.I_Ice;
                    yield return TerrainType.I_BlackSquares;
                    yield return TerrainType.I_Snow;
                    yield break;

                case Tileset.Outland:
                    yield return TerrainType.O_Dirt;
                    yield return TerrainType.O_LightDirt;
                    yield return TerrainType.O_RoughDirt;
                    yield return TerrainType.O_CrackedDirt;
                    yield return TerrainType.O_FlatStones;
                    yield return TerrainType.O_Rock;
                    yield return TerrainType.O_LightFlatStones;
                    yield return TerrainType.O_Abyss;
                    yield break;

                case Tileset.BlackCitadel:
                    yield return TerrainType.K_Dirt;
                    yield return TerrainType.K_LightDirt;
                    yield return TerrainType.K_RoughDirt;
                    yield return TerrainType.K_FlatStones;
                    yield return TerrainType.K_SmallBricks;
                    yield return TerrainType.K_LargeBricks;
                    yield return TerrainType.K_SquareTiles;
                    yield return TerrainType.K_DarkTiles;
                    yield break;

                case Tileset.DalaranRuins:
                    yield return TerrainType.J_Dirt;
                    yield return TerrainType.J_RoughDirt;
                    yield return TerrainType.J_BlackMarble;
                    yield return TerrainType.J_BrickTiles;
                    yield return TerrainType.J_SquareTiles;
                    yield return TerrainType.J_RoundTiles;
                    yield return TerrainType.J_Grass;
                    yield return TerrainType.J_TrimGrass;
                    yield return TerrainType.J_WhiteMarble;
                    yield break;

                default:
                    yield break;
            }
        }

        public static IEnumerable<CliffType> GetCliffTypes(Tileset tileset, bool legacy = false)
        {
            switch (tileset)
            {
                case Tileset.LordaeronSummer:
                    yield return CliffType.L_Dirt;
                    yield return CliffType.L_Grass;
                    if (legacy)
                    {
                        yield return CliffType.L_Unknown;
                    }
                    yield break;

                case Tileset.LordaeronFall:
                    if (legacy)
                    {
                        yield return CliffType.L_Dirt;
                    }
                    yield return CliffType.F_Dirt;
                    yield return CliffType.F_Grass;
                    yield break;

                case Tileset.LordaeronWinter:
                    if (legacy)
                    {
                        yield return CliffType.W_Snow;
                        yield return CliffType.W_Grass;
                    }
                    else
                    {
                        yield return CliffType.W_Grass;
                        yield return CliffType.W_Snow;
                    }
                    yield break;

                case Tileset.Barrens:
                    yield return CliffType.B_Desert;
                    yield return CliffType.B_Grass;
                    yield break;

                case Tileset.Ashenvale:
                    yield return CliffType.A_Grass;
                    yield return CliffType.A_Dirt;
                    yield break;

                case Tileset.Felwood:
                    yield return CliffType.C_Grass;
                    yield return CliffType.C_Dirt;
                    yield break;

                case Tileset.Northrend:
                    yield return CliffType.N_Dirt;
                    yield return CliffType.N_Snow;
                    yield break;

                case Tileset.Cityscape:
                    yield return CliffType.Y_Dirt;
                    yield return CliffType.Y_SquareTiles;
                    yield break;

                case Tileset.Village:
                    yield return CliffType.V_Dirt;
                    yield return CliffType.V_GrassDirt;
                    yield break;

                case Tileset.VillageFall:
                    yield return CliffType.Q_Dirt;
                    yield return CliffType.Q_GrassThick;
                    yield break;

                case Tileset.Dalaran:
                    yield return CliffType.X_Dirt;
                    yield return CliffType.X_SquareTiles;
                    yield break;

                case Tileset.Dungeon:
                    yield return CliffType.D_Dirt;
                    yield return CliffType.D_SquareTiles;
                    yield break;

                case Tileset.Underground:
                    yield return CliffType.G_Dirt;
                    yield return CliffType.G_SquareTiles;
                    yield break;

                case Tileset.SunkenRuins:
                    yield return CliffType.Z_Dirt;
                    yield return CliffType.Z_LargeBricks;
                    yield break;

                case Tileset.IcecrownGlacier:
                    yield return CliffType.I_Snow;
                    yield return CliffType.I_RuneBricks;
                    yield break;

                case Tileset.Outland:
                    yield return CliffType.O_Abyss;
                    yield return CliffType.O_RoughDirt;
                    yield break;

                case Tileset.BlackCitadel:
                    yield return CliffType.K_Dirt;
                    yield return CliffType.K_DarkTiles;
                    yield break;

                case Tileset.DalaranRuins:
                    yield return CliffType.J_Dirt;
                    yield return CliffType.J_SquareTiles;
                    yield break;

                default:
                    yield break;
            }
        }

        public static TerrainType GetDefaultTerrainType(Tileset tileset)
        {
            return tileset switch
            {
                Tileset.LordaeronSummer => TerrainType.L_Dirt,

                // TODO

                _ => throw new InvalidEnumArgumentException(nameof(tileset), (int)tileset, typeof(Tileset)),
            };
        }
    }
}