// ------------------------------------------------------------------------------
// <copyright file="AbilityRealField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Enums.Object
{
    public sealed class AbilityRealField
    {
        private static readonly Dictionary<int, AbilityRealField> _fields = GetTypes().ToDictionary(t => (int)t, t => new AbilityRealField(t));

        private readonly Type _type;

        private AbilityRealField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            ARF_MISSILE_ARC = 1634558307,
        }

        public static AbilityRealField GetAbilityRealField(int i)
        {
            if (!_fields.TryGetValue(i, out var abilityRealField))
            {
                abilityRealField = new AbilityRealField((Type)i);
                _fields.Add(i, abilityRealField);
            }

            return abilityRealField;
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