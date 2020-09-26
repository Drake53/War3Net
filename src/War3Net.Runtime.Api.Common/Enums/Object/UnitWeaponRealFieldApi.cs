// ------------------------------------------------------------------------------
// <copyright file="UnitWeaponRealFieldApi.cs" company="Drake53">
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
    public static class UnitWeaponRealFieldApi
    {
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_BACKSWING_POINT = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_BACKSWING_POINT);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_DAMAGE_POINT = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_DAMAGE_POINT);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_BASE_COOLDOWN = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_BASE_COOLDOWN);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_DAMAGE_LOSS_FACTOR = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_DAMAGE_LOSS_FACTOR);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_DAMAGE_FACTOR_MEDIUM = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_DAMAGE_FACTOR_MEDIUM);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_DAMAGE_FACTOR_SMALL = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_DAMAGE_FACTOR_SMALL);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_DAMAGE_SPILL_DISTANCE = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_DAMAGE_SPILL_DISTANCE);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_DAMAGE_SPILL_RADIUS = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_DAMAGE_SPILL_RADIUS);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_PROJECTILE_SPEED = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_PROJECTILE_SPEED);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_PROJECTILE_ARC = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_PROJECTILE_ARC);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_AREA_OF_EFFECT_FULL_DAMAGE = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_AREA_OF_EFFECT_FULL_DAMAGE);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_AREA_OF_EFFECT_MEDIUM_DAMAGE = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_AREA_OF_EFFECT_MEDIUM_DAMAGE);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_AREA_OF_EFFECT_SMALL_DAMAGE = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_AREA_OF_EFFECT_SMALL_DAMAGE);
        public static readonly UnitWeaponRealField UNIT_WEAPON_RF_ATTACK_RANGE = ConvertUnitWeaponRealField((int)UnitWeaponRealField.Type.ATTACK_RANGE);

        public static UnitWeaponRealField ConvertUnitWeaponRealField(int i)
        {
            return UnitWeaponRealField.GetUnitWeaponRealField(i);
        }
    }
}