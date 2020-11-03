// ------------------------------------------------------------------------------
// <copyright file="AbilityStringLevelArrayField.cs" company="Drake53">
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
    public sealed class AbilityStringLevelArrayField : Handle
    {
        private static readonly Dictionary<int, AbilityStringLevelArrayField> _fields = GetTypes().ToDictionary(t => (int)t, t => new AbilityStringLevelArrayField(t));

        private readonly Type _type;

        private AbilityStringLevelArrayField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
        }

        public static implicit operator Type(AbilityStringLevelArrayField abilityStringLevelArrayField) => abilityStringLevelArrayField._type;

        public static explicit operator int(AbilityStringLevelArrayField abilityStringLevelArrayField) => (int)abilityStringLevelArrayField._type;

        public static AbilityStringLevelArrayField GetAbilityStringLevelArrayField(int i)
        {
            if (!_fields.TryGetValue(i, out var abilityStringLevelArrayField))
            {
                abilityStringLevelArrayField = new AbilityStringLevelArrayField((Type)i);
                _fields.Add(i, abilityStringLevelArrayField);
            }

            return abilityStringLevelArrayField;
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