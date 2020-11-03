// ------------------------------------------------------------------------------
// <copyright file="PlayerColor.cs" company="Drake53">
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
    public sealed class PlayerColor : Handle
    {
        private static readonly Dictionary<int, PlayerColor> _colors = GetTypes().ToDictionary(t => (int)t, t => new PlayerColor(t));

        private readonly Type _type;

        private PlayerColor(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Red = 0,
            Blue = 1,
            Cyan = 2,
            Purple = 3,
            Yellow = 4,
            Orange = 5,
            Green = 6,
            Pink = 7,
            LightGray = 8,
            LightBlue = 9,
            DarkGreen = 10,
            Brown = 11,

            Maroon = 12,
            Navy = 13,
            Turquoise = 14,
            Violet = 15,
            Wheat = 16,
            Peach = 17,
            Mint = 18,
            Lavender = 19,
            Coal = 20,
            Snow = 21,
            Emerald = 22,
            Peanut = 23,

            Black = 24,
        }

        public static implicit operator Type(PlayerColor playerColor) => playerColor._type;

        public static explicit operator int(PlayerColor playerColor) => (int)playerColor._type;

        public static PlayerColor GetPlayerColor(int i)
        {
            if (!_colors.TryGetValue(i, out var playerColor))
            {
                playerColor = new PlayerColor((Type)i);
                _colors.Add(i, playerColor);
            }

            return playerColor;
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