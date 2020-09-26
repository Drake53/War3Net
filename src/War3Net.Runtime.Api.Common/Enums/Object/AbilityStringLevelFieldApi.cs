// ------------------------------------------------------------------------------
// <copyright file="AbilityStringLevelFieldApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums.Object;

namespace War3Net.Runtime.Api.Common.Enums.Object
{
    public static class AbilityStringLevelFieldApi
    {
        public static readonly AbilityStringLevelField ABILITY_SLF_ICON_NORMAL = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.ICON_NORMAL);
        public static readonly AbilityStringLevelField ABILITY_SLF_CASTER = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.CASTER);
        public static readonly AbilityStringLevelField ABILITY_SLF_TARGET = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.TARGET);
        public static readonly AbilityStringLevelField ABILITY_SLF_SPECIAL = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.SPECIAL);
        public static readonly AbilityStringLevelField ABILITY_SLF_EFFECT = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.EFFECT);
        public static readonly AbilityStringLevelField ABILITY_SLF_AREA_EFFECT = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.AREA_EFFECT);
        public static readonly AbilityStringLevelField ABILITY_SLF_LIGHTNING_EFFECTS = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.LIGHTNING_EFFECTS);
        public static readonly AbilityStringLevelField ABILITY_SLF_MISSILE_ART = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.MISSILE_ART);
        public static readonly AbilityStringLevelField ABILITY_SLF_TOOLTIP_LEARN = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.TOOLTIP_LEARN);
        public static readonly AbilityStringLevelField ABILITY_SLF_TOOLTIP_LEARN_EXTENDED = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.TOOLTIP_LEARN_EXTENDED);
        public static readonly AbilityStringLevelField ABILITY_SLF_TOOLTIP_NORMAL = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.TOOLTIP_NORMAL);
        public static readonly AbilityStringLevelField ABILITY_SLF_TOOLTIP_TURN_OFF = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.TOOLTIP_TURN_OFF);
        public static readonly AbilityStringLevelField ABILITY_SLF_TOOLTIP_NORMAL_EXTENDED = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.TOOLTIP_NORMAL_EXTENDED);
        public static readonly AbilityStringLevelField ABILITY_SLF_TOOLTIP_TURN_OFF_EXTENDED = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.TOOLTIP_TURN_OFF_EXTENDED);
        public static readonly AbilityStringLevelField ABILITY_SLF_NORMAL_FORM_UNIT_EME1 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.NORMAL_FORM_UNIT_EME1);
        public static readonly AbilityStringLevelField ABILITY_SLF_SPAWNED_UNITS = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.SPAWNED_UNITS);
        public static readonly AbilityStringLevelField ABILITY_SLF_ABILITY_FOR_UNIT_CREATION = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.ABILITY_FOR_UNIT_CREATION);
        public static readonly AbilityStringLevelField ABILITY_SLF_NORMAL_FORM_UNIT_MIL1 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.NORMAL_FORM_UNIT_MIL1);
        public static readonly AbilityStringLevelField ABILITY_SLF_ALTERNATE_FORM_UNIT_MIL2 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.ALTERNATE_FORM_UNIT_MIL2);
        public static readonly AbilityStringLevelField ABILITY_SLF_BASE_ORDER_ID_ANS5 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.BASE_ORDER_ID_ANS5);
        public static readonly AbilityStringLevelField ABILITY_SLF_MORPH_UNITS_GROUND = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.MORPH_UNITS_GROUND);
        public static readonly AbilityStringLevelField ABILITY_SLF_MORPH_UNITS_AIR = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.MORPH_UNITS_AIR);
        public static readonly AbilityStringLevelField ABILITY_SLF_MORPH_UNITS_AMPHIBIOUS = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.MORPH_UNITS_AMPHIBIOUS);
        public static readonly AbilityStringLevelField ABILITY_SLF_MORPH_UNITS_WATER = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.MORPH_UNITS_WATER);
        public static readonly AbilityStringLevelField ABILITY_SLF_UNIT_TYPE_ONE = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.UNIT_TYPE_ONE);
        public static readonly AbilityStringLevelField ABILITY_SLF_UNIT_TYPE_TWO = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.UNIT_TYPE_TWO);
        public static readonly AbilityStringLevelField ABILITY_SLF_UNIT_TYPE_SOD2 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.UNIT_TYPE_SOD2);
        public static readonly AbilityStringLevelField ABILITY_SLF_SUMMON_1_UNIT_TYPE = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.SUMMON_1_UNIT_TYPE);
        public static readonly AbilityStringLevelField ABILITY_SLF_SUMMON_2_UNIT_TYPE = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.SUMMON_2_UNIT_TYPE);
        public static readonly AbilityStringLevelField ABILITY_SLF_RACE_TO_CONVERT = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.RACE_TO_CONVERT);
        public static readonly AbilityStringLevelField ABILITY_SLF_PARTNER_UNIT_TYPE = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.PARTNER_UNIT_TYPE);
        public static readonly AbilityStringLevelField ABILITY_SLF_PARTNER_UNIT_TYPE_ONE = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.PARTNER_UNIT_TYPE_ONE);
        public static readonly AbilityStringLevelField ABILITY_SLF_PARTNER_UNIT_TYPE_TWO = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.PARTNER_UNIT_TYPE_TWO);
        public static readonly AbilityStringLevelField ABILITY_SLF_REQUIRED_UNIT_TYPE = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.REQUIRED_UNIT_TYPE);
        public static readonly AbilityStringLevelField ABILITY_SLF_CONVERTED_UNIT_TYPE = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.CONVERTED_UNIT_TYPE);
        public static readonly AbilityStringLevelField ABILITY_SLF_SPELL_LIST = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.SPELL_LIST);
        public static readonly AbilityStringLevelField ABILITY_SLF_BASE_ORDER_ID_SPB5 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.BASE_ORDER_ID_SPB5);
        public static readonly AbilityStringLevelField ABILITY_SLF_BASE_ORDER_ID_NCL6 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.BASE_ORDER_ID_NCL6);
        public static readonly AbilityStringLevelField ABILITY_SLF_ABILITY_UPGRADE_1 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.ABILITY_UPGRADE_1);
        public static readonly AbilityStringLevelField ABILITY_SLF_ABILITY_UPGRADE_2 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.ABILITY_UPGRADE_2);
        public static readonly AbilityStringLevelField ABILITY_SLF_ABILITY_UPGRADE_3 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.ABILITY_UPGRADE_3);
        public static readonly AbilityStringLevelField ABILITY_SLF_ABILITY_UPGRADE_4 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.ABILITY_UPGRADE_4);
        public static readonly AbilityStringLevelField ABILITY_SLF_SPAWN_UNIT_ID_NSY2 = ConvertAbilityStringLevelField((int)AbilityStringLevelField.Type.SPAWN_UNIT_ID_NSY2);

        public static AbilityStringLevelField ConvertAbilityStringLevelField(int i)
        {
            return AbilityStringLevelField.GetAbilityStringLevelField(i);
        }
    }
}