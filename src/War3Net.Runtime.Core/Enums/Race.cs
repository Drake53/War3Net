// ------------------------------------------------------------------------------
// <copyright file="Race.cs" company="Drake53">
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
    public sealed class Race : Handle
    {
        private static readonly Dictionary<int, Race> _races = GetTypes().ToDictionary(t => (int)t, t => new Race(t));

        private readonly Type _type;

        private Race(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Human = 1,
            Orc = 2,
            Undead = 3,
            NightElf = 4,
            Demon = 5,
            Other = 7,
        }

        public static implicit operator Type(Race race) => race._type;

        public static explicit operator int(Race race) => (int)race._type;

        public static Race GetRace(int i)
        {
            if (!_races.TryGetValue(i, out var race))
            {
                race = new Race((Type)i);
                _races.Add(i, race);
            }

            return race;
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