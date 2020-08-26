// ------------------------------------------------------------------------------
// <copyright file="GameSpeed.cs" company="Drake53">
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
    public sealed class GameSpeed
    {
        private static readonly Dictionary<int, GameSpeed> _speeds = GetTypes().ToDictionary(t => (int)t, t => new GameSpeed(t));

        private readonly Type _type;

        private GameSpeed(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Slowest = 0,
            Slow = 1,
            Normal = 2,
            Fast = 3,
            Fastest = 4,
        }

        public static GameSpeed GetGameSpeed(int i)
        {
            if (!_speeds.TryGetValue(i, out var gameSpeed))
            {
                gameSpeed = new GameSpeed((Type)i);
                _speeds.Add(i, gameSpeed);
            }

            return gameSpeed;
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