// ------------------------------------------------------------------------------
// <copyright file="AbilityBooleanLevelFieldApi.cs" company="Drake53">
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
    public static class AbilityBooleanLevelFieldApi
    {
        public static readonly AbilityBooleanLevelField ABILITY_BLF_PERCENT_BONUS_HAB2 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.PERCENT_BONUS_HAB2);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_USE_TELEPORT_CLUSTERING_HMT3 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.USE_TELEPORT_CLUSTERING_HMT3);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_NEVER_MISS_OCR5 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.NEVER_MISS_OCR5);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_EXCLUDE_ITEM_DAMAGE = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.EXCLUDE_ITEM_DAMAGE);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_BACKSTAB_DAMAGE = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.BACKSTAB_DAMAGE);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_INHERIT_UPGRADES_UAN3 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.INHERIT_UPGRADES_UAN3);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_MANA_CONVERSION_AS_PERCENT = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.MANA_CONVERSION_AS_PERCENT);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_LIFE_CONVERSION_AS_PERCENT = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.LIFE_CONVERSION_AS_PERCENT);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_LEAVE_TARGET_ALIVE = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.LEAVE_TARGET_ALIVE);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_PERCENT_BONUS_UAU3 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.PERCENT_BONUS_UAU3);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_DAMAGE_IS_PERCENT_RECEIVED = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.DAMAGE_IS_PERCENT_RECEIVED);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_MELEE_BONUS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.MELEE_BONUS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_RANGED_BONUS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.RANGED_BONUS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_FLAT_BONUS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.FLAT_BONUS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_NEVER_MISS_HBH5 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.NEVER_MISS_HBH5);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_PERCENT_BONUS_HAD2 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.PERCENT_BONUS_HAD2);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_CAN_DEACTIVATE = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.CAN_DEACTIVATE);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_RAISED_UNITS_ARE_INVULNERABLE = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.RAISED_UNITS_ARE_INVULNERABLE);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_PERCENTAGE_OAR2 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.PERCENTAGE_OAR2);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_SUMMON_BUSY_UNITS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.SUMMON_BUSY_UNITS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_CREATES_BLIGHT = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.CREATES_BLIGHT);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_EXPLODES_ON_DEATH = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.EXPLODES_ON_DEATH);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_ALWAYS_AUTOCAST_FAE2 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.ALWAYS_AUTOCAST_FAE2);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_REGENERATE_ONLY_AT_NIGHT = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.REGENERATE_ONLY_AT_NIGHT);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_SHOW_SELECT_UNIT_BUTTON = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.SHOW_SELECT_UNIT_BUTTON);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_SHOW_UNIT_INDICATOR = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.SHOW_UNIT_INDICATOR);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_CHARGE_OWNING_PLAYER = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.CHARGE_OWNING_PLAYER);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_PERCENTAGE_ARM2 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.PERCENTAGE_ARM2);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_TARGET_IS_INVULNERABLE = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.TARGET_IS_INVULNERABLE);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_TARGET_IS_MAGIC_IMMUNE = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.TARGET_IS_MAGIC_IMMUNE);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_KILL_ON_CASTER_DEATH = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.KILL_ON_CASTER_DEATH);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_NO_TARGET_REQUIRED_REJ4 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.NO_TARGET_REQUIRED_REJ4);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_ACCEPTS_GOLD = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.ACCEPTS_GOLD);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_ACCEPTS_LUMBER = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.ACCEPTS_LUMBER);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_PREFER_HOSTILES_ROA5 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.PREFER_HOSTILES_ROA5);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_PREFER_FRIENDLIES_ROA6 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.PREFER_FRIENDLIES_ROA6);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_ROOTED_TURNING = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.ROOTED_TURNING);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_ALWAYS_AUTOCAST_SLO3 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.ALWAYS_AUTOCAST_SLO3);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_HIDE_BUTTON = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.HIDE_BUTTON);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_USE_TELEPORT_CLUSTERING_ITP2 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.USE_TELEPORT_CLUSTERING_ITP2);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_IMMUNE_TO_MORPH_EFFECTS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.IMMUNE_TO_MORPH_EFFECTS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_DOES_NOT_BLOCK_BUILDINGS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.DOES_NOT_BLOCK_BUILDINGS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_AUTO_ACQUIRE_ATTACK_TARGETS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.AUTO_ACQUIRE_ATTACK_TARGETS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_IMMUNE_TO_MORPH_EFFECTS_GHO2 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.IMMUNE_TO_MORPH_EFFECTS_GHO2);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_DO_NOT_BLOCK_BUILDINGS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.DO_NOT_BLOCK_BUILDINGS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_INCLUDE_RANGED_DAMAGE = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.INCLUDE_RANGED_DAMAGE);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_INCLUDE_MELEE_DAMAGE = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.INCLUDE_MELEE_DAMAGE);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_MOVE_TO_PARTNER = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.MOVE_TO_PARTNER);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_CAN_BE_DISPELLED = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.CAN_BE_DISPELLED);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_IGNORE_FRIENDLY_BUFFS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.IGNORE_FRIENDLY_BUFFS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_DROP_ITEMS_ON_DEATH = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.DROP_ITEMS_ON_DEATH);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_CAN_USE_ITEMS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.CAN_USE_ITEMS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_CAN_GET_ITEMS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.CAN_GET_ITEMS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_CAN_DROP_ITEMS = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.CAN_DROP_ITEMS);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_REPAIRS_ALLOWED = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.REPAIRS_ALLOWED);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_CASTER_ONLY_SPLASH = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.CASTER_ONLY_SPLASH);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_NO_TARGET_REQUIRED_IRL4 = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.NO_TARGET_REQUIRED_IRL4);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_DISPEL_ON_ATTACK = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.DISPEL_ON_ATTACK);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_AMOUNT_IS_RAW_VALUE = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.AMOUNT_IS_RAW_VALUE);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_SHARED_SPELL_COOLDOWN = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.SHARED_SPELL_COOLDOWN);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_SLEEP_ONCE = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.SLEEP_ONCE);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_ALLOW_ON_ANY_PLAYER_SLOT = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.ALLOW_ON_ANY_PLAYER_SLOT);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_DISABLE_OTHER_ABILITIES = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.DISABLE_OTHER_ABILITIES);
        public static readonly AbilityBooleanLevelField ABILITY_BLF_ALLOW_BOUNTY = ConvertAbilityBooleanLevelField((int)AbilityBooleanLevelField.Type.ALLOW_BOUNTY);

        public static AbilityBooleanLevelField ConvertAbilityBooleanLevelField(int i)
        {
            return AbilityBooleanLevelField.GetAbilityBooleanLevelField(i);
        }
    }
}