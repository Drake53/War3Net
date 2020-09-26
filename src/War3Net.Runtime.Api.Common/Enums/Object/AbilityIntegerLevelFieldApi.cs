// ------------------------------------------------------------------------------
// <copyright file="AbilityIntegerLevelFieldApi.cs" company="Drake53">
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
    public static class AbilityIntegerLevelFieldApi
    {
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MANA_COST = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MANA_COST);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_WAVES = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_WAVES);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_SHARDS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_SHARDS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_UNITS_TELEPORTED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_UNITS_TELEPORTED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMONED_UNIT_COUNT_HWE2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMONED_UNIT_COUNT_HWE2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_IMAGES = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_IMAGES);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_CORPSES_RAISED_UAN1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_CORPSES_RAISED_UAN1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MORPHING_FLAGS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MORPHING_FLAGS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_STRENGTH_BONUS_NRG5 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.STRENGTH_BONUS_NRG5);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DEFENSE_BONUS_NRG6 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DEFENSE_BONUS_NRG6);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_TARGETS_HIT = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_TARGETS_HIT);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DETECTION_TYPE_OFS1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DETECTION_TYPE_OFS1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_SUMMONED_UNITS_OSF2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_SUMMONED_UNITS_OSF2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_SUMMONED_UNITS_EFN1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_SUMMONED_UNITS_EFN1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_CORPSES_RAISED_HRE1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_CORPSES_RAISED_HRE1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_STACK_FLAGS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.STACK_FLAGS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MINIMUM_NUMBER_OF_UNITS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MINIMUM_NUMBER_OF_UNITS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_NUMBER_OF_UNITS_NDP3 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_NUMBER_OF_UNITS_NDP3);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_UNITS_CREATED_NRC2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_UNITS_CREATED_NRC2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SHIELD_LIFE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SHIELD_LIFE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MANA_LOSS_AMS4 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MANA_LOSS_AMS4);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_GOLD_PER_INTERVAL_BGM1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.GOLD_PER_INTERVAL_BGM1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAX_NUMBER_OF_MINERS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAX_NUMBER_OF_MINERS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_CARGO_CAPACITY = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.CARGO_CAPACITY);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_CREEP_LEVEL_DEV3 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_CREEP_LEVEL_DEV3);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAX_CREEP_LEVEL_DEV1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAX_CREEP_LEVEL_DEV1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_GOLD_PER_INTERVAL_EGM1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.GOLD_PER_INTERVAL_EGM1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DEFENSE_REDUCTION = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DEFENSE_REDUCTION);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DETECTION_TYPE_FLA1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DETECTION_TYPE_FLA1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_FLARE_COUNT = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.FLARE_COUNT);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAX_GOLD = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAX_GOLD);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MINING_CAPACITY = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MINING_CAPACITY);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_NUMBER_OF_CORPSES_GYD1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_NUMBER_OF_CORPSES_GYD1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DAMAGE_TO_TREE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DAMAGE_TO_TREE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_LUMBER_CAPACITY = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.LUMBER_CAPACITY);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_GOLD_CAPACITY = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.GOLD_CAPACITY);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DEFENSE_INCREASE_INF2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DEFENSE_INCREASE_INF2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_INTERACTION_TYPE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.INTERACTION_TYPE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_GOLD_COST_NDT1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.GOLD_COST_NDT1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_LUMBER_COST_NDT2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.LUMBER_COST_NDT2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DETECTION_TYPE_NDT3 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DETECTION_TYPE_NDT3);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_STACKING_TYPE_POI4 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.STACKING_TYPE_POI4);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_STACKING_TYPE_POA5 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.STACKING_TYPE_POA5);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_CREEP_LEVEL_PLY1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_CREEP_LEVEL_PLY1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_CREEP_LEVEL_POS1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_CREEP_LEVEL_POS1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MOVEMENT_UPDATE_FREQUENCY_PRG1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MOVEMENT_UPDATE_FREQUENCY_PRG1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ATTACK_UPDATE_FREQUENCY_PRG2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ATTACK_UPDATE_FREQUENCY_PRG2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MANA_LOSS_PRG6 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MANA_LOSS_PRG6);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_UNITS_SUMMONED_TYPE_ONE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.UNITS_SUMMONED_TYPE_ONE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_UNITS_SUMMONED_TYPE_TWO = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.UNITS_SUMMONED_TYPE_TWO);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAX_UNITS_SUMMONED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAX_UNITS_SUMMONED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ALLOW_WHEN_FULL_REJ3 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ALLOW_WHEN_FULL_REJ3);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_UNITS_CHARGED_TO_CASTER = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_UNITS_CHARGED_TO_CASTER);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_UNITS_AFFECTED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_UNITS_AFFECTED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DEFENSE_INCREASE_ROA2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DEFENSE_INCREASE_ROA2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAX_UNITS_ROA7 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAX_UNITS_ROA7);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ROOTED_WEAPONS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ROOTED_WEAPONS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_UPROOTED_WEAPONS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.UPROOTED_WEAPONS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_UPROOTED_DEFENSE_TYPE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.UPROOTED_DEFENSE_TYPE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ACCUMULATION_STEP = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ACCUMULATION_STEP);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_OWLS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_OWLS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_STACKING_TYPE_SPO4 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.STACKING_TYPE_SPO4);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_UNITS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_UNITS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SPIDER_CAPACITY = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SPIDER_CAPACITY);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_INTERVALS_BEFORE_CHANGING_TREES = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.INTERVALS_BEFORE_CHANGING_TREES);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_AGILITY_BONUS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.AGILITY_BONUS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_INTELLIGENCE_BONUS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.INTELLIGENCE_BONUS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_STRENGTH_BONUS_ISTR = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.STRENGTH_BONUS_ISTR);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ATTACK_BONUS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ATTACK_BONUS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DEFENSE_BONUS_IDEF = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DEFENSE_BONUS_IDEF);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMON_1_AMOUNT = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMON_1_AMOUNT);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMON_2_AMOUNT = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMON_2_AMOUNT);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_EXPERIENCE_GAINED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.EXPERIENCE_GAINED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_HIT_POINTS_GAINED_IHPG = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.HIT_POINTS_GAINED_IHPG);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MANA_POINTS_GAINED_IMPG = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MANA_POINTS_GAINED_IMPG);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_HIT_POINTS_GAINED_IHP2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.HIT_POINTS_GAINED_IHP2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MANA_POINTS_GAINED_IMP2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MANA_POINTS_GAINED_IMP2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DAMAGE_BONUS_DICE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DAMAGE_BONUS_DICE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ARMOR_PENALTY_IARP = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ARMOR_PENALTY_IARP);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ENABLED_ATTACK_INDEX_IOB5 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ENABLED_ATTACK_INDEX_IOB5);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_LEVELS_GAINED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.LEVELS_GAINED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAX_LIFE_GAINED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAX_LIFE_GAINED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAX_MANA_GAINED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAX_MANA_GAINED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_GOLD_GIVEN = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.GOLD_GIVEN);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_LUMBER_GIVEN = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.LUMBER_GIVEN);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DETECTION_TYPE_IFA1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DETECTION_TYPE_IFA1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_CREEP_LEVEL_ICRE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_CREEP_LEVEL_ICRE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MOVEMENT_SPEED_BONUS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MOVEMENT_SPEED_BONUS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_HIT_POINTS_REGENERATED_PER_SECOND = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.HIT_POINTS_REGENERATED_PER_SECOND);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SIGHT_RANGE_BONUS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SIGHT_RANGE_BONUS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DAMAGE_PER_DURATION = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DAMAGE_PER_DURATION);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MANA_USED_PER_SECOND = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MANA_USED_PER_SECOND);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_EXTRA_MANA_REQUIRED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.EXTRA_MANA_REQUIRED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DETECTION_RADIUS_IDET = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DETECTION_RADIUS_IDET);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MANA_LOSS_PER_UNIT_IDIM = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MANA_LOSS_PER_UNIT_IDIM);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DAMAGE_TO_SUMMONED_UNITS_IDID = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DAMAGE_TO_SUMMONED_UNITS_IDID);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_NUMBER_OF_UNITS_IREC = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_NUMBER_OF_UNITS_IREC);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DELAY_AFTER_DEATH_SECONDS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DELAY_AFTER_DEATH_SECONDS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_RESTORED_LIFE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.RESTORED_LIFE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_RESTORED_MANA__1_FOR_CURRENT = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.RESTORED_MANA__1_FOR_CURRENT);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_HIT_POINTS_RESTORED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.HIT_POINTS_RESTORED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MANA_POINTS_RESTORED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MANA_POINTS_RESTORED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_NUMBER_OF_UNITS_ITPM = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_NUMBER_OF_UNITS_ITPM);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_CORPSES_RAISED_CAD1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_CORPSES_RAISED_CAD1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_TERRAIN_DEFORMATION_DURATION_MS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.TERRAIN_DEFORMATION_DURATION_MS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_UNITS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_UNITS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DETECTION_TYPE_DET1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DETECTION_TYPE_DET1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_GOLD_COST_PER_STRUCTURE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.GOLD_COST_PER_STRUCTURE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_LUMBER_COST_PER_USE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.LUMBER_COST_PER_USE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DETECTION_TYPE_NSP3 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DETECTION_TYPE_NSP3);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_SWARM_UNITS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_SWARM_UNITS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAX_SWARM_UNITS_PER_TARGET = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAX_SWARM_UNITS_PER_TARGET);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_SUMMONED_UNITS_NBA2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_SUMMONED_UNITS_NBA2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_CREEP_LEVEL_NCH1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_CREEP_LEVEL_NCH1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ATTACKS_PREVENTED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ATTACKS_PREVENTED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_NUMBER_OF_TARGETS_EFK3 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_NUMBER_OF_TARGETS_EFK3);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_SUMMONED_UNITS_ESV1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_SUMMONED_UNITS_ESV1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_NUMBER_OF_CORPSES_EXH1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_NUMBER_OF_CORPSES_EXH1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ITEM_CAPACITY = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ITEM_CAPACITY);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_NUMBER_OF_TARGETS_SPL2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_NUMBER_OF_TARGETS_SPL2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ALLOW_WHEN_FULL_IRL3 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ALLOW_WHEN_FULL_IRL3);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_DISPELLED_UNITS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_DISPELLED_UNITS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_LURES = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_LURES);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NEW_TIME_OF_DAY_HOUR = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NEW_TIME_OF_DAY_HOUR);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NEW_TIME_OF_DAY_MINUTE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NEW_TIME_OF_DAY_MINUTE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_UNITS_CREATED_MEC1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_UNITS_CREATED_MEC1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MINIMUM_SPELLS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MINIMUM_SPELLS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_SPELLS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_SPELLS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DISABLED_ATTACK_INDEX = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DISABLED_ATTACK_INDEX);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ENABLED_ATTACK_INDEX_GRA4 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ENABLED_ATTACK_INDEX_GRA4);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAXIMUM_ATTACKS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAXIMUM_ATTACKS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_BUILDING_TYPES_ALLOWED_NPR1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.BUILDING_TYPES_ALLOWED_NPR1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_BUILDING_TYPES_ALLOWED_NSA1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.BUILDING_TYPES_ALLOWED_NSA1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ATTACK_MODIFICATION = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ATTACK_MODIFICATION);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMONED_UNIT_COUNT_NPA5 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMONED_UNIT_COUNT_NPA5);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_UPGRADE_LEVELS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.UPGRADE_LEVELS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_SUMMONED_UNITS_NDO2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_SUMMONED_UNITS_NDO2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_BEASTS_PER_SECOND = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.BEASTS_PER_SECOND);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_TARGET_TYPE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.TARGET_TYPE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_OPTIONS = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.OPTIONS);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ARMOR_PENALTY_NAB3 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ARMOR_PENALTY_NAB3);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_WAVE_COUNT_NHS6 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.WAVE_COUNT_NHS6);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAX_CREEP_LEVEL_NTM3 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAX_CREEP_LEVEL_NTM3);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MISSILE_COUNT = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MISSILE_COUNT);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SPLIT_ATTACK_COUNT = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SPLIT_ATTACK_COUNT);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_GENERATION_COUNT = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.GENERATION_COUNT);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ROCK_RING_COUNT = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ROCK_RING_COUNT);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_WAVE_COUNT_NVC2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.WAVE_COUNT_NVC2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_PREFER_HOSTILES_TAU1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.PREFER_HOSTILES_TAU1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_PREFER_FRIENDLIES_TAU2 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.PREFER_FRIENDLIES_TAU2);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_MAX_UNITS_TAU3 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.MAX_UNITS_TAU3);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NUMBER_OF_PULSES = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NUMBER_OF_PULSES);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMONED_UNIT_TYPE_HWE1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMONED_UNIT_TYPE_HWE1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMONED_UNIT_UIN4 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMONED_UNIT_UIN4);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMONED_UNIT_OSF1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMONED_UNIT_OSF1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMONED_UNIT_TYPE_EFNU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMONED_UNIT_TYPE_EFNU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMONED_UNIT_TYPE_NBAU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMONED_UNIT_TYPE_NBAU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMONED_UNIT_TYPE_NTOU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMONED_UNIT_TYPE_NTOU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMONED_UNIT_TYPE_ESVU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMONED_UNIT_TYPE_ESVU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMONED_UNIT_TYPES = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMONED_UNIT_TYPES);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SUMMONED_UNIT_TYPE_NDOU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SUMMONED_UNIT_TYPE_NDOU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ALTERNATE_FORM_UNIT_EMEU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ALTERNATE_FORM_UNIT_EMEU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_PLAGUE_WARD_UNIT_TYPE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.PLAGUE_WARD_UNIT_TYPE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ALLOWED_UNIT_TYPE_BTL1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ALLOWED_UNIT_TYPE_BTL1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_NEW_UNIT_TYPE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.NEW_UNIT_TYPE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_RESULTING_UNIT_TYPE_ENT1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.RESULTING_UNIT_TYPE_ENT1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_CORPSE_UNIT_TYPE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.CORPSE_UNIT_TYPE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_ALLOWED_UNIT_TYPE_LOA1 = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.ALLOWED_UNIT_TYPE_LOA1);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_UNIT_TYPE_FOR_LIMIT_CHECK = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.UNIT_TYPE_FOR_LIMIT_CHECK);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_WARD_UNIT_TYPE_STAU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.WARD_UNIT_TYPE_STAU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_EFFECT_ABILITY = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.EFFECT_ABILITY);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_CONVERSION_UNIT = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.CONVERSION_UNIT);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_UNIT_TO_PRESERVE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.UNIT_TO_PRESERVE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_UNIT_TYPE_ALLOWED = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.UNIT_TYPE_ALLOWED);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SWARM_UNIT_TYPE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SWARM_UNIT_TYPE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_RESULTING_UNIT_TYPE_COAU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.RESULTING_UNIT_TYPE_COAU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_UNIT_TYPE_EXHU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.UNIT_TYPE_EXHU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_WARD_UNIT_TYPE_HWDU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.WARD_UNIT_TYPE_HWDU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_LURE_UNIT_TYPE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.LURE_UNIT_TYPE);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_UNIT_TYPE_IPMU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.UNIT_TYPE_IPMU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_FACTORY_UNIT_ID = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.FACTORY_UNIT_ID);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_SPAWN_UNIT_ID_NFYU = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.SPAWN_UNIT_ID_NFYU);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_DESTRUCTIBLE_ID = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.DESTRUCTIBLE_ID);
        public static readonly AbilityIntegerLevelField ABILITY_ILF_UPGRADE_TYPE = ConvertAbilityIntegerLevelField((int)AbilityIntegerLevelField.Type.UPGRADE_TYPE);

        public static AbilityIntegerLevelField ConvertAbilityIntegerLevelField(int i)
        {
            return AbilityIntegerLevelField.GetAbilityIntegerLevelField(i);
        }
    }
}