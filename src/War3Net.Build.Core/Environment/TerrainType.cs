// ------------------------------------------------------------------------------
// <copyright file="TerrainType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace War3Net.Build.Environment
{
    // Documentation used: http://max.slid.free.fr/maxEscapeCreation/terrains.php?lang=eng
    public enum TerrainType
    {
        /// ===================================== \\\
        /// <see cref="Tileset.LordaeronSummer"/> \\\
        /// ===================================== \\\

        /// <summary>
        /// Lordaeron Summer - Dirt ('Ldrt').
        /// </summary>
        L_Dirt = ('L' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Lordaeron Summer - Rough Dirt ('Ldro').
        /// </summary>
        L_RoughDirt = ('L' << 0) | ('d' << 8) | ('r' << 16) | ('o' << 24),

        /// <summary>
        /// Lordaeron Summer - Grassy Dirt ('Ldrg').
        /// </summary>
        L_GrassyDirt = ('L' << 0) | ('d' << 8) | ('r' << 16) | ('g' << 24),

        /// <summary>
        /// Lordaeron Summer - Rock ('Lrok').
        /// </summary>
        L_Rock = ('L' << 0) | ('r' << 8) | ('o' << 16) | ('k' << 24),

        /// <summary>
        /// Lordaeron Summer - Grass ('Lgrs').
        /// </summary>
        L_Grass = ('L' << 0) | ('g' << 8) | ('r' << 16) | ('s' << 24),

        /// <summary>
        /// Lordaeron Summer - Dark Grass ('Lgrd').
        /// </summary>
        L_DarkGrass = ('L' << 0) | ('g' << 8) | ('r' << 16) | ('d' << 24),

        /// <summary>
        /// Lordaeron Summer - Dirt Cliff ('cLc2').
        /// </summary>
        L_DirtCliff = ('c' << 0) | ('L' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Lordaeron Summer - Grass Cliff ('cLc1').
        /// </summary>
        L_GrassCliff = ('c' << 0) | ('L' << 8) | ('c' << 16) | ('1' << 24),

        /// =================================== \\\
        /// <see cref="Tileset.LordaeronFall"/> \\\
        /// =================================== \\\

        /// <summary>
        /// Lordaeron Fall - Dirt ('Fdrt').
        /// </summary>
        F_Dirt = ('F' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Lordaeron Fall - Rough Dirt ('Fdro').
        /// </summary>
        F_RoughDirt = ('F' << 0) | ('d' << 8) | ('r' << 16) | ('o' << 24),

        /// <summary>
        /// Lordaeron Fall - Grassy Dirt ('Fdrg').
        /// </summary>
        F_GrassyDirt = ('F' << 0) | ('d' << 8) | ('r' << 16) | ('g' << 24),

        /// <summary>
        /// Lordaeron Fall - Rock ('Frok').
        /// </summary>
        F_Rock = ('F' << 0) | ('r' << 8) | ('o' << 16) | ('k' << 24),

        /// <summary>
        /// Lordaeron Fall - Grass ('Fgrs').
        /// </summary>
        F_Grass = ('F' << 0) | ('g' << 8) | ('r' << 16) | ('s' << 24),

        /// <summary>
        /// Lordaeron Fall - Dark Grass ('Fgrd').
        /// </summary>
        F_DarkGrass = ('F' << 0) | ('g' << 8) | ('r' << 16) | ('d' << 24),

        /// <summary>
        /// Lordaeron Fall - Dirt Cliff ('cFc2').
        /// </summary>
        F_DirtCliff = ('c' << 0) | ('F' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Lordaeron Fall - Grass Cliff ('cFc1').
        /// </summary>
        F_GrassCliff = ('c' << 0) | ('F' << 8) | ('c' << 16) | ('1' << 24),

        /// ===================================== \\\
        /// <see cref="Tileset.LordaeronWinter"/> \\\
        /// ===================================== \\\

        /// <summary>
        /// Lordaeron Winter - Dirt ('Wdrt').
        /// </summary>
        W_Dirt = ('W' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Lordaeron Winter - Rough Dirt ('Wdro').
        /// </summary>
        W_RoughDirt = ('W' << 0) | ('d' << 8) | ('r' << 16) | ('o' << 24),

        /// <summary>
        /// Lordaeron Winter - Grassy Snow ('Wsng').
        /// </summary>
        W_GrassySnow = ('W' << 0) | ('s' << 8) | ('n' << 16) | ('g' << 24),

        /// <summary>
        /// Lordaeron Winter - Rock ('Wrok').
        /// </summary>
        W_Rock = ('W' << 0) | ('r' << 8) | ('o' << 16) | ('k' << 24),

        /// <summary>
        /// Lordaeron Winter - Grass ('Wgrs').
        /// </summary>
        W_Grass = ('W' << 0) | ('g' << 8) | ('r' << 16) | ('s' << 24),

        /// <summary>
        /// Lordaeron Winter - Snow ('Wsnw').
        /// </summary>
        W_Snow = ('W' << 0) | ('s' << 8) | ('n' << 16) | ('w' << 24),

        /// <summary>
        /// Lordaeron Winter - Grass Cliff ('cWc2').
        /// </summary>
        W_GrassCliff = ('c' << 0) | ('W' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Lordaeron Winter - Snow Cliff ('cWc1').
        /// </summary>
        W_SnowCliff = ('c' << 0) | ('W' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Barrens"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Barrens - Dirt ('Bdrt').
        /// </summary>
        B_Dirt = ('B' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Barrens - Rough Dirt ('Bdrh').
        /// </summary>
        B_RoughDirt = ('B' << 0) | ('d' << 8) | ('r' << 16) | ('h' << 24),

        /// <summary>
        /// Barrens - Pebbles ('Bdrr').
        /// </summary>
        B_Pebbles = ('B' << 0) | ('d' << 8) | ('r' << 16) | ('r' << 24),

        /// <summary>
        /// Barrens - Grassy Dirt ('Bdrg').
        /// </summary>
        B_GrassyDirt = ('B' << 0) | ('d' << 8) | ('r' << 16) | ('g' << 24),

        /// <summary>
        /// Barrens - Desert ('Bdsr').
        /// </summary>
        B_Desert = ('B' << 0) | ('d' << 8) | ('s' << 16) | ('r' << 24),

        /// <summary>
        /// Barrens - Dark Desert ('Bdsd').
        /// </summary>
        B_DarkDesert = ('B' << 0) | ('d' << 8) | ('s' << 16) | ('d' << 24),

        /// <summary>
        /// Barrens - Rock ('Bflr').
        /// </summary>
        B_Rock = ('B' << 0) | ('f' << 8) | ('l' << 16) | ('r' << 24),

        /// <summary>
        /// Barrens - Grass ('Bgrr').
        /// </summary>
        B_Grass = ('B' << 0) | ('g' << 8) | ('r' << 16) | ('r' << 24),

        /// <summary>
        /// Barrens - Desert Cliff ('cBc2').
        /// </summary>
        B_DesertCliff = ('c' << 0) | ('B' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Barrens - Grass Cliff ('cBc1').
        /// </summary>
        B_GrassCliff = ('c' << 0) | ('B' << 8) | ('c' << 16) | ('1' << 24),

        /// =============================== \\\
        /// <see cref="Tileset.Ashenvale"/> \\\
        /// =============================== \\\

        /// <summary>
        /// Ashenvale - Dirt ('Adrt').
        /// </summary>
        A_Dirt = ('A' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Ashenvale - Rough Dirt ('Adrd').
        /// </summary>
        A_RoughDirt = ('A' << 0) | ('d' << 8) | ('r' << 16) | ('d' << 24),

        /// <summary>
        /// Ashenvale - Grass ('Agrs').
        /// </summary>
        A_Grass = ('A' << 0) | ('g' << 8) | ('r' << 16) | ('s' << 24),

        /// <summary>
        /// Ashenvale - Rock ('Arck').
        /// </summary>
        A_Rock = ('A' << 0) | ('r' << 8) | ('c' << 16) | ('k' << 24),

        /// <summary>
        /// Ashenvale - Lumpy Grass ('Agrd').
        /// </summary>
        A_LumpyGrass = ('A' << 0) | ('g' << 8) | ('r' << 16) | ('d' << 24),

        /// <summary>
        /// Ashenvale - Vines ('Avin').
        /// </summary>
        A_Vines = ('A' << 0) | ('v' << 8) | ('i' << 16) | ('n' << 24),

        /// <summary>
        /// Ashenvale - Grassy Dirt ('Adrg').
        /// </summary>
        A_GrassyDirt = ('A' << 0) | ('d' << 8) | ('r' << 16) | ('g' << 24),

        /// <summary>
        /// Ashenvale - Leaves ('Alvd').
        /// </summary>
        A_Leaves = ('A' << 0) | ('l' << 8) | ('v' << 16) | ('d' << 24),

        /// <summary>
        /// Ashenvale - Dirt Cliff ('cAc2').
        /// </summary>
        A_DirtCliff = ('c' << 0) | ('A' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Ashenvale - Grass Cliff ('cAc1').
        /// </summary>
        A_GrassCliff = ('c' << 0) | ('A' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Felwood"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Felwood - Dirt ('Cdrt').
        /// </summary>
        C_Dirt = ('C' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Felwood - Rough Dirt ('Cdrd').
        /// </summary>
        C_RoughDirt = ('C' << 0) | ('d' << 8) | ('r' << 16) | ('d' << 24),

        /// <summary>
        /// Felwood - Poison ('Cpos').
        /// </summary>
        C_Poison = ('C' << 0) | ('p' << 8) | ('o' << 16) | ('s' << 24),

        /// <summary>
        /// Felwood - Rock ('Crck').
        /// </summary>
        C_Rock = ('C' << 0) | ('r' << 8) | ('c' << 16) | ('k' << 24),

        /// <summary>
        /// Felwood - Vines ('Cvin').
        /// </summary>
        C_Vines = ('C' << 0) | ('v' << 8) | ('i' << 16) | ('n' << 24),

        /// <summary>
        /// Felwood - Grass ('Cgrs').
        /// </summary>
        C_Grass = ('C' << 0) | ('g' << 8) | ('r' << 16) | ('s' << 24),

        /// <summary>
        /// Felwood - Leaves ('Clvg').
        /// </summary>
        C_Leaves = ('C' << 0) | ('l' << 8) | ('v' << 16) | ('g' << 24),

        /// <summary>
        /// Felwood - Dirt Cliff ('cCc2').
        /// </summary>
        C_DirtCliff = ('c' << 0) | ('C' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Felwood - Grass Cliff ('cCc1').
        /// </summary>
        C_GrassCliff = ('c' << 0) | ('C' << 8) | ('c' << 16) | ('1' << 24),

        /// =============================== \\\
        /// <see cref="Tileset.Northrend"/> \\\
        /// =============================== \\\

        /// <summary>
        /// Northrend - Dirt ('Ndrt').
        /// </summary>
        N_Dirt = ('N' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Northrend - Dark Dirt ('Ndrd').
        /// </summary>
        N_DarkDirt = ('N' << 0) | ('d' << 8) | ('r' << 16) | ('d' << 24),

        /// <summary>
        /// Northrend - Rock ('Nrck').
        /// </summary>
        N_Rock = ('N' << 0) | ('r' << 8) | ('c' << 16) | ('k' << 24),

        /// <summary>
        /// Northrend - Grass ('Ngrs').
        /// </summary>
        N_Grass = ('N' << 0) | ('g' << 8) | ('r' << 16) | ('s' << 24),

        /// <summary>
        /// Northrend - Ice ('Nice').
        /// </summary>
        N_Ice = ('N' << 0) | ('i' << 8) | ('c' << 16) | ('e' << 24),

        /// <summary>
        /// Northrend - Snow ('Nsnw').
        /// </summary>
        N_Snow = ('N' << 0) | ('s' << 8) | ('n' << 16) | ('w' << 24),

        /// <summary>
        /// Northrend - Pocky Snow ('Nsnr').
        /// </summary>
        N_PockySnow = ('N' << 0) | ('s' << 8) | ('n' << 16) | ('r' << 24),

        /// <summary>
        /// Northrend - Dirt Cliff ('cNc2').
        /// </summary>
        N_DirtCliff = ('c' << 0) | ('N' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Northrend - Snow Cliff ('cNc1').
        /// </summary>
        N_SnowCliff = ('c' << 0) | ('N' << 8) | ('c' << 16) | ('1' << 24),

        /// =============================== \\\
        /// <see cref="Tileset.Cityscape"/> \\\
        /// =============================== \\\

        /// <summary>
        /// Cityscape - Dirt ('Ydrt').
        /// </summary>
        Y_Dirt = ('Y' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Cityscape - Rough Dirt ('Ydtr').
        /// </summary>
        Y_RoughDirt = ('Y' << 0) | ('d' << 8) | ('t' << 16) | ('r' << 24),

        /// <summary>
        /// Cityscape - Black Marble ('Yblm').
        /// </summary>
        Y_BlackMarble = ('Y' << 0) | ('b' << 8) | ('l' << 16) | ('m' << 24),

        /// <summary>
        /// Cityscape - Brick ('Ybtl').
        /// </summary>
        Y_Brick = ('Y' << 0) | ('b' << 8) | ('t' << 16) | ('l' << 24),

        /// <summary>
        /// Cityscape - Square Tiles ('Ysqd').
        /// </summary>
        Y_SquareTiles = ('Y' << 0) | ('s' << 8) | ('q' << 16) | ('d' << 24),

        /// <summary>
        /// Cityscape - Round Tiles ('Yrtl').
        /// </summary>
        Y_RoundTiles = ('Y' << 0) | ('r' << 8) | ('t' << 16) | ('l' << 24),

        /// <summary>
        /// Cityscape - Grass ('Ygsb').
        /// </summary>
        Y_Grass = ('Y' << 0) | ('g' << 8) | ('s' << 16) | ('b' << 24),

        /// <summary>
        /// Cityscape - Grass Trim ('Yhdg').
        /// </summary>
        Y_GrassTrim = ('Y' << 0) | ('h' << 8) | ('d' << 16) | ('g' << 24),

        /// <summary>
        /// Cityscape - White Marble ('Ywmb').
        /// </summary>
        Y_WhiteMarble = ('Y' << 0) | ('w' << 8) | ('m' << 16) | ('b' << 24),

        /// <summary>
        /// Cityscape - Dirt Cliff ('cYc2').
        /// </summary>
        Y_DirtCliff = ('c' << 0) | ('Y' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Cityscape - Square Tiles Cliff ('cYc1').
        /// </summary>
        Y_SquareTilesCliff = ('c' << 0) | ('Y' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Village"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Village - Dirt ('Vdrt').
        /// </summary>
        V_Dirt = ('V' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Village - Rough Dirt ('Vdrr').
        /// </summary>
        V_RoughDirt = ('V' << 0) | ('d' << 8) | ('r' << 16) | ('r' << 24),

        /// <summary>
        /// Village - Crops ('Vcrp').
        /// </summary>
        V_Crops = ('V' << 0) | ('c' << 8) | ('r' << 16) | ('p' << 24),

        /// <summary>
        /// Village - Cobble Path ('Vcbp').
        /// </summary>
        V_CobblePath = ('V' << 0) | ('c' << 8) | ('b' << 16) | ('p' << 24),

        /// <summary>
        /// Village - Stone Path ('Vstp').
        /// </summary>
        V_StonePath = ('V' << 0) | ('s' << 8) | ('t' << 16) | ('p' << 24),

        /// <summary>
        /// Village - Short Grass ('Vgrs').
        /// </summary>
        V_ShortGrass = ('V' << 0) | ('g' << 8) | ('r' << 16) | ('s' << 24),

        /// <summary>
        /// Village - Rocks ('Vrck').
        /// </summary>
        V_Grass = ('V' << 0) | ('r' << 8) | ('c' << 16) | ('k' << 24),

        /// <summary>
        /// Village - Thick Grass ('Vgrt').
        /// </summary>
        V_ThickGrass = ('V' << 0) | ('g' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Village - Dirt Cliff ('cVc2').
        /// </summary>
        V_DirtCliff = ('c' << 0) | ('V' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Village - Grass Dirt Cliff ('cVc1').
        /// </summary>
        V_GrassDirtCliff = ('c' << 0) | ('V' << 8) | ('c' << 16) | ('1' << 24),

        /// ================================= \\\
        /// <see cref="Tileset.VillageFall"/> \\\
        /// ================================= \\\

        /// <summary>
        /// Village Fall - Dirt ('Qdrt').
        /// </summary>
        Q_Dirt = ('Q' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Village Fall - Rough Dirt ('Qdrr').
        /// </summary>
        Q_RoughDirt = ('Q' << 0) | ('d' << 8) | ('r' << 16) | ('r' << 24),

        /// <summary>
        /// Village Fall - Crops ('Qcrp').
        /// </summary>
        Q_Crops = ('Q' << 0) | ('c' << 8) | ('r' << 16) | ('p' << 24),

        /// <summary>
        /// Village Fall - Cobble Path ('Qcbp').
        /// </summary>
        Q_CobblePath = ('Q' << 0) | ('c' << 8) | ('b' << 16) | ('p' << 24),

        /// <summary>
        /// Village Fall - Stone Path ('Qstp').
        /// </summary>
        Q_StonePath = ('Q' << 0) | ('s' << 8) | ('t' << 16) | ('p' << 24),

        /// <summary>
        /// Village Fall - Short Grass ('Qgrs').
        /// </summary>
        Q_ShortGrass = ('Q' << 0) | ('g' << 8) | ('r' << 16) | ('s' << 24),

        /// <summary>
        /// Village Fall - Rocks ('Qrck').
        /// </summary>
        Q_Rocks = ('Q' << 0) | ('r' << 8) | ('c' << 16) | ('k' << 24),

        /// <summary>
        /// Village Fall - Thick Grass ('Qgrt').
        /// </summary>
        Q_ThickGrass = ('Q' << 0) | ('g' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Village Fall - Dirt Cliff ('cQc2').
        /// </summary>
        Q_DirtCliff = ('c' << 0) | ('Q' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Village Fall - Grass Thick Cliff ('cQc1').
        /// </summary>
        Q_GrassThickCliff = ('c' << 0) | ('Q' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Dalaran"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Dalaran - Dirt ('Xdrt').
        /// </summary>
        X_Dirt = ('X' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Dalaran - Rough Dirt ('Xdtr').
        /// </summary>
        X_RoughDirt = ('X' << 0) | ('d' << 8) | ('t' << 16) | ('r' << 24),

        /// <summary>
        /// Dalaran - Black Marble ('Xblm').
        /// </summary>
        X_BlackMarble = ('X' << 0) | ('b' << 8) | ('l' << 16) | ('m' << 24),

        /// <summary>
        /// Dalaran - Brick Tiles ('Xbtl').
        /// </summary>
        X_BrickTiles = ('X' << 0) | ('b' << 8) | ('t' << 16) | ('l' << 24),

        /// <summary>
        /// Dalaran - Square Tiles ('Xsqd').
        /// </summary>
        X_SquareTiles = ('X' << 0) | ('s' << 8) | ('q' << 16) | ('d' << 24),

        /// <summary>
        /// Dalaran - Round Tiles ('Xrtl').
        /// </summary>
        X_RoundTiles = ('X' << 0) | ('r' << 8) | ('t' << 16) | ('l' << 24),

        /// <summary>
        /// Dalaran - Grass ('Xgsb').
        /// </summary>
        X_Grass = ('X' << 0) | ('g' << 8) | ('s' << 16) | ('b' << 24),

        /// <summary>
        /// Dalaran - Trim Grass ('Xhdg').
        /// </summary>
        X_TrimGrass = ('X' << 0) | ('h' << 8) | ('d' << 16) | ('g' << 24),

        /// <summary>
        /// Dalaran - White Marble ('Xwmb').
        /// </summary>
        X_WhiteMarble = ('X' << 0) | ('w' << 8) | ('m' << 16) | ('b' << 24),

        /// <summary>
        /// Dalaran - Dirt Cliff ('cXc2').
        /// </summary>
        X_DirtCliff = ('c' << 0) | ('X' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Dalaran - Square Tiles Cliff ('cXc1').
        /// </summary>
        X_SquareTilesCliff = ('c' << 0) | ('X' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Dungeon"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Dungeon - Dirt ('Ddrt').
        /// </summary>
        D_Dirt = ('D' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Dungeon - Brick ('Dbrk').
        /// </summary>
        D_Brick = ('D' << 0) | ('b' << 8) | ('r' << 16) | ('k' << 24),

        /// <summary>
        /// Dungeon - Red Stones ('Drds').
        /// </summary>
        D_RedStones = ('D' << 0) | ('r' << 8) | ('d' << 16) | ('s' << 24),

        /// <summary>
        /// Dungeon - Lava Cracks ('Dlvc').
        /// </summary>
        D_LavaCracks = ('D' << 0) | ('l' << 8) | ('v' << 16) | ('c' << 24),

        /// <summary>
        /// Dungeon - Lava ('Dlav').
        /// </summary>
        D_Lava = ('D' << 0) | ('l' << 8) | ('a' << 16) | ('v' << 24),

        /// <summary>
        /// Dungeon - Dark Rocks ('Ddkr').
        /// </summary>
        D_DarkRocks = ('D' << 0) | ('d' << 8) | ('k' << 16) | ('r' << 24),

        /// <summary>
        /// Dungeon - Grey Stones ('Dgrs').
        /// </summary>
        D_GreyStones = ('D' << 0) | ('g' << 8) | ('r' << 16) | ('s' << 24),

        /// <summary>
        /// Dungeon - Square Tiles ('Dsqd').
        /// </summary>
        D_SquareTiles = ('D' << 0) | ('s' << 8) | ('q' << 16) | ('d' << 24),

        /// <summary>
        /// Dungeon - Dirt Cliff ('cDc2').
        /// </summary>
        D_DirtCliff = ('c' << 0) | ('D' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Dungeon - Square Tiles Cliff ('cDc1').
        /// </summary>
        D_SquareTilesCliff = ('c' << 0) | ('D' << 8) | ('c' << 16) | ('1' << 24),

        /// ================================= \\\
        /// <see cref="Tileset.Underground"/> \\\
        /// ================================= \\\

        /// <summary>
        /// Underground - Dirt ('Gdrt').
        /// </summary>
        G_Dirt = ('G' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Underground - Brick ('Gbrk').
        /// </summary>
        G_Brick = ('G' << 0) | ('b' << 8) | ('r' << 16) | ('k' << 24),

        /// <summary>
        /// Underground - Red Stones ('Grds').
        /// </summary>
        G_RedStones = ('G' << 0) | ('r' << 8) | ('d' << 16) | ('s' << 24),

        /// <summary>
        /// Underground - Ice Chunks ('Glvc').
        /// </summary>
        G_IceChunks = ('G' << 0) | ('l' << 8) | ('v' << 16) | ('c' << 24),

        /// <summary>
        /// Underground - Ice ('Glav').
        /// </summary>
        G_Ice = ('G' << 0) | ('l' << 8) | ('a' << 16) | ('v' << 24),

        /// <summary>
        /// Underground - Dark Rocks ('Gdkr').
        /// </summary>
        G_DarkRocks = ('G' << 0) | ('d' << 8) | ('k' << 16) | ('r' << 24),

        /// <summary>
        /// Underground - Grey Stones ('Ggrs').
        /// </summary>
        G_GreyStones = ('G' << 0) | ('g' << 8) | ('r' << 16) | ('s' << 24),

        /// <summary>
        /// Underground - Square Tiles ('Gsqd').
        /// </summary>
        G_SquareTiles = ('G' << 0) | ('s' << 8) | ('q' << 16) | ('d' << 24),

        /// <summary>
        /// Underground - Dirt Cliff ('cGc2').
        /// </summary>
        G_DirtCliff = ('c' << 0) | ('G' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Underground - Square Tiles Cliff ('cGc1').
        /// </summary>
        G_SquareTilesCliff = ('c' << 0) | ('G' << 8) | ('c' << 16) | ('1' << 24),

        /// ================================= \\\
        /// <see cref="Tileset.SunkenRuins"/> \\\
        /// ================================= \\\

        /// <summary>
        /// Sunken Ruins - Dirt ('Zdrt').
        /// </summary>
        Z_Dirt = ('Z' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Sunken Ruins - Rough Dirt ('Zdtr').
        /// </summary>
        Z_RoughDirt = ('Z' << 0) | ('d' << 8) | ('t' << 16) | ('r' << 24),

        /// <summary>
        /// Sunken Ruins - Grassy Dirt ('Zdrg').
        /// </summary>
        Z_GrassyDirt = ('Z' << 0) | ('d' << 8) | ('r' << 16) | ('g' << 24),

        /// <summary>
        /// Sunken Ruins - Small Bricks ('Zbks').
        /// </summary>
        Z_SmallBricks = ('Z' << 0) | ('b' << 8) | ('k' << 16) | ('s' << 24),

        /// <summary>
        /// Sunken Ruins - Sand ('Zsan').
        /// </summary>
        Z_Sand = ('Z' << 0) | ('s' << 8) | ('a' << 16) | ('n' << 24),

        /// <summary>
        /// Sunken Ruins - Large Bricks ('Zbkl').
        /// </summary>
        Z_LargeBricks = ('Z' << 0) | ('b' << 8) | ('k' << 16) | ('l' << 24),

        /// <summary>
        /// Sunken Ruins - Round Tiles ('Ztil').
        /// </summary>
        Z_RoundTiles = ('Z' << 0) | ('t' << 8) | ('i' << 16) | ('l' << 24),

        /// <summary>
        /// Sunken Ruins - Grass ('Zgrs').
        /// </summary>
        Z_Grass = ('Z' << 0) | ('g' << 8) | ('r' << 16) | ('s' << 24),

        /// <summary>
        /// Sunken Ruins - Dark Grass ('Zvin').
        /// </summary>
        Z_DarkGrass = ('Z' << 0) | ('v' << 8) | ('i' << 16) | ('n' << 24),

        /// <summary>
        /// Sunken Ruins - Dirt Cliff ('cZc2').
        /// </summary>
        Z_DirtCliff = ('c' << 0) | ('Z' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Sunken Ruins - Large Bricks Cliff ('cZc1').
        /// </summary>
        Z_LargeBricksCliff = ('c' << 0) | ('Z' << 8) | ('c' << 16) | ('1' << 24),

        /// ===================================== \\\
        /// <see cref="Tileset.IcecrownGlacier"/> \\\
        /// ===================================== \\\

        /// <summary>
        /// Icecrown Glacier - Dirt ('Idrt').
        /// </summary>
        I_Dirt = ('I' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Icecrown Glacier - Rough Dirt ('Idtr').
        /// </summary>
        I_RoughDirt = ('I' << 0) | ('d' << 8) | ('t' << 16) | ('r' << 24),

        /// <summary>
        /// Icecrown Glacier - Dark Ice ('Idki').
        /// </summary>
        I_DarkIce = ('I' << 0) | ('d' << 8) | ('k' << 16) | ('i' << 24),

        /// <summary>
        /// Icecrown Glacier - Black Bricks ('Ibkb').
        /// </summary>
        I_BlackBricks = ('I' << 0) | ('b' << 8) | ('k' << 16) | ('b' << 24),

        /// <summary>
        /// Icecrown Glacier - Rune Bricks ('Irbk').
        /// </summary>
        I_RuneBricks = ('I' << 0) | ('r' << 8) | ('b' << 16) | ('k' << 24),

        /// <summary>
        /// Icecrown Glacier - Tiled Bricks ('Itbk').
        /// </summary>
        I_TiledBricks = ('I' << 0) | ('t' << 8) | ('b' << 16) | ('k' << 24),

        /// <summary>
        /// Icecrown Glacier - Ice ('Iice').
        /// </summary>
        I_Ice = ('I' << 0) | ('i' << 8) | ('c' << 16) | ('e' << 24),

        /// <summary>
        /// Icecrown Glacier - Black Squares ('Ibsq').
        /// </summary>
        I_BlackSquares = ('I' << 0) | ('b' << 8) | ('s' << 16) | ('q' << 24),

        /// <summary>
        /// Icecrown Glacier - Snow ('Isnw').
        /// </summary>
        I_Snow = ('I' << 0) | ('s' << 8) | ('n' << 16) | ('w' << 24),

        /// <summary>
        /// Icecrown Glacier - Rune Bricks Cliff ('cIc2').
        /// </summary>
        I_RuneBricksCliff = ('c' << 0) | ('I' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Icecrown Glacier - Snow Cliff ('cIc1').
        /// </summary>
        I_SnowCliff = ('c' << 0) | ('I' << 8) | ('c' << 16) | ('1' << 24),

        /// ============================= \\\
        /// <see cref="Tileset.Outland"/> \\\
        /// ============================= \\\

        /// <summary>
        /// Outland - Dirt ('Odrt').
        /// </summary>
        O_Dirt = ('O' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Outland - Light Dirt ('Odtr').
        /// </summary>
        O_LightDirt = ('O' << 0) | ('d' << 8) | ('t' << 16) | ('r' << 24),

        /// <summary>
        /// Outland - Rough Dirt ('Osmb').
        /// </summary>
        O_RoughDirt = ('O' << 0) | ('s' << 8) | ('m' << 16) | ('b' << 24),

        /// <summary>
        /// Outland - Cracked Dirt ('Ofst').
        /// </summary>
        O_CrackedDirt = ('O' << 0) | ('f' << 8) | ('s' << 16) | ('t' << 24),

        /// <summary>
        /// Outland - Flat Stones ('Olgb').
        /// </summary>
        O_FlatStones = ('O' << 0) | ('l' << 8) | ('g' << 16) | ('b' << 24),

        /// <summary>
        /// Outland - Rock ('Orok').
        /// </summary>
        O_Rock = ('O' << 0) | ('r' << 8) | ('o' << 16) | ('k' << 24),

        /// <summary>
        /// Outland - Light Flat Stones ('Ofsl').
        /// </summary>
        O_LightFlatStones = ('O' << 0) | ('f' << 8) | ('s' << 16) | ('l' << 24),

        /// <summary>
        /// Outland - Abyss ('Oaby').
        /// </summary>
        O_Abyss = ('O' << 0) | ('a' << 8) | ('b' << 16) | ('y' << 24),

        /// <summary>
        /// Outland - Rough Dirt Cliff ('cOc2').
        /// </summary>
        O_RoughDirtCliff = ('c' << 0) | ('O' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Outland - Abyss Cliff ('cOc1').
        /// </summary>
        O_AbyssCliff = ('c' << 0) | ('O' << 8) | ('c' << 16) | ('1' << 24),

        /// ================================== \\\
        /// <see cref="Tileset.BlackCitadel"/> \\\
        /// ================================== \\\

        /// <summary>
        /// Black Citadel - Dirt ('Kdrt').
        /// </summary>
        K_Dirt = ('K' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Black Citadel - Light Dirt ('Kfsl').
        /// </summary>
        K_LightDirt = ('K' << 0) | ('f' << 8) | ('s' << 16) | ('l' << 24),

        /// <summary>
        /// Black Citadel - Rough Dirt ('Kdtr').
        /// </summary>
        K_RoughDirt = ('K' << 0) | ('d' << 8) | ('t' << 16) | ('r' << 24),

        /// <summary>
        /// Black Citadel - Flat Stones ('Kfst').
        /// </summary>
        K_FlatStones = ('K' << 0) | ('f' << 8) | ('s' << 16) | ('t' << 24),

        /// <summary>
        /// Black Citadel - Small Bricks ('Ksmb').
        /// </summary>
        K_SmallBricks = ('K' << 0) | ('s' << 8) | ('m' << 16) | ('b' << 24),

        /// <summary>
        /// Black Citadel - Large Bricks ('Klgb').
        /// </summary>
        K_LargeBricks = ('K' << 0) | ('l' << 8) | ('g' << 16) | ('b' << 24),

        /// <summary>
        /// Black Citadel - Square Tiles ('Ksqt').
        /// </summary>
        K_SquareTiles = ('K' << 0) | ('s' << 8) | ('q' << 16) | ('t' << 24),

        /// <summary>
        /// Black Citadel - Dark Tiles ('Kdkt').
        /// </summary>
        K_DarkTiles = ('K' << 0) | ('d' << 8) | ('k' << 16) | ('t' << 24),

        /// <summary>
        /// Black Citadel - Dark Tiles Cliff ('cKc2').
        /// </summary>
        K_DarkTilesCliff = ('c' << 0) | ('K' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Black Citadel - Dirt Cliff ('cKc1').
        /// </summary>
        K_DirtCliff = ('c' << 0) | ('K' << 8) | ('c' << 16) | ('1' << 24),

        /// ================================== \\\
        /// <see cref="Tileset.DalaranRuins"/> \\\
        /// ================================== \\\

        /// <summary>
        /// Dalaran Ruins - Dirt ('Jdrt').
        /// </summary>
        J_Dirt = ('J' << 0) | ('d' << 8) | ('r' << 16) | ('t' << 24),

        /// <summary>
        /// Dalaran Ruins - Rough Dirt ('Jdtr').
        /// </summary>
        J_RoughDirt = ('J' << 0) | ('d' << 8) | ('t' << 16) | ('r' << 24),

        /// <summary>
        /// Dalaran Ruins - Black Marble ('Jblm').
        /// </summary>
        J_BlackMarble = ('J' << 0) | ('b' << 8) | ('l' << 16) | ('m' << 24),

        /// <summary>
        /// Dalaran Ruins - Brick Tiles ('Jbtl').
        /// </summary>
        J_BrickTiles = ('J' << 0) | ('b' << 8) | ('t' << 16) | ('l' << 24),

        /// <summary>
        /// Dalaran Ruins - Square Tiles ('Jsqd').
        /// </summary>
        J_SquareTiles = ('J' << 0) | ('s' << 8) | ('q' << 16) | ('d' << 24),

        /// <summary>
        /// Dalaran Ruins - Round Tiles ('Jrtl').
        /// </summary>
        J_RoundTiles = ('J' << 0) | ('r' << 8) | ('t' << 16) | ('l' << 24),

        /// <summary>
        /// Dalaran Ruins - Grass ('Jgsb').
        /// </summary>
        J_Grass = ('J' << 0) | ('g' << 8) | ('s' << 16) | ('b' << 24),

        /// <summary>
        /// Dalaran Ruins - Trim Grass ('Jhdg').
        /// </summary>
        J_TrimGrass = ('J' << 0) | ('h' << 8) | ('d' << 16) | ('g' << 24),

        /// <summary>
        /// Dalaran Ruins - White Marble ('Jwmb').
        /// </summary>
        J_WhiteMarble = ('J' << 0) | ('w' << 8) | ('m' << 16) | ('b' << 24),

        /// <summary>
        /// Dalaran Ruins - Dirt Cliff ('cJc2').
        /// </summary>
        J_DirtCliff = ('c' << 0) | ('J' << 8) | ('c' << 16) | ('2' << 24),

        /// <summary>
        /// Dalaran Ruins - Square Tiles Cliff ('cJc1').
        /// </summary>
        J_SquareTilesCliff = ('c' << 0) | ('J' << 8) | ('c' << 16) | ('1' << 24),
    }
}