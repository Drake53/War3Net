// ------------------------------------------------------------------------------
// <copyright file="GameSpeed.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.Enums
{
    public sealed class GameSpeed : Handle
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

        public static implicit operator Type(GameSpeed gameSpeed) => gameSpeed._type;

        public static explicit operator int(GameSpeed gameSpeed) => (int)gameSpeed._type;

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