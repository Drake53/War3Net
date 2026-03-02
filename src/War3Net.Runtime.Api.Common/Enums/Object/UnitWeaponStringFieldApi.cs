#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

namespace War3Net.Runtime.Api.Common.Enums.Object
{
    public static class UnitWeaponStringFieldApi
    {
        public static readonly UnitWeaponStringField UNIT_WEAPON_SF_ATTACK_PROJECTILE_ART = ConvertUnitWeaponStringField((int)UnitWeaponStringField.Type.ATTACK_PROJECTILE_ART);

        public static UnitWeaponStringField ConvertUnitWeaponStringField(int i)
        {
            return UnitWeaponStringField.GetUnitWeaponStringField(i);
        }
    }
}