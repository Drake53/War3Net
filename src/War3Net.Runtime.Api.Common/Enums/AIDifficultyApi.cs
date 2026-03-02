#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class AIDifficultyApi
    {
        public static readonly AIDifficulty AI_DIFFICULTY_NEWBIE = ConvertAIDifficulty((int)AIDifficulty.Type.Newbie);
        public static readonly AIDifficulty AI_DIFFICULTY_NORMAL = ConvertAIDifficulty((int)AIDifficulty.Type.Normal);
        public static readonly AIDifficulty AI_DIFFICULTY_INSANE = ConvertAIDifficulty((int)AIDifficulty.Type.Insane);

        public static AIDifficulty ConvertAIDifficulty(int i)
        {
            return AIDifficulty.GetAIDifficulty(i);
        }
    }
}