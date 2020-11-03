// ------------------------------------------------------------------------------
// <copyright file="AbilityRealLevelArrayField.cs" company="Drake53">
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
    public sealed class AbilityRealLevelArrayField : Handle
    {
        private static readonly Dictionary<int, AbilityRealLevelArrayField> _fields = GetTypes().ToDictionary(t => (int)t, t => new AbilityRealLevelArrayField(t));

        private readonly Type _type;

        private AbilityRealLevelArrayField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
        }

        public static implicit operator Type(AbilityRealLevelArrayField abilityRealLevelArrayField) => abilityRealLevelArrayField._type;

        public static explicit operator int(AbilityRealLevelArrayField abilityRealLevelArrayField) => (int)abilityRealLevelArrayField._type;

        public static AbilityRealLevelArrayField GetAbilityRealLevelArrayField(int i)
        {
            if (!_fields.TryGetValue(i, out var abilityRealLevelArrayField))
            {
                abilityRealLevelArrayField = new AbilityRealLevelArrayField((Type)i);
                _fields.Add(i, abilityRealLevelArrayField);
            }

            return abilityRealLevelArrayField;
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