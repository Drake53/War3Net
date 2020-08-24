// ------------------------------------------------------------------------------
// <copyright file="AbilityRealField.cs" company="Drake53">
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
    public sealed class AbilityRealField
    {
        private static readonly Dictionary<int, AbilityRealField> _events = GetTypes().ToDictionary(t => (int)t, t => new AbilityRealField(t));

        private readonly Type _type;

        private AbilityRealField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            ARF_MISSILE_ARC = 1634558307,
        }

        public static AbilityRealField? GetAbilityRealField(int i)
        {
            return _events.TryGetValue(i, out var abilityRealField) ? abilityRealField : null;
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