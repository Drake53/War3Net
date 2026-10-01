namespace War3Net.Build.Common
{
    public enum Tileset : byte
    {
        Unspecified = 0,
        Ashenvale = (byte)'A',
        Barrens = (byte)'B',
        BlackCitadel = (byte)'K',
        Cityscape = (byte)'Y',

        /// <summary>Introduced in patch 3.0.0.</summary>
        CityscapeRuins = (byte)'P',
        Dalaran = (byte)'X',
        DalaranRuins = (byte)'J',
        Dungeon = (byte)'D',
        Felwood = (byte)'C',
        IcecrownGlacier = (byte)'I',

        /// <summary>Introduced in patch 3.0.0.</summary>
        LordaeronCapital = (byte)'e',

        /// <summary>Introduced in patch 3.0.0.</summary>
        LordaeronCapitalRuins = (byte)'R',
        LordaeronFall = (byte)'F',
        LordaeronSummer = (byte)'L',
        LordaeronWinter = (byte)'W',

        /// <summary>Introduced in patch 3.0.0.</summary>
        Misc = (byte)'M',
        Northrend = (byte)'N',
        Outland = (byte)'O',
        SunkenRuins = (byte)'Z',

        /// <summary>Introduced in patch 3.0.0.</summary>
        Undercity = (byte)'u',
        Underground = (byte)'G',
        Village = (byte)'V',
        VillageFall = (byte)'Q',
    }
}