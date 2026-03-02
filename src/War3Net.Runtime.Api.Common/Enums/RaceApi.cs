#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class RaceApi
    {
        public static readonly Race RACE_HUMAN = ConvertRace((int)Race.Type.Human);
        public static readonly Race RACE_ORC = ConvertRace((int)Race.Type.Orc);
        public static readonly Race RACE_UNDEAD = ConvertRace((int)Race.Type.Undead);
        public static readonly Race RACE_NIGHTELF = ConvertRace((int)Race.Type.NightElf);
        public static readonly Race RACE_DEMON = ConvertRace((int)Race.Type.Demon);
        public static readonly Race RACE_OTHER = ConvertRace((int)Race.Type.Other);

        public static Race ConvertRace(int i)
        {
            return Race.GetRace(i);
        }
    }
}