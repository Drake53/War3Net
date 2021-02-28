// ------------------------------------------------------------------------------
// <copyright file="PlayerRaceExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Info;

namespace War3Net.Build.Extensions
{
    public static class PlayerRaceExtensions
    {
        public static string GetVariableName(this PlayerRace playerRace)
        {
            return playerRace switch
            {
                PlayerRace.Human => RacePreferenceName.Human,
                PlayerRace.Orc => RacePreferenceName.Orc,
                PlayerRace.NightElf => RacePreferenceName.NightElf,
                PlayerRace.Undead => RacePreferenceName.Undead,
                // PlayerRace.Demon => RacePreferenceName.Demon,
                // _ => RacePreferenceName.UserSelectable,
                _ => RacePreferenceName.Random,
            };
        }

        private class RacePreferenceName
        {
            internal const string Human = "RACE_PREF_HUMAN";
            internal const string Orc = "RACE_PREF_ORC";
            internal const string NightElf = "RACE_PREF_NIGHTELF";
            internal const string Undead = "RACE_PREF_UNDEAD";
            internal const string Demon = "RACE_PREF_DEMON";
            internal const string UserSelectable = "RACE_PREF_USER_SELECTABLE";
            internal const string Random = "RACE_PREF_RANDOM";
        }
    }
}