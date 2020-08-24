// ------------------------------------------------------------------------------
// <copyright file="AbilityStringLevelArrayField.cs" company="Drake53">
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
    public sealed class AbilityStringLevelArrayField
    {
        private static readonly Dictionary<int, AbilityStringLevelArrayField> _events = GetTypes().ToDictionary(t => (int)t, t => new AbilityStringLevelArrayField(t));

        private readonly Type _type;

        private AbilityStringLevelArrayField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
        }

        public static AbilityStringLevelArrayField? GetAbilityStringLevelArrayField(int i)
        {
            return _events.TryGetValue(i, out var abilityStringLevelArrayField) ? abilityStringLevelArrayField : null;
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