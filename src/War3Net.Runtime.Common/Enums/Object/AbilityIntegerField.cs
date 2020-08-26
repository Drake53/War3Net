// ------------------------------------------------------------------------------
// <copyright file="AbilityIntegerField.cs" company="Drake53">
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
    public sealed class AbilityIntegerField
    {
        private static readonly Dictionary<int, AbilityIntegerField> _fields = GetTypes().ToDictionary(t => (int)t, t => new AbilityIntegerField(t));

        private readonly Type _type;

        private AbilityIntegerField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            BUTTON_POSITION_NORMAL_X = 1633841272,
            BUTTON_POSITION_NORMAL_Y = 1633841273,
            BUTTON_POSITION_ACTIVATED_X = 1635082872,
            BUTTON_POSITION_ACTIVATED_Y = 1635082873,
            BUTTON_POSITION_RESEARCH_X = 1634889848,
            BUTTON_POSITION_RESEARCH_Y = 1634889849,
            MISSILE_SPEED = 1634562928,
            TARGET_ATTACHMENTS = 1635017059,
            CASTER_ATTACHMENTS = 1633902947,
            PRIORITY = 1634759273,
            LEVELS = 1634493814,
            REQUIRED_LEVEL = 1634888822,
            LEVEL_SKIP_REQUIREMENT = 1634497387,
        }

        public static AbilityIntegerField GetAbilityIntegerField(int i)
        {
            if (!_fields.TryGetValue(i, out var abilityIntegerField))
            {
                abilityIntegerField = new AbilityIntegerField((Type)i);
                _fields.Add(i, abilityIntegerField);
            }

            return abilityIntegerField;
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