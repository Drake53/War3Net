// ------------------------------------------------------------------------------
// <copyright file="AttackType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class AttackType
    {
        private static readonly Dictionary<int, AttackType> _types = GetTypes().ToDictionary(t => (int)t, t => new AttackType(t));

        private readonly Type _type;

        private AttackType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Normal = 0,
            Melee = 1,
            Pierce = 2,
            Siege = 3,
            Magic = 4,
            Chaos = 5,
            Hero = 6,
        }

        public static AttackType GetAttackType(int i)
        {
            if (!_types.TryGetValue(i, out var attackType))
            {
                attackType = new AttackType((Type)i);
                _types.Add(i, attackType);
            }

            return attackType;
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