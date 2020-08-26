// ------------------------------------------------------------------------------
// <copyright file="UnitWeaponBooleanField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Object
{
    public sealed class UnitWeaponBooleanField
    {
        private static readonly Dictionary<int, UnitWeaponBooleanField> _fields = GetTypes().ToDictionary(t => (int)t, t => new UnitWeaponBooleanField(t));

        private readonly Type _type;

        private UnitWeaponBooleanField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            ATTACK_SHOW_UI = 1970763057,
            ATTACKS_ENABLED = 1969317230,
            ATTACK_PROJECTILE_HOMING_ENABLED = 1970104369,
        }

        public static UnitWeaponBooleanField GetUnitWeaponBooleanField(int i)
        {
            if (!_fields.TryGetValue(i, out var unitWeaponBooleanField))
            {
                unitWeaponBooleanField = new UnitWeaponBooleanField((Type)i);
                _fields.Add(i, unitWeaponBooleanField);
            }

            return unitWeaponBooleanField;
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