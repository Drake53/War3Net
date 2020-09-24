// ------------------------------------------------------------------------------
// <copyright file="GameType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class GameType
    {
        private static readonly Dictionary<int, GameType> _types = GetTypes().ToDictionary(t => (int)t, t => new GameType(t));

        private readonly Type _type;

        private GameType(Type type)
        {
            _type = type;
        }

        [Flags]
        public enum Type
        {
            Melee = 1 << 0,
            FFA = 1 << 1,
            UseMapSettings = 1 << 2,
            Blizzard = 1 << 3,
            OneOnOne = 1 << 4,
            TwoTeamPlay = 1 << 5,
            ThreeTeamPlay = 1 << 6,
            FourTeamPlay = 1 << 7,
        }

        public static GameType GetGameType(int i)
        {
            if (!_types.TryGetValue(i, out var gameType))
            {
                gameType = new GameType((Type)i);
                _types.Add(i, gameType);
            }

            return gameType;
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