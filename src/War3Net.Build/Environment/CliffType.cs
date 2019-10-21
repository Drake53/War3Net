#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace War3Net.Build.Environment
{
    // Documentation used: http://max.slid.free.fr/maxEscapeCreation/terrains.php?lang=eng
    public enum CliffType
    {
        /// ===================================== \\\
        /// <see cref="Tileset.LordaeronSummer"/> \\\
        /// ===================================== \\\

        /// <summary>
        /// Lordaeron Summer - Dirt Cliff ('cLc2').
        /// </summary>
        L_Dirt = ('c' << 0) | ('L' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Lordaeron Summer - Grass Cliff ('cLc1').
        /// </summary>
        L_Grass = ('c' << 0) | ('L' << 8) | ('c' << 16) | ('1' << 24),

        /// =================================== \\\
        /// <see cref="Tileset.LordaeronFall"/> \\\
        /// =================================== \\\

        /// <summary>
        /// Lordaeron Fall - Dirt Cliff ('cFc2').
        /// </summary>
        F_Dirt = ('c' << 0) | ('F' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Lordaeron Fall - Grass Cliff ('cFc1').
        /// </summary>
        F_Grass = ('c' << 0) | ('F' << 8) | ('c' << 16) | ('1' << 24),

        /// ===================================== \\\
        /// <see cref="Tileset.LordaeronWinter"/> \\\
        /// ===================================== \\\

        /// <summary>
        /// Lordaeron Winter - Grass Cliff ('cWc2').
        /// </summary>
        W_Grass = ('c' << 0) | ('W' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Lordaeron Winter - Snow Cliff ('cWc1').
        /// </summary>
        W_Snow = ('c' << 0) | ('W' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Barrens"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Barrens - Desert Cliff ('cBc2').
        /// </summary>
        B_Desert = ('c' << 0) | ('B' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Barrens - Grass Cliff ('cBc1').
        /// </summary>
        B_Grass = ('c' << 0) | ('B' << 8) | ('c' << 16) | ('1' << 24),

        /// =============================== \\\
        /// <see cref="Tileset.Ashenvale"/> \\\
        /// =============================== \\\

        /// <summary>
        /// Ashenvale - Dirt Cliff ('cAc2').
        /// </summary>
        A_Dirt = ('c' << 0) | ('A' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Ashenvale - Grass Cliff ('cAc1').
        /// </summary>
        A_Grass = ('c' << 0) | ('A' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Felwood"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Felwood - Dirt Cliff ('cCc2').
        /// </summary>
        C_Dirt = ('c' << 0) | ('C' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Felwood - Grass Cliff ('cCc1').
        /// </summary>
        C_Grass = ('c' << 0) | ('C' << 8) | ('c' << 16) | ('1' << 24),

        /// =============================== \\\
        /// <see cref="Tileset.Northrend"/> \\\
        /// =============================== \\\

        /// <summary>
        /// Northrend - Dirt Cliff ('cNc2').
        /// </summary>
        N_Dirt = ('c' << 0) | ('N' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Northrend - Snow Cliff ('cNc1').
        /// </summary>
        N_Snow = ('c' << 0) | ('N' << 8) | ('c' << 16) | ('1' << 24),

        /// =============================== \\\
        /// <see cref="Tileset.Cityscape"/> \\\
        /// =============================== \\\

        /// <summary>
        /// Cityscape - Dirt Cliff ('cYc2').
        /// </summary>
        Y_Dirt = ('c' << 0) | ('Y' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Cityscape - Square Tiles Cliff ('cYc1').
        /// </summary>
        Y_SquareTiles = ('c' << 0) | ('Y' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Village"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Village - Dirt Cliff ('cVc2').
        /// </summary>
        V_Dirt = ('c' << 0) | ('V' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Village - Grass Dirt Cliff ('cVc1').
        /// </summary>
        V_GrassDirt = ('c' << 0) | ('V' << 8) | ('c' << 16) | ('1' << 24),

        /// ================================= \\\
        /// <see cref="Tileset.VillageFall"/> \\\
        /// ================================= \\\

        /// <summary>
        /// Village Fall - Dirt Cliff ('cQc2').
        /// </summary>
        Q_Dirt = ('c' << 0) | ('Q' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Village Fall - Grass Thick Cliff ('cQc1').
        /// </summary>
        Q_GrassThick = ('c' << 0) | ('Q' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Dalaran"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Dalaran - Dirt Cliff ('cXc2').
        /// </summary>
        X_Dirt = ('c' << 0) | ('X' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Dalaran - Square Tiles Cliff ('cXc1').
        /// </summary>
        X_SquareTiles = ('c' << 0) | ('X' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Dungeon"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Dungeon - Dirt Cliff ('cDc2').
        /// </summary>
        D_Dirt = ('c' << 0) | ('D' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Dungeon - Square Tiles Cliff ('cDc1').
        /// </summary>
        D_SquareTiles = ('c' << 0) | ('D' << 8) | ('c' << 16) | ('1' << 24),

        /// ================================= \\\
        /// <see cref="Tileset.Underground"/> \\\
        /// ================================= \\\

        /// <summary>
        /// Underground - Dirt Cliff ('cGc2').
        /// </summary>
        G_Dirt = ('c' << 0) | ('G' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Underground - Square Tiles Cliff ('cGc1').
        /// </summary>
        G_SquareTiles = ('c' << 0) | ('G' << 8) | ('c' << 16) | ('1' << 24),

        /// ================================= \\\
        /// <see cref="Tileset.SunkenRuins"/> \\\
        /// ================================= \\\

        /// <summary>
        /// Sunken Ruins - Dirt Cliff ('cZc2').
        /// </summary>
        Z_Dirt = ('c' << 0) | ('Z' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Sunken Ruins - Large Bricks Cliff ('cZc1').
        /// </summary>
        Z_LargeBricks = ('c' << 0) | ('Z' << 8) | ('c' << 16) | ('1' << 24),

        /// ===================================== \\\
        /// <see cref="Tileset.IcecrownGlacier"/> \\\
        /// ===================================== \\\

        /// <summary>
        /// Icecrown Glacier - Rune Bricks Cliff ('cIc2').
        /// </summary>
        I_RuneBricks = ('c' << 0) | ('I' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Icecrown Glacier - Snow Cliff ('cIc1').
        /// </summary>
        I_Snow = ('c' << 0) | ('I' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Outland"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Outland - Rough Dirt Cliff ('cOc2').
        /// </summary>
        O_RoughDirt = ('c' << 0) | ('O' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Outland - Abyss Cliff ('cOc1').
        /// </summary>
        O_Abyss = ('c' << 0) | ('O' << 8) | ('c' << 16) | ('1' << 24),

        /// ================================== \\\
        /// <see cref="Tileset.BlackCitadel"/> \\\
        /// ================================== \\\

        /// <summary>
        /// Black Citadel - Dark Tiles Cliff ('cKc2').
        /// </summary>
        K_DarkTiles = ('c' << 0) | ('K' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Black Citadel - Dirt Cliff ('cKc1').
        /// </summary>
        K_Dirt = ('c' << 0) | ('K' << 8) | ('c' << 16) | ('1' << 24),

        /// ================================== \\\
        /// <see cref="Tileset.DalaranRuins"/> \\\
        /// ================================== \\\

        /// <summary>
        /// Dalaran Ruins - Dirt Cliff ('cJc2').
        /// </summary>
        J_Dirt = ('c' << 0) | ('J' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Dalaran Ruins - Square Tiles Cliff ('cJc1').
        /// </summary>
        J_SquareTiles = ('c' << 0) | ('J' << 8) | ('c' << 16) | ('1' << 24),
    }
}