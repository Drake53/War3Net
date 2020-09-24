// ------------------------------------------------------------------------------
// <copyright file="UnitWeaponStringField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Object
{
    public sealed class UnitWeaponStringField
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