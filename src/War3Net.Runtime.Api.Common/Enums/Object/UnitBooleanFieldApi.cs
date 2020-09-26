// ------------------------------------------------------------------------------
// <copyright file="UnitBooleanFieldApi.cs" company="Drake53">
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
    public static class UnitBooleanFieldApi
    {
        public static readonly UnitBooleanField UNIT_BF_RAISABLE = ConvertUnitBooleanField((int)UnitBooleanField.Type.RAISABLE);
        public static readonly UnitBooleanField UNIT_BF_DECAYABLE = ConvertUnitBooleanField((int)UnitBooleanField.Type.DECAYABLE);
        public static readonly UnitBooleanField UNIT_BF_IS_A_BUILDING = ConvertUnitBooleanField((int)UnitBooleanField.Type.IS_A_BUILDING);
        public static readonly UnitBooleanField UNIT_BF_USE_EXTENDED_LINE_OF_SIGHT = ConvertUnitBooleanField((int)UnitBooleanField.Type.USE_EXTENDED_LINE_OF_SIGHT);
        public static readonly UnitBooleanField UNIT_BF_NEUTRAL_BUILDING_SHOWS_MINIMAP_ICON = ConvertUnitBooleanField((int)UnitBooleanField.Type.NEUTRAL_BUILDING_SHOWS_MINIMAP_ICON);
        public static readonly UnitBooleanField UNIT_BF_HERO_HIDE_HERO_INTERFACE_ICON = ConvertUnitBooleanField((int)UnitBooleanField.Type.HERO_HIDE_HERO_INTERFACE_ICON);
        public static readonly UnitBooleanField UNIT_BF_HERO_HIDE_HERO_MINIMAP_DISPLAY = ConvertUnitBooleanField((int)UnitBooleanField.Type.HERO_HIDE_HERO_MINIMAP_DISPLAY);
        public static readonly UnitBooleanField UNIT_BF_HERO_HIDE_HERO_DEATH_MESSAGE = ConvertUnitBooleanField((int)UnitBooleanField.Type.HERO_HIDE_HERO_DEATH_MESSAGE);
        public static readonly UnitBooleanField UNIT_BF_HIDE_MINIMAP_DISPLAY = ConvertUnitBooleanField((int)UnitBooleanField.Type.HIDE_MINIMAP_DISPLAY);
        public static readonly UnitBooleanField UNIT_BF_SCALE_PROJECTILES = ConvertUnitBooleanField((int)UnitBooleanField.Type.SCALE_PROJECTILES);
        public static readonly UnitBooleanField UNIT_BF_SELECTION_CIRCLE_ON_WATER = ConvertUnitBooleanField((int)UnitBooleanField.Type.SELECTION_CIRCLE_ON_WATER);
        public static readonly UnitBooleanField UNIT_BF_HAS_WATER_SHADOW = ConvertUnitBooleanField((int)UnitBooleanField.Type.HAS_WATER_SHADOW);

        public static UnitBooleanField ConvertUnitBooleanField(int i)
        {
            return UnitBooleanField.GetUnitBooleanField(i);
        }
    }
}