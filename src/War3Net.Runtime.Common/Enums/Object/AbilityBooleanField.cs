// ------------------------------------------------------------------------------
// <copyright file="AbilityBooleanField.cs" company="Drake53">
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
    public sealed class AbilityBooleanField
    {
        private static readonly Dictionary<int, AbilityBooleanField> _fields = GetTypes().ToDictionary(t => (int)t, t => new AbilityBooleanField(t));

        private readonly Type _type;

        private AbilityBooleanField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            HERO_ABILITY = 1634231666,
            ITEM_ABILITY = 1634301029,
            CHECK_DEPENDENCIES = 1633904740,
        }

        public static AbilityBooleanField GetAbilityBooleanField(int i)
        {
            if (!_fields.TryGetValue(i, out var abilityBooleanField))
            {
                abilityBooleanField = new AbilityBooleanField((Type)i);
                _fields.Add(i, abilityBooleanField);
            }

            return abilityBooleanField;
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