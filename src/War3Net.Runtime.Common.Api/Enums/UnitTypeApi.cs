// ------------------------------------------------------------------------------
// <copyright file="UnitTypeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums;

namespace War3Net.Runtime.Common.Api.Enums
{
    public static class UnitTypeApi
    {
        public static readonly UnitType UNIT_TYPE_HERO = ConvertUnitType((int)UnitType.Type.Hero);
        public static readonly UnitType UNIT_TYPE_DEAD = ConvertUnitType((int)UnitType.Type.Dead);
        public static readonly UnitType UNIT_TYPE_STRUCTURE = ConvertUnitType((int)UnitType.Type.Structure);

        public static readonly UnitType UNIT_TYPE_FLYING = ConvertUnitType((int)UnitType.Type.Flying);
        public static readonly UnitType UNIT_TYPE_GROUND = ConvertUnitType((int)UnitType.Type.Ground);

        public static readonly UnitType UNIT_TYPE_ATTACKS_FLYING = ConvertUnitType((int)UnitType.Type.AttacksFlying);
        public static readonly UnitType UNIT_TYPE_ATTACKS_GROUND = ConvertUnitType((int)UnitType.Type.AttacksGround);

        public static readonly UnitType UNIT_TYPE_MELEE_ATTACKER = ConvertUnitType((int)UnitType.Type.MeleeAttacker);
        public static readonly UnitType UNIT_TYPE_RANGED_ATTACKER = ConvertUnitType((int)UnitType.Type.RangedAttacker);

        public static readonly UnitType UNIT_TYPE_GIANT = ConvertUnitType((int)UnitType.Type.Giant);
        public static readonly UnitType UNIT_TYPE_SUMMONED = ConvertUnitType((int)UnitType.Type.Summoned);
        public static readonly UnitType UNIT_TYPE_STUNNED = ConvertUnitType((int)UnitType.Type.Stunned);
        public static readonly UnitType UNIT_TYPE_PLAGUED = ConvertUnitType((int)UnitType.Type.Plagued);
        public static readonly UnitType UNIT_TYPE_SNARED = ConvertUnitType((int)UnitType.Type.Snared);

        public static readonly UnitType UNIT_TYPE_UNDEAD = ConvertUnitType((int)UnitType.Type.Undead);
        public static readonly UnitType UNIT_TYPE_MECHANICAL = ConvertUnitType((int)UnitType.Type.Mechanical);
        public static readonly UnitType UNIT_TYPE_PEON = ConvertUnitType((int)UnitType.Type.Peon);
        public static readonly UnitType UNIT_TYPE_SAPPER = ConvertUnitType((int)UnitType.Type.Sapper);
        public static readonly UnitType UNIT_TYPE_TOWNHALL = ConvertUnitType((int)UnitType.Type.Townhall);
        public static readonly UnitType UNIT_TYPE_ANCIENT = ConvertUnitType((int)UnitType.Type.Ancient);

        public static readonly UnitType UNIT_TYPE_TAUREN = ConvertUnitType((int)UnitType.Type.Tauren);
        public static readonly UnitType UNIT_TYPE_POISONED = ConvertUnitType((int)UnitType.Type.Poisoned);
        public static readonly UnitType UNIT_TYPE_POLYMORPHED = ConvertUnitType((int)UnitType.Type.Polymorphed);
        public static readonly UnitType UNIT_TYPE_SLEEPING = ConvertUnitType((int)UnitType.Type.Sleeping);
        public static readonly UnitType UNIT_TYPE_RESISTANT = ConvertUnitType((int)UnitType.Type.Resistant);
        public static readonly UnitType UNIT_TYPE_ETHEREAL = ConvertUnitType((int)UnitType.Type.Ethereal);
        public static readonly UnitType UNIT_TYPE_MAGIC_IMMUNE = ConvertUnitType((int)UnitType.Type.MagicImmune);

        public static UnitType ConvertUnitType(int i)
        {
            return UnitType.GetUnitType(i);
        }
    }
}