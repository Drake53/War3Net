// ------------------------------------------------------------------------------
// <copyright file="DamageType.cs" company="Drake53">
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
    public sealed class DamageType
    {
        private static readonly Dictionary<int, DamageType> _types = GetTypes().ToDictionary(t => (int)t, t => new DamageType(t));

        private readonly Type _type;

        private DamageType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Unknown = 0,
            Normal = 4,
            Enhanced = 5,
            Fire = 8,
            Cold = 9,
            Lightning = 10,
            Poison = 11,
            Disease = 12,
            Divine = 13,
            Magic = 14,
            Sonic = 15,
            Acid = 16,
            Force = 17,
            Death = 18,
            Mind = 19,
            Plant = 20,
            Defensive = 21,
            Demolition = 22,
            SlowPoison = 23,
            SpiritLink = 24,
            ShadowStrike = 25,
            Universal = 26,
        }

        public static DamageType GetDamageType(int i)
        {
            if (!_types.TryGetValue(i, out var damageType))
            {
                damageType = new DamageType((Type)i);
                _types.Add(i, damageType);
            }

            return damageType;
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