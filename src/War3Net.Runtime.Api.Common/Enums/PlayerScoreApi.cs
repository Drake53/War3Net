// ------------------------------------------------------------------------------
// <copyright file="PlayerScoreApi.cs" company="Drake53">
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
    public static class PlayerScoreApi
    {
        public static readonly PlayerScore PLAYER_SCORE_UNITS_TRAINED = ConvertPlayerScore((int)PlayerScore.Type.UnitsTrained);
        public static readonly PlayerScore PLAYER_SCORE_UNITS_KILLED = ConvertPlayerScore((int)PlayerScore.Type.UnitsKilled);
        public static readonly PlayerScore PLAYER_SCORE_STRUCT_BUILT = ConvertPlayerScore((int)PlayerScore.Type.StructuresBuilt);
        public static readonly PlayerScore PLAYER_SCORE_STRUCT_RAZED = ConvertPlayerScore((int)PlayerScore.Type.StructuresRazed);
        public static readonly PlayerScore PLAYER_SCORE_TECH_PERCENT = ConvertPlayerScore((int)PlayerScore.Type.TechPercent);
        public static readonly PlayerScore PLAYER_SCORE_FOOD_MAXPROD = ConvertPlayerScore((int)PlayerScore.Type.FoodMaxProduced);
        public static readonly PlayerScore PLAYER_SCORE_FOOD_MAXUSED = ConvertPlayerScore((int)PlayerScore.Type.FoodMaxUsed);
        public static readonly PlayerScore PLAYER_SCORE_HEROES_KILLED = ConvertPlayerScore((int)PlayerScore.Type.HeroesKilled);
        public static readonly PlayerScore PLAYER_SCORE_ITEMS_GAINED = ConvertPlayerScore((int)PlayerScore.Type.ItemsGained);
        public static readonly PlayerScore PLAYER_SCORE_MERCS_HIRED = ConvertPlayerScore((int)PlayerScore.Type.MercenariesHired);
        public static readonly PlayerScore PLAYER_SCORE_GOLD_MINED_TOTAL = ConvertPlayerScore((int)PlayerScore.Type.GoldMinedTotal);
        public static readonly PlayerScore PLAYER_SCORE_GOLD_MINED_UPKEEP = ConvertPlayerScore((int)PlayerScore.Type.GoldMinedUpkeep);
        public static readonly PlayerScore PLAYER_SCORE_GOLD_LOST_UPKEEP = ConvertPlayerScore((int)PlayerScore.Type.GoldLostUpkeep);
        public static readonly PlayerScore PLAYER_SCORE_GOLD_LOST_TAX = ConvertPlayerScore((int)PlayerScore.Type.GoldLostTax);
        public static readonly PlayerScore PLAYER_SCORE_GOLD_GIVEN = ConvertPlayerScore((int)PlayerScore.Type.GoldGiven);
        public static readonly PlayerScore PLAYER_SCORE_GOLD_RECEIVED = ConvertPlayerScore((int)PlayerScore.Type.GoldReceived);
        public static readonly PlayerScore PLAYER_SCORE_LUMBER_TOTAL = ConvertPlayerScore((int)PlayerScore.Type.LumberTotal);
        public static readonly PlayerScore PLAYER_SCORE_LUMBER_LOST_UPKEEP = ConvertPlayerScore((int)PlayerScore.Type.LumberLostUpkeep);
        public static readonly PlayerScore PLAYER_SCORE_LUMBER_LOST_TAX = ConvertPlayerScore((int)PlayerScore.Type.LumberLostTax);
        public static readonly PlayerScore PLAYER_SCORE_LUMBER_GIVEN = ConvertPlayerScore((int)PlayerScore.Type.LumberGiven);
        public static readonly PlayerScore PLAYER_SCORE_LUMBER_RECEIVED = ConvertPlayerScore((int)PlayerScore.Type.LumberReceived);
        public static readonly PlayerScore PLAYER_SCORE_UNIT_TOTAL = ConvertPlayerScore((int)PlayerScore.Type.UnitTotal);
        public static readonly PlayerScore PLAYER_SCORE_HERO_TOTAL = ConvertPlayerScore((int)PlayerScore.Type.HeroTotal);
        public static readonly PlayerScore PLAYER_SCORE_RESOURCE_TOTAL = ConvertPlayerScore((int)PlayerScore.Type.ResourceTotal);
        public static readonly PlayerScore PLAYER_SCORE_TOTAL = ConvertPlayerScore((int)PlayerScore.Type.Total);

        public static PlayerScore ConvertPlayerScore(int i)
        {
            return PlayerScore.GetPlayerScore(i);
        }
    }
}