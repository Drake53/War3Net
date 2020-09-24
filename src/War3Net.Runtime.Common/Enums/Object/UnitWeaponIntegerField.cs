// ------------------------------------------------------------------------------
// <copyright file="UnitWeaponIntegerField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Object
{
    public sealed class UnitWeaponIntegerField
    {
        private static readonly Dictionary<int, UnitWeaponIntegerField> _fields = GetTypes().ToDictionary(t => (int)t, t => new UnitWeaponIntegerField(t));

        private readonly Type _type;

        private UnitWeaponIntegerField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            ATTACK_DAMAGE_NUMBER_OF_DICE = 1969303908,
            ATTACK_DAMAGE_BASE = 1969303906,
            ATTACK_DAMAGE_SIDES_PER_DIE = 1969303923,
            ATTACK_MAXIMUM_NUMBER_OF_TARGETS = 1970561841,
            ATTACK_ATTACK_TYPE = 1969303924,
            ATTACK_WEAPON_SOUND = 1969451825,
            ATTACK_AREA_OF_EFFECT_TARGETS = 1969303920,
            ATTACK_TARGETS_ALLOWED = 1969303911,
        }

        public static UnitWeaponIntegerField GetUnitWeaponIntegerField(int i)
        {
            if (!_fields.TryGetValue(i, out var unitWeaponIntegerField))
            {
                unitWeaponIntegerField = new UnitWeaponIntegerField((Type)i);
                _fields.Add(i, unitWeaponIntegerField);
            }

            return unitWeaponIntegerField;
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