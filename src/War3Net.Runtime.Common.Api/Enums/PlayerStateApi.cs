// ------------------------------------------------------------------------------
// <copyright file="PlayerStateApi.cs" company="Drake53">
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
    public static class PlayerStateApi
    {
        public static readonly PlayerState PLAYER_STATE_GAME_RESULT = ConvertPlayerState((int)PlayerState.Type.GameResult);

        public static readonly PlayerState PLAYER_STATE_RESOURCE_GOLD = ConvertPlayerState((int)PlayerState.Type.ResourceGold);
        public static readonly PlayerState PLAYER_STATE_RESOURCE_LUMBER = ConvertPlayerState((int)PlayerState.Type.ResourceLumber);
        public static readonly PlayerState PLAYER_STATE_RESOURCE_HERO_TOKENS = ConvertPlayerState((int)PlayerState.Type.ResourceHeroTokens);
        public static readonly PlayerState PLAYER_STATE_RESOURCE_FOOD_CAP = ConvertPlayerState((int)PlayerState.Type.ResourceFoodCap);
        public static readonly PlayerState PLAYER_STATE_RESOURCE_FOOD_USED = ConvertPlayerState((int)PlayerState.Type.ResourceFoodUsed);
        public static readonly PlayerState PLAYER_STATE_FOOD_CAP_CEILING = ConvertPlayerState((int)PlayerState.Type.FoodCapCeiling);

        public static readonly PlayerState PLAYER_STATE_GIVES_BOUNTY = ConvertPlayerState((int)PlayerState.Type.GivesBounty);
        public static readonly PlayerState PLAYER_STATE_ALLIED_VICTORY = ConvertPlayerState((int)PlayerState.Type.AlliedVictory);
        public static readonly PlayerState PLAYER_STATE_PLACED = ConvertPlayerState((int)PlayerState.Type.Placed);
        public static readonly PlayerState PLAYER_STATE_OBSERVER_ON_DEATH = ConvertPlayerState((int)PlayerState.Type.ObserverOnDeath);
        public static readonly PlayerState PLAYER_STATE_OBSERVER = ConvertPlayerState((int)PlayerState.Type.Observer);
        public static readonly PlayerState PLAYER_STATE_UNFOLLOWABLE = ConvertPlayerState((int)PlayerState.Type.Unfollowable);

        public static readonly PlayerState PLAYER_STATE_GOLD_UPKEEP_RATE = ConvertPlayerState((int)PlayerState.Type.GoldUpkeepRate);
        public static readonly PlayerState PLAYER_STATE_LUMBER_UPKEEP_RATE = ConvertPlayerState((int)PlayerState.Type.LumberUpkeepRate);

        public static readonly PlayerState PLAYER_STATE_GOLD_GATHERED = ConvertPlayerState((int)PlayerState.Type.GoldGathered);
        public static readonly PlayerState PLAYER_STATE_LUMBER_GATHERED = ConvertPlayerState((int)PlayerState.Type.LumberGathered);

        public static readonly PlayerState PLAYER_STATE_NO_CREEP_SLEEP = ConvertPlayerState((int)PlayerState.Type.NoCreepSleep);

        public static PlayerState ConvertPlayerState(int i)
        {
            return PlayerState.GetPlayerState(i);
        }
    }
}