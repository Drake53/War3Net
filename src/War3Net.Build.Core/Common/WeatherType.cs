// ------------------------------------------------------------------------------
// <copyright file="WeatherType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Common
{
    // war3.w3mod/terrainart/weather.slk
    public enum WeatherType
    {
        None = 0,
        AshenvaleHeavyRain = ('R' << 0) | ('A' << 8) | ('h' << 16) | ('r' << 24),
        AshenvaleLightRain = ('R' << 0) | ('A' << 8) | ('l' << 16) | ('r' << 24),
        DalaranShield = ('M' << 0) | ('E' << 8) | ('d' << 16) | ('s' << 24),
        DungeonHeavyBlueFog = ('F' << 0) | ('D' << 8) | ('b' << 16) | ('h' << 24),
        DungeonBlueFog = ('F' << 0) | ('D' << 8) | ('b' << 16) | ('l' << 24),
        DungeonHeavyGreenFog = ('F' << 0) | ('D' << 8) | ('g' << 16) | ('h' << 24),
        DungeonGreenFog = ('F' << 0) | ('D' << 8) | ('g' << 16) | ('l' << 24),
        DungeonHeavyRedFog = ('F' << 0) | ('D' << 8) | ('r' << 16) | ('h' << 24),
        DungeonRedFog = ('F' << 0) | ('D' << 8) | ('r' << 16) | ('l' << 24),
        DungeonHeavyWhiteFog = ('F' << 0) | ('D' << 8) | ('w' << 16) | ('h' << 24),
        DungeonWhiteFog = ('F' << 0) | ('D' << 8) | ('w' << 16) | ('l' << 24),
        LordaeronHeavyRain = ('R' << 0) | ('L' << 8) | ('h' << 16) | ('r' << 24),
        LordaeronLightRain = ('R' << 0) | ('L' << 8) | ('l' << 16) | ('r' << 24),
        NorthrendBlizzard = ('S' << 0) | ('N' << 8) | ('b' << 16) | ('s' << 24),
        NorthrendHeavySnow = ('S' << 0) | ('N' << 8) | ('h' << 16) | ('s' << 24),
        NorthrendLightSnow = ('S' << 0) | ('N' << 8) | ('l' << 16) | ('s' << 24),
        RaysOfLight = ('L' << 0) | ('R' << 8) | ('a' << 16) | ('a' << 24),
        RaysOfMoonlight = ('L' << 0) | ('R' << 8) | ('m' << 16) | ('a' << 24),
        Wind = ('W' << 0) | ('N' << 8) | ('c' << 16) | ('w' << 24),
        OutlandWind = ('W' << 0) | ('O' << 8) | ('c' << 16) | ('w' << 24),
        OutlandWindLight = ('W' << 0) | ('O' << 8) | ('l' << 16) | ('w' << 24),
    }
}