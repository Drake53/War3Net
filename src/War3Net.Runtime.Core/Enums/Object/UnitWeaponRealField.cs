// ------------------------------------------------------------------------------
// <copyright file="UnitWeaponRealField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.Enums.Object
{
    public sealed class UnitWeaponRealField : Handle
    {
        private static readonly Dictionary<int, UnitWeaponRealField> _fields = GetTypes().ToDictionary(t => (int)t, t => new UnitWeaponRealField(t));

        private readonly Type _type;

        private UnitWeaponRealField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            ATTACK_BACKSWING_POINT = 1969386289,
            ATTACK_DAMAGE_POINT = 1969516593,
            ATTACK_BASE_COOLDOWN = 1969303907,
            ATTACK_DAMAGE_LOSS_FACTOR = 1969515569,
            ATTACK_DAMAGE_FACTOR_MEDIUM = 1969775665,
            ATTACK_DAMAGE_FACTOR_SMALL = 1970365489,
            ATTACK_DAMAGE_SPILL_DISTANCE = 1970496561,
            ATTACK_DAMAGE_SPILL_RADIUS = 1970500145,
            ATTACK_PROJECTILE_SPEED = 1969303930,
            ATTACK_PROJECTILE_ARC = 1970102577,
            ATTACK_AREA_OF_EFFECT_FULL_DAMAGE = 1969303910,
            ATTACK_AREA_OF_EFFECT_MEDIUM_DAMAGE = 1969303912,
            ATTACK_AREA_OF_EFFECT_SMALL_DAMAGE = 1969303921,
            ATTACK_RANGE = 1969303922,
        }

        public static implicit operator Type(UnitWeaponRealField unitWeaponRealField) => unitWeaponRealField._type;

        public static explicit operator int(UnitWeaponRealField unitWeaponRealField) => (int)unitWeaponRealField._type;

        public static UnitWeaponRealField GetUnitWeaponRealField(int i)
        {
            if (!_fields.TryGetValue(i, out var unitWeaponRealField))
            {
                unitWeaponRealField = new UnitWeaponRealField((Type)i);
                _fields.Add(i, unitWeaponRealField);
            }

            return unitWeaponRealField;
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