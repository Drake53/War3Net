// ------------------------------------------------------------------------------
// <copyright file="AIDifficultyApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums;

namespace War3Net.Runtime.Common.Api.Enums
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