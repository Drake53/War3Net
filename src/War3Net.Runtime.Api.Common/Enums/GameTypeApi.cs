// ------------------------------------------------------------------------------
// <copyright file="GameTypeApi.cs" company="Drake53">
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
    public static class GameTypeApi
    {
        public static readonly GameType GAME_TYPE_MELEE = ConvertGameType((int)GameType.Type.Melee);
        public static readonly GameType GAME_TYPE_FFA = ConvertGameType((int)GameType.Type.FFA);
        public static readonly GameType GAME_TYPE_USE_MAP_SETTINGS = ConvertGameType((int)GameType.Type.UseMapSettings);
        public static readonly GameType GAME_TYPE_BLIZ = ConvertGameType((int)GameType.Type.Blizzard);
        public static readonly GameType GAME_TYPE_ONE_ON_ONE = ConvertGameType((int)GameType.Type.OneOnOne);
        public static readonly GameType GAME_TYPE_TWO_TEAM_PLAY = ConvertGameType((int)GameType.Type.TwoTeamPlay);
        public static readonly GameType GAME_TYPE_THREE_TEAM_PLAY = ConvertGameType((int)GameType.Type.ThreeTeamPlay);
        public static readonly GameType GAME_TYPE_FOUR_TEAM_PLAY = ConvertGameType((int)GameType.Type.FourTeamPlay);

        public static GameType ConvertGameType(int i)
        {
            return GameType.GetGameType(i);
        }
    }
}