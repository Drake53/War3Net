// ------------------------------------------------------------------------------
// <copyright file="RacePreferenceApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums;

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class RacePreferenceApi
    {
        public static readonly RacePreference RACE_PREF_HUMAN = ConvertRacePref((int)RacePreference.Type.Human);
        public static readonly RacePreference RACE_PREF_ORC = ConvertRacePref((int)RacePreference.Type.Orc);
        public static readonly RacePreference RACE_PREF_NIGHTELF = ConvertRacePref((int)RacePreference.Type.NightElf);
        public static readonly RacePreference RACE_PREF_UNDEAD = ConvertRacePref((int)RacePreference.Type.Undead);
        public static readonly RacePreference RACE_PREF_DEMON = ConvertRacePref((int)RacePreference.Type.Demon);
        public static readonly RacePreference RACE_PREF_RANDOM = ConvertRacePref((int)RacePreference.Type.Random);
        public static readonly RacePreference RACE_PREF_USER_SELECTABLE = ConvertRacePref((int)RacePreference.Type.UserSelectable);

        public static RacePreference ConvertRacePref(int i)
        {
            return RacePreference.GetRacePreference(i);
        }
    }
}