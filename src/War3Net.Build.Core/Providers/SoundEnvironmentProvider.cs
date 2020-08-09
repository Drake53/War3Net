// ------------------------------------------------------------------------------
// <copyright file="SoundEnvironmentProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Audio;
using War3Net.Build.Common;

namespace War3Net.Build.Providers
{
    public static class SoundEnvironmentProvider
    {
        public static string GetSoundEnvironment(SoundEnvironment soundEnvironment)
        {
            switch (soundEnvironment)
            {
                case SoundEnvironment.Mountains: return "Default";
                case SoundEnvironment.Lake: return "lake";
                case SoundEnvironment.Psychotic: return "psychotic";
                case SoundEnvironment.Dungeon: return "Dungeon";

                default: return "Default";
            }
        }

        public static string GetAmbientDaySound(Tileset tileset)
        {
            return $"{GetAmbientSound(tileset)}Day";
        }

        public static string GetAmbientNightSound(Tileset tileset)
        {
            return $"{GetAmbientSound(tileset)}Night";
        }

        private static string GetAmbientSound(Tileset tileset)
        {
            switch (tileset)
            {
                case Tileset.Ashenvale: return "Ashenvale";
                case Tileset.Barrens: return "Barrens";
                case Tileset.BlackCitadel: return "BlackCitadel";
                case Tileset.Cityscape: return "CityScape";
                case Tileset.Dalaran: return "Dalaran";
                case Tileset.DalaranRuins: return "DalaranRuins";
                case Tileset.Dungeon: return "Dungeon";
                case Tileset.Felwood: return "Felwood";
                case Tileset.IcecrownGlacier: return "IceCrown";
                case Tileset.LordaeronFall: return "LordaeronFall";
                case Tileset.LordaeronSummer: return "LordaeronSummer";
                case Tileset.LordaeronWinter: return "LordaeronWinter";
                case Tileset.Northrend: return "Northrend";
                case Tileset.Outland: return "BlackCitadel";
                case Tileset.SunkenRuins: return "SunkenRuins";
                case Tileset.Underground: return "Dungeon";
                case Tileset.Village: return "Village";
                case Tileset.VillageFall: return "VillageFall";

                default: return string.Empty;
            }
        }
    }
}