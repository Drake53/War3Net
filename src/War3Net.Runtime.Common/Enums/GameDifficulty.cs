// ------------------------------------------------------------------------------
// <copyright file="GameDifficulty.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class GameDifficulty
    {
        private static readonly Dictionary<int, GameDifficulty> _difficulties = GetTypes().ToDictionary(t => (int)t, t => new GameDifficulty(t));

        private readonly Type _type;

        private GameDifficulty(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Easy = 0,
            Normal = 1,
            Hard = 2,
            Insane = 3,
        }

        public static GameDifficulty GetGameDifficulty(int i)
        {
            if (!_difficulties.TryGetValue(i, out var gameDifficulty))
            {
                gameDifficulty = new GameDifficulty((Type)i);
                _difficulties.Add(i, gameDifficulty);
            }

            return gameDifficulty;
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