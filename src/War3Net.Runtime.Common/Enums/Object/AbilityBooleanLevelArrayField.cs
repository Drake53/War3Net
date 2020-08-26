// ------------------------------------------------------------------------------
// <copyright file="AbilityBooleanLevelArrayField.cs" company="Drake53">
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
    public sealed class AbilityBooleanLevelArrayField
    {
        private static readonly Dictionary<int, AbilityBooleanLevelArrayField> _fields = GetTypes().ToDictionary(t => (int)t, t => new AbilityBooleanLevelArrayField(t));

        private readonly Type _type;

        private AbilityBooleanLevelArrayField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
        }

        public static AbilityBooleanLevelArrayField GetAbilityBooleanLevelArrayField(int i)
        {
            if (!_fields.TryGetValue(i, out var abilityBooleanLevelArrayField))
            {
                abilityBooleanLevelArrayField = new AbilityBooleanLevelArrayField((Type)i);
                _fields.Add(i, abilityBooleanLevelArrayField);
            }

            return abilityBooleanLevelArrayField;
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