// ------------------------------------------------------------------------------
// <copyright file="UnitCategory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class UnitCategory
    {
        private static readonly Dictionary<int, UnitCategory> _categories = GetTypes().ToDictionary(t => (int)t, t => new UnitCategory(t));

        private readonly Type _type;

        private UnitCategory(Type type)
        {
            _type = type;
        }

        [Flags]
        public enum Type
        {
            Giant = 1 << 0,
            Undead = 1 << 1,
            Summoned = 1 << 2,
            Mechanical = 1 << 3,
            Peon = 1 << 4,
            Sapper = 1 << 5,
            Townhall = 1 << 6,
            Ancient = 1 << 7,
            Neutral = 1 << 8,
            Ward = 1 << 9,
            StandOn = 1 << 10,
            Tauren = 1 << 11,
        }

        public static UnitCategory GetUnitCategory(int i)
        {
            if (!_categories.TryGetValue(i, out var unitCategory))
            {
                unitCategory = new UnitCategory((Type)i);
                _categories.Add(i, unitCategory);
            }

            return unitCategory;
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