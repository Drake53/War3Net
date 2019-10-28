// ------------------------------------------------------------------------------
// <copyright file="CliffType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace War3Net.Build.Environment
{
    public enum CliffType
    {
        /// ===================================== \\\
        /// <see cref="Tileset.LordaeronSummer"/> \\\
        /// ===================================== \\\

        /// <summary>
        /// Lordaeron Summer - Dirt Cliff ('CLdi').
        /// </summary>
        L_Dirt = ('C' << 0) | ('L' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Lordaeron Summer - Grass Cliff ('CLgr').
        /// </summary>
        L_Grass = ('C' << 0) | ('L' << 8) | ('g' << 16) | ('r' << 24),

        /// =================================== \\\
        /// <see cref="Tileset.LordaeronFall"/> \\\
        /// =================================== \\\

        /// <summary>
        /// Lordaeron Fall - Dirt Cliff ('CFdi').
        /// </summary>
        F_Dirt = ('C' << 0) | ('F' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Lordaeron Fall - Grass Cliff ('CFgr').
        /// </summary>
        F_Grass = ('C' << 0) | ('F' << 8) | ('g' << 16) | ('r' << 24),

        /// ===================================== \\\
        /// <see cref="Tileset.LordaeronWinter"/> \\\
        /// ===================================== \\\

        /// <summary>
        /// Lordaeron Winter - Grass Cliff ('CWgr').
        /// </summary>
        W_Grass = ('C' << 0) | ('W' << 8) | ('g' << 16) | ('r' << 24),

        /// <summary>
        /// Lordaeron Winter - Snow Cliff ('CWsn').
        /// </summary>
        W_Snow = ('C' << 0) | ('W' << 8) | ('s' << 16) | ('n' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Barrens"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Barrens - Desert Cliff ('CBde').
        /// </summary>
        B_Desert = ('C' << 0) | ('B' << 8) | ('d' << 16) | ('e' << 24),

        /// <summary>
        /// Barrens - Grass Cliff ('CBgr').
        /// </summary>
        B_Grass = ('C' << 0) | ('B' << 8) | ('g' << 16) | ('r' << 24),

        /// =============================== \\\
        /// <see cref="Tileset.Ashenvale"/> \\\
        /// =============================== \\\

        /// <summary>
        /// Ashenvale - Grass Cliff ('CAgr').
        /// </summary>
        A_Grass = ('C' << 0) | ('A' << 8) | ('g' << 16) | ('r' << 24),

        /// <summary>
        /// Ashenvale - Dirt Cliff ('CAdi').
        /// </summary>
        A_Dirt = ('C' << 0) | ('A' << 8) | ('d' << 16) | ('i' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Felwood"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Felwood - Grass Cliff ('CCgr').
        /// </summary>
        C_Grass = ('C' << 0) | ('C' << 8) | ('g' << 16) | ('r' << 24),

        /// <summary>
        /// Felwood - Dirt Cliff ('CCdi').
        /// </summary>
        C_Dirt = ('C' << 0) | ('C' << 8) | ('d' << 16) | ('i' << 24),

        /// =============================== \\\
        /// <see cref="Tileset.Northrend"/> \\\
        /// =============================== \\\

        /// <summary>
        /// Northrend - Dirt Cliff ('CNdi').
        /// </summary>
        N_Dirt = ('C' << 0) | ('N' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Northrend - Snow Cliff ('CNsn').
        /// </summary>
        N_Snow = ('C' << 0) | ('N' << 8) | ('s' << 16) | ('n' << 24),

        /// =============================== \\\
        /// <see cref="Tileset.Cityscape"/> \\\
        /// =============================== \\\

        /// <summary>
        /// Cityscape - Dirt Cliff ('CYdi').
        /// </summary>
        Y_Dirt = ('C' << 0) | ('Y' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Cityscape - Square Tiles Cliff ('CYsq').
        /// </summary>
        Y_SquareTiles = ('C' << 0) | ('Y' << 8) | ('s' << 16) | ('q' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Village"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Village - Dirt Cliff ('CVdi').
        /// </summary>
        V_Dirt = ('C' << 0) | ('V' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Village - Grass Dirt Cliff ('CVgr').
        /// </summary>
        V_GrassDirt = ('C' << 0) | ('V' << 8) | ('g' << 16) | ('r' << 24),

        /// ================================= \\\
        /// <see cref="Tileset.VillageFall"/> \\\
        /// ================================= \\\

        /// <summary>
        /// Village Fall - Dirt Cliff ('CQdi').
        /// </summary>
        Q_Dirt = ('C' << 0) | ('Q' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Village Fall - Grass Thick Cliff ('CQgr').
        /// </summary>
        Q_GrassThick = ('C' << 0) | ('Q' << 8) | ('g' << 16) | ('r' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Dalaran"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Dalaran - Dirt Cliff ('CXdi').
        /// </summary>
        X_Dirt = ('C' << 0) | ('X' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Dalaran - Square Tiles Cliff ('CXsq').
        /// </summary>
        X_SquareTiles = ('C' << 0) | ('X' << 8) | ('s' << 16) | ('q' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Dungeon"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Dungeon - Dirt Cliff ('CDdi').
        /// </summary>
        D_Dirt = ('C' << 0) | ('D' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Dungeon - Square Tiles Cliff ('CDsq').
        /// </summary>
        D_SquareTiles = ('C' << 0) | ('D' << 8) | ('s' << 16) | ('q' << 24),

        /// ================================= \\\
        /// <see cref="Tileset.Underground"/> \\\
        /// ================================= \\\

        /// <summary>
        /// Underground - Dirt Cliff ('CGdi').
        /// </summary>
        G_Dirt = ('C' << 0) | ('G' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Underground - Square Tiles Cliff ('CGsq').
        /// </summary>
        G_SquareTiles = ('C' << 0) | ('G' << 8) | ('s' << 16) | ('q' << 24),

        /// ================================= \\\
        /// <see cref="Tileset.SunkenRuins"/> \\\
        /// ================================= \\\

        /// <summary>
        /// Sunken Ruins - Dirt Cliff ('CZdi').
        /// </summary>
        Z_Dirt = ('C' << 0) | ('Z' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Sunken Ruins - Large Bricks Cliff ('CZlb').
        /// </summary>
        Z_LargeBricks = ('C' << 0) | ('Z' << 8) | ('l' << 16) | ('b' << 24),

        /// ===================================== \\\
        /// <see cref="Tileset.IcecrownGlacier"/> \\\
        /// ===================================== \\\

        /// <summary>
        /// Icecrown Glacier - Snow Cliff ('CIsn').
        /// </summary>
        I_Snow = ('C' << 0) | ('I' << 8) | ('s' << 16) | ('n' << 24),

        /// <summary>
        /// Icecrown Glacier - Rune Bricks Cliff ('CIrb').
        /// </summary>
        I_RuneBricks = ('C' << 0) | ('I' << 8) | ('r' << 16) | ('b' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Outland"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Outland - Abyss Cliff ('COdi').
        /// </summary>
        O_Abyss = ('C' << 0) | ('O' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Outland - Rough Dirt Cliff ('COrd').
        /// </summary>
        O_RoughDirt = ('C' << 0) | ('O' << 8) | ('r' << 16) | ('d' << 24),

        /// ================================== \\\
        /// <see cref="Tileset.BlackCitadel"/> \\\
        /// ================================== \\\

        /// <summary>
        /// Black Citadel - Dirt Cliff ('CKdi').
        /// </summary>
        K_Dirt = ('C' << 0) | ('K' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Black Citadel - Dark Tiles Cliff ('CKdt').
        /// </summary>
        K_DarkTiles = ('C' << 0) | ('K' << 8) | ('d' << 16) | ('t' << 24),

        /// ================================== \\\
        /// <see cref="Tileset.DalaranRuins"/> \\\
        /// ================================== \\\

        /// <summary>
        /// Dalaran Ruins - Dirt Cliff ('CJdi').
        /// </summary>
        J_Dirt = ('C' << 0) | ('J' << 8) | ('d' << 16) | ('i' << 24),

        /// <summary>
        /// Dalaran Ruins - Square Tiles Cliff ('CJsq').
        /// </summary>
        J_SquareTiles = ('C' << 0) | ('J' << 8) | ('s' << 16) | ('q' << 24),
    }
}