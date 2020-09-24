// ------------------------------------------------------------------------------
// <copyright file="PlayerScore.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class PlayerScore
    {
        private static readonly Dictionary<int, PlayerScore> _scores = GetTypes().ToDictionary(t => (int)t, t => new PlayerScore(t));

        private readonly Type _type;

        private PlayerScore(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            UnitsTrained = 0,
            UnitsKilled = 1,
            StructuresBuilt = 2,
            StructuresRazed = 3,
            TechPercent = 4,
            FoodMaxProduced = 5,
            FoodMaxUsed = 6,
            HeroesKilled = 7,
            ItemsGained = 8,
            MercenariesHired = 9,
            GoldMinedTotal = 10,
            GoldMinedUpkeep = 11,
            GoldLostUpkeep = 12,
            GoldLostTax = 13,
            GoldGiven = 14,
            GoldReceived = 15,
            LumberTotal = 16,
            LumberLostUpkeep = 17,
            LumberLostTax = 18,
            LumberGiven = 19,
            LumberReceived = 20,
            UnitTotal = 21,
            HeroTotal = 22,
            ResourceTotal = 23,
            Total = 24,
        }

        public static PlayerScore GetPlayerScore(int i)
        {
            if (!_scores.TryGetValue(i, out var playerScore))
            {
                playerScore = new PlayerScore((Type)i);
                _scores.Add(i, playerScore);
            }

            return playerScore;
        }

        private static IEnumerable<Type> GetTypes()
        {
            foreach (Type type in Enum.GetValues(typeof(Type)))
            {
                yield return type;
            }
        }
    }
}