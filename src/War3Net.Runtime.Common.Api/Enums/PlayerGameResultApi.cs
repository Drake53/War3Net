// ------------------------------------------------------------------------------
// <copyright file="PlayerGameResultApi.cs" company="Drake53">
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
    public static class PlayerGameResultApi
    {
        public static readonly PlayerGameResult PLAYER_GAME_RESULT_VICTORY = ConvertPlayerGameResult((int)PlayerGameResult.Type.Victory);
        public static readonly PlayerGameResult PLAYER_GAME_RESULT_DEFEAT = ConvertPlayerGameResult((int)PlayerGameResult.Type.Defeat);
        public static readonly PlayerGameResult PLAYER_GAME_RESULT_TIE = ConvertPlayerGameResult((int)PlayerGameResult.Type.Tie);
        public static readonly PlayerGameResult PLAYER_GAME_RESULT_NEUTRAL = ConvertPlayerGameResult((int)PlayerGameResult.Type.Neutral);

        public static PlayerGameResult ConvertPlayerGameResult(int i)
        {
            return PlayerGameResult.GetPlayerGameResult(i);
        }
    }
}