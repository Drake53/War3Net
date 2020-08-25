// ------------------------------------------------------------------------------
// <copyright file="AbilityStringLevelField.cs" company="Drake53">
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
    public sealed class AbilityStringLevelField
    {
        private static readonly Dictionary<int, AbilityStringLevelField> _fields = GetTypes().ToDictionary(t => (int)t, t => new AbilityStringLevelField(t));

        private readonly Type _type;

        private AbilityStringLevelField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            ICON_NORMAL = 1633776244,
            CASTER = 1633902964,
            TARGET = 1635017076,
            SPECIAL = 1634951540,
            EFFECT = 1634034036,
            AREA_EFFECT = 1633772897,
            LIGHTNING_EFFECTS = 1634494823,
            MISSILE_ART = 1634558324,
            TOOLTIP_LEARN = 1634887028,
            TOOLTIP_LEARN_EXTENDED = 1634891124,
            TOOLTIP_NORMAL = 1635020849,
            TOOLTIP_TURN_OFF = 1635087409,
            TOOLTIP_NORMAL_EXTENDED = 1635082801,
            TOOLTIP_TURN_OFF_EXTENDED = 1635087665,
            NORMAL_FORM_UNIT_EME1 = 1164797233,
            SPAWNED_UNITS = 1315205169,
            ABILITY_FOR_UNIT_CREATION = 1316119345,
            NORMAL_FORM_UNIT_MIL1 = 1298754609,
            ALTERNATE_FORM_UNIT_MIL2 = 1298754610,
            BASE_ORDER_ID_ANS5 = 1097757493,
            MORPH_UNITS_GROUND = 1349286194,
            MORPH_UNITS_AIR = 1349286195,
            MORPH_UNITS_AMPHIBIOUS = 1349286196,
            MORPH_UNITS_WATER = 1349286197,
            UNIT_TYPE_ONE = 1382115635,
            UNIT_TYPE_TWO = 1382115636,
            UNIT_TYPE_SOD2 = 1399809074,
            SUMMON_1_UNIT_TYPE = 1232303153,
            SUMMON_2_UNIT_TYPE = 1232303154,
            RACE_TO_CONVERT = 1315201841,
            PARTNER_UNIT_TYPE = 1668243761,
            PARTNER_UNIT_TYPE_ONE = 1684238385,
            PARTNER_UNIT_TYPE_TWO = 1684238386,
            REQUIRED_UNIT_TYPE = 1953524017,
            CONVERTED_UNIT_TYPE = 1953524018,
            SPELL_LIST = 1936745009,
            BASE_ORDER_ID_SPB5 = 1936745013,
            BASE_ORDER_ID_NCL6 = 1315138614,
            ABILITY_UPGRADE_1 = 1315268403,
            ABILITY_UPGRADE_2 = 1315268404,
            ABILITY_UPGRADE_3 = 1315268405,
            ABILITY_UPGRADE_4 = 1315268406,
            SPAWN_UNIT_ID_NSY2 = 1316190514,
        }

        public static AbilityStringLevelField? GetAbilityStringLevelField(int i)
        {
            return _fields.TryGetValue(i, out var abilityStringLevelField) ? abilityStringLevelField : null;
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