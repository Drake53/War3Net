// ------------------------------------------------------------------------------
// <copyright file="AbilityRealLevelArrayField.cs" company="Drake53">
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
    public sealed class AbilityRealLevelArrayField
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