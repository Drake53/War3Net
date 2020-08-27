// ------------------------------------------------------------------------------
// <copyright file="WeaponTypeApi.cs" company="Drake53">
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
    public static class WeaponTypeApi
    {
        public static readonly WeaponType WEAPON_TYPE_WHOKNOWS = ConvertWeaponType((int)WeaponType.Type.Undefined);
        public static readonly WeaponType WEAPON_TYPE_METAL_LIGHT_CHOP = ConvertWeaponType((int)WeaponType.Type.MetalLightChop);
        public static readonly WeaponType WEAPON_TYPE_METAL_MEDIUM_CHOP = ConvertWeaponType((int)WeaponType.Type.MetalMediumChop);
        public static readonly WeaponType WEAPON_TYPE_METAL_HEAVY_CHOP = ConvertWeaponType((int)WeaponType.Type.MetalHeavyChop);
        public static readonly WeaponType WEAPON_TYPE_METAL_LIGHT_SLICE = ConvertWeaponType((int)WeaponType.Type.MetalLightSlice);
        public static readonly WeaponType WEAPON_TYPE_METAL_MEDIUM_SLICE = ConvertWeaponType((int)WeaponType.Type.MetalMediumSlice);
        public static readonly WeaponType WEAPON_TYPE_METAL_HEAVY_SLICE = ConvertWeaponType((int)WeaponType.Type.MetalHeavySlice);
        public static readonly WeaponType WEAPON_TYPE_METAL_MEDIUM_BASH = ConvertWeaponType((int)WeaponType.Type.MetalMediumBash);
        public static readonly WeaponType WEAPON_TYPE_METAL_HEAVY_BASH = ConvertWeaponType((int)WeaponType.Type.MetalHeavyBash);
        public static readonly WeaponType WEAPON_TYPE_METAL_MEDIUM_STAB = ConvertWeaponType((int)WeaponType.Type.MetalMediumStab);
        public static readonly WeaponType WEAPON_TYPE_METAL_HEAVY_STAB = ConvertWeaponType((int)WeaponType.Type.MetalHeavyStab);
        public static readonly WeaponType WEAPON_TYPE_WOOD_LIGHT_SLICE = ConvertWeaponType((int)WeaponType.Type.WoodLightSlice);
        public static readonly WeaponType WEAPON_TYPE_WOOD_MEDIUM_SLICE = ConvertWeaponType((int)WeaponType.Type.WoodMediumSlice);
        public static readonly WeaponType WEAPON_TYPE_WOOD_HEAVY_SLICE = ConvertWeaponType((int)WeaponType.Type.WoodHeavySlice);
        public static readonly WeaponType WEAPON_TYPE_WOOD_LIGHT_BASH = ConvertWeaponType((int)WeaponType.Type.WoodLightBash);
        public static readonly WeaponType WEAPON_TYPE_WOOD_MEDIUM_BASH = ConvertWeaponType((int)WeaponType.Type.WoodMediumBash);
        public static readonly WeaponType WEAPON_TYPE_WOOD_HEAVY_BASH = ConvertWeaponType((int)WeaponType.Type.WoodHeavyBash);
        public static readonly WeaponType WEAPON_TYPE_WOOD_LIGHT_STAB = ConvertWeaponType((int)WeaponType.Type.WoodLightStab);
        public static readonly WeaponType WEAPON_TYPE_WOOD_MEDIUM_STAB = ConvertWeaponType((int)WeaponType.Type.WoodMediumStab);
        public static readonly WeaponType WEAPON_TYPE_CLAW_LIGHT_SLICE = ConvertWeaponType((int)WeaponType.Type.ClawLightSlice);
        public static readonly WeaponType WEAPON_TYPE_CLAW_MEDIUM_SLICE = ConvertWeaponType((int)WeaponType.Type.ClawMediumSlice);
        public static readonly WeaponType WEAPON_TYPE_CLAW_HEAVY_SLICE = ConvertWeaponType((int)WeaponType.Type.ClawHeavySlice);
        public static readonly WeaponType WEAPON_TYPE_AXE_MEDIUM_CHOP = ConvertWeaponType((int)WeaponType.Type.AxeMediumChop);
        public static readonly WeaponType WEAPON_TYPE_ROCK_HEAVY_BASH = ConvertWeaponType((int)WeaponType.Type.RockHeavyBash);

        public static WeaponType ConvertWeaponType(int i)
        {
            return WeaponType.GetWeaponType(i);
        }
    }
}