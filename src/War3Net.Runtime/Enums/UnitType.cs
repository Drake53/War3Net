// ------------------------------------------------------------------------------
// <copyright file="UnitType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Enums
{
    public sealed class UnitType
    {
        private static readonly Dictionary<int, UnitType> _types = GetTypes().ToDictionary(t => (int)t, t => new UnitType(t));

        private readonly Type _type;

        private UnitType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Hero = 0,
            Dead = 1,
            Structure = 2,

            Flying = 3,
            Ground = 4,

            AttacksFlying = 5,
            AttacksGround = 6,

            MeleeAttacker = 7,
            RangedAttacker = 8,

            Giant = 9,
            Summoned = 10,
            Stunned = 11,
            Plagued = 12,
            Snared = 13,

            Undead = 14,
            Mechanical = 15,
            Peon = 16,
            Sapper = 17,
            Townhall = 18,
            Ancient = 19,

            Tauren = 20,
            Poisoned = 21,
            Polymorphed = 22,
            Sleeping = 23,
            Resistant = 24,
            Ethereal = 25,
            MagicImmune = 26,
        }

        public static UnitType GetUnitType(int i)
        {
            if (!_types.TryGetValue(i, out var unitType))
            {
                unitType = new UnitType((Type)i);
                _types.Add(i, unitType);
            }

            return unitType;
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