// ------------------------------------------------------------------------------
// <copyright file="UnitWeaponStringField.cs" company="Drake53">
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
    public sealed class UnitWeaponStringField : Handle
    {
        private static readonly Dictionary<int, UnitWeaponStringField> _fields = GetTypes().ToDictionary(t => (int)t, t => new UnitWeaponStringField(t));

        private readonly Type _type;

        private UnitWeaponStringField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            ATTACK_PROJECTILE_ART = 1969303917,
        }

        public static implicit operator Type(UnitWeaponStringField unitWeaponStringField) => unitWeaponStringField._type;

        public static explicit operator int(UnitWeaponStringField unitWeaponStringField) => (int)unitWeaponStringField._type;

        public static UnitWeaponStringField GetUnitWeaponStringField(int i)
        {
            if (!_fields.TryGetValue(i, out var unitWeaponStringField))
            {
                unitWeaponStringField = new UnitWeaponStringField((Type)i);
                _fields.Add(i, unitWeaponStringField);
            }

            return unitWeaponStringField;
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