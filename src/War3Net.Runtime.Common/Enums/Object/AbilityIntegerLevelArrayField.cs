// ------------------------------------------------------------------------------
// <copyright file="AbilityIntegerLevelArrayField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Object
{
    public sealed class AbilityIntegerLevelArrayField
    {
        private static readonly Dictionary<int, AbilityIntegerLevelArrayField> _fields = GetTypes().ToDictionary(t => (int)t, t => new AbilityIntegerLevelArrayField(t));

        private readonly Type _type;

        private AbilityIntegerLevelArrayField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
        }

        public static AbilityIntegerLevelArrayField GetAbilityIntegerLevelArrayField(int i)
        {
            if (!_fields.TryGetValue(i, out var abilityIntegerLevelArrayField))
            {
                abilityIntegerLevelArrayField = new AbilityIntegerLevelArrayField((Type)i);
                _fields.Add(i, abilityIntegerLevelArrayField);
            }

            return abilityIntegerLevelArrayField;
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