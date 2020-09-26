// ------------------------------------------------------------------------------
// <copyright file="UnitWeaponIntegerFieldApi.cs" company="Drake53">
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
    public static class UnitWeaponIntegerFieldApi
    {
        public static readonly UnitWeaponIntegerField UNIT_WEAPON_IF_ATTACK_DAMAGE_NUMBER_OF_DICE = ConvertUnitWeaponIntegerField((int)UnitWeaponIntegerField.Type.ATTACK_DAMAGE_NUMBER_OF_DICE);
        public static readonly UnitWeaponIntegerField UNIT_WEAPON_IF_ATTACK_DAMAGE_BASE = ConvertUnitWeaponIntegerField((int)UnitWeaponIntegerField.Type.ATTACK_DAMAGE_BASE);
        public static readonly UnitWeaponIntegerField UNIT_WEAPON_IF_ATTACK_DAMAGE_SIDES_PER_DIE = ConvertUnitWeaponIntegerField((int)UnitWeaponIntegerField.Type.ATTACK_DAMAGE_SIDES_PER_DIE);
        public static readonly UnitWeaponIntegerField UNIT_WEAPON_IF_ATTACK_MAXIMUM_NUMBER_OF_TARGETS = ConvertUnitWeaponIntegerField((int)UnitWeaponIntegerField.Type.ATTACK_MAXIMUM_NUMBER_OF_TARGETS);
        public static readonly UnitWeaponIntegerField UNIT_WEAPON_IF_ATTACK_ATTACK_TYPE = ConvertUnitWeaponIntegerField((int)UnitWeaponIntegerField.Type.ATTACK_ATTACK_TYPE);
        public static readonly UnitWeaponIntegerField UNIT_WEAPON_IF_ATTACK_WEAPON_SOUND = ConvertUnitWeaponIntegerField((int)UnitWeaponIntegerField.Type.ATTACK_WEAPON_SOUND);
        public static readonly UnitWeaponIntegerField UNIT_WEAPON_IF_ATTACK_AREA_OF_EFFECT_TARGETS = ConvertUnitWeaponIntegerField((int)UnitWeaponIntegerField.Type.ATTACK_AREA_OF_EFFECT_TARGETS);
        public static readonly UnitWeaponIntegerField UNIT_WEAPON_IF_ATTACK_TARGETS_ALLOWED = ConvertUnitWeaponIntegerField((int)UnitWeaponIntegerField.Type.ATTACK_TARGETS_ALLOWED);

        public static UnitWeaponIntegerField ConvertUnitWeaponIntegerField(int i)
        {
            return UnitWeaponIntegerField.GetUnitWeaponIntegerField(i);
        }
    }
}