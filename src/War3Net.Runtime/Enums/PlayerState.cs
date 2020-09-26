// ------------------------------------------------------------------------------
// <copyright file="PlayerState.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Enums
{
    public sealed class PlayerState
    {
        private static readonly Dictionary<int, PlayerState> _states = GetTypes().ToDictionary(t => (int)t, t => new PlayerState(t));

        private readonly Type _type;

        private PlayerState(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            GameResult = 0,

            ResourceGold = 1,
            ResourceLumber = 2,
            ResourceHeroTokens = 3,
            ResourceFoodCap = 4,
            ResourceFoodUsed = 5,
            FoodCapCeiling = 6,

            GivesBounty = 7,
            AlliedVictory = 8,
            Placed = 9,
            ObserverOnDeath = 10,
            Observer = 11,
            Unfollowable = 12,

            GoldUpkeepRate = 13,
            LumberUpkeepRate = 14,

            GoldGathered = 15,
            LumberGathered = 16,

            NoCreepSleep = 25,
        }

        public static PlayerState GetPlayerState(int i)
        {
            if (!_states.TryGetValue(i, out var playerState))
            {
                playerState = new PlayerState((Type)i);
                _states.Add(i, playerState);
            }

            return playerState;
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