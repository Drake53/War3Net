// ------------------------------------------------------------------------------
// <copyright file="UnitWeaponBooleanFieldApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums.Object;

namespace War3Net.Runtime.Common.Api.Enums.Object
{
    public static class UnitWeaponBooleanFieldApi
    {
        public static readonly UnitWeaponBooleanField UNIT_WEAPON_BF_ATTACK_SHOW_UI = ConvertUnitWeaponBooleanField((int)UnitWeaponBooleanField.Type.ATTACK_SHOW_UI);
        public static readonly UnitWeaponBooleanField UNIT_WEAPON_BF_ATTACKS_ENABLED = ConvertUnitWeaponBooleanField((int)UnitWeaponBooleanField.Type.ATTACKS_ENABLED);
        public static readonly UnitWeaponBooleanField UNIT_WEAPON_BF_ATTACK_PROJECTILE_HOMING_ENABLED = ConvertUnitWeaponBooleanField((int)UnitWeaponBooleanField.Type.ATTACK_PROJECTILE_HOMING_ENABLED);

        public static UnitWeaponBooleanField ConvertUnitWeaponBooleanField(int i)
        {
            return UnitWeaponBooleanField.GetUnitWeaponBooleanField(i);
        }
    }
}