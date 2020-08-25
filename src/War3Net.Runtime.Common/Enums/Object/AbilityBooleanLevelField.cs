// ------------------------------------------------------------------------------
// <copyright file="AbilityBooleanLevelField.cs" company="Drake53">
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
    public sealed class AbilityBooleanLevelField
    {
        private static readonly Dictionary<int, AbilityBooleanLevelField> _fields = GetTypes().ToDictionary(t => (int)t, t => new AbilityBooleanLevelField(t));

        private readonly Type _type;

        private AbilityBooleanLevelField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            PERCENT_BONUS_HAB2 = 1214341682,
            USE_TELEPORT_CLUSTERING_HMT3 = 1215132723,
            NEVER_MISS_OCR5 = 1331917365,
            EXCLUDE_ITEM_DAMAGE = 1331917366,
            BACKSTAB_DAMAGE = 1333226292,
            INHERIT_UPGRADES_UAN3 = 1432448563,
            MANA_CONVERSION_AS_PERCENT = 1432645683,
            LIFE_CONVERSION_AS_PERCENT = 1432645684,
            LEAVE_TARGET_ALIVE = 1432645685,
            PERCENT_BONUS_UAU3 = 1432450355,
            DAMAGE_IS_PERCENT_RECEIVED = 1164011570,
            MELEE_BONUS = 1164014130,
            RANGED_BONUS = 1164014131,
            FLAT_BONUS = 1164014132,
            NEVER_MISS_HBH5 = 1214408757,
            PERCENT_BONUS_HAD2 = 1214342194,
            CAN_DEACTIVATE = 1214542641,
            RAISED_UNITS_ARE_INVULNERABLE = 1215456562,
            PERCENTAGE_OAR2 = 1331786290,
            SUMMON_BUSY_UNITS = 1114926130,
            CREATES_BLIGHT = 1114401074,
            EXPLODES_ON_DEATH = 1399092022,
            ALWAYS_AUTOCAST_FAE2 = 1180788018,
            REGENERATE_ONLY_AT_NIGHT = 1298297909,
            SHOW_SELECT_UNIT_BUTTON = 1315271987,
            SHOW_UNIT_INDICATOR = 1315271988,
            CHARGE_OWNING_PLAYER = 1097757494,
            PERCENTAGE_ARM2 = 1098018098,
            TARGET_IS_INVULNERABLE = 1349481267,
            TARGET_IS_MAGIC_IMMUNE = 1349481268,
            KILL_ON_CASTER_DEATH = 1432576566,
            NO_TARGET_REQUIRED_REJ4 = 1382378036,
            ACCEPTS_GOLD = 1383362097,
            ACCEPTS_LUMBER = 1383362098,
            PREFER_HOSTILES_ROA5 = 1383031093,
            PREFER_FRIENDLIES_ROA6 = 1383031094,
            ROOTED_TURNING = 1383034675,
            ALWAYS_AUTOCAST_SLO3 = 1399615283,
            HIDE_BUTTON = 1231579492,
            USE_TELEPORT_CLUSTERING_ITP2 = 1232367666,
            IMMUNE_TO_MORPH_EFFECTS = 1165256753,
            DOES_NOT_BLOCK_BUILDINGS = 1165256754,
            AUTO_ACQUIRE_ATTACK_TARGETS = 1198026545,
            IMMUNE_TO_MORPH_EFFECTS_GHO2 = 1198026546,
            DO_NOT_BLOCK_BUILDINGS = 1198026547,
            INCLUDE_RANGED_DAMAGE = 1400073012,
            INCLUDE_MELEE_DAMAGE = 1400073013,
            MOVE_TO_PARTNER = 1668243762,
            CAN_BE_DISPELLED = 1668899633,
            IGNORE_FRIENDLY_BUFFS = 1685482806,
            DROP_ITEMS_ON_DEATH = 1768846898,
            CAN_USE_ITEMS = 1768846899,
            CAN_GET_ITEMS = 1768846900,
            CAN_DROP_ITEMS = 1768846901,
            REPAIRS_ALLOWED = 1818849588,
            CASTER_ONLY_SPLASH = 1835428918,
            NO_TARGET_REQUIRED_IRL4 = 1769106484,
            DISPEL_ON_ATTACK = 1769106485,
            AMOUNT_IS_RAW_VALUE = 1768977971,
            SHARED_SPELL_COOLDOWN = 1936745010,
            SLEEP_ONCE = 1936482609,
            ALLOW_ON_ANY_PLAYER_SLOT = 1936482610,
            DISABLE_OTHER_ABILITIES = 1315138613,
            ALLOW_BOUNTY = 1316252980,
        }

        public static AbilityBooleanLevelField? GetAbilityBooleanLevelField(int i)
        {
            return _fields.TryGetValue(i, out var abilityBooleanLevelField) ? abilityBooleanLevelField : null;
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