// ------------------------------------------------------------------------------
// <copyright file="ConstantsApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Runtime.Api.Common.Core
{
    public static class ConstantsApi
    {
        public const bool FALSE = false;
        public const bool TRUE = true;
        public const int JASS_MAX_ARRAY_SIZE = 32768;
        public static readonly int PLAYER_NEUTRAL_PASSIVE = PlayerApi.GetPlayerNeutralPassive();
        public static readonly int PLAYER_NEUTRAL_AGGRESSIVE = PlayerApi.GetPlayerNeutralAggressive();
    }
}