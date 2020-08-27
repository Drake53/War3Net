// ------------------------------------------------------------------------------
// <copyright file="UnitRealFieldApi.cs" company="Drake53">
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
    public static class UnitRealFieldApi
    {
        public static readonly UnitRealField UNIT_RF_STRENGTH_PER_LEVEL = ConvertUnitRealField((int)UnitRealField.Type.STRENGTH_PER_LEVEL);
        public static readonly UnitRealField UNIT_RF_AGILITY_PER_LEVEL = ConvertUnitRealField((int)UnitRealField.Type.AGILITY_PER_LEVEL);
        public static readonly UnitRealField UNIT_RF_INTELLIGENCE_PER_LEVEL = ConvertUnitRealField((int)UnitRealField.Type.INTELLIGENCE_PER_LEVEL);
        public static readonly UnitRealField UNIT_RF_HIT_POINTS_REGENERATION_RATE = ConvertUnitRealField((int)UnitRealField.Type.HIT_POINTS_REGENERATION_RATE);
        public static readonly UnitRealField UNIT_RF_MANA_REGENERATION = ConvertUnitRealField((int)UnitRealField.Type.MANA_REGENERATION);
        public static readonly UnitRealField UNIT_RF_DEATH_TIME = ConvertUnitRealField((int)UnitRealField.Type.DEATH_TIME);
        public static readonly UnitRealField UNIT_RF_FLY_HEIGHT = ConvertUnitRealField((int)UnitRealField.Type.FLY_HEIGHT);
        public static readonly UnitRealField UNIT_RF_TURN_RATE = ConvertUnitRealField((int)UnitRealField.Type.TURN_RATE);
        public static readonly UnitRealField UNIT_RF_ELEVATION_SAMPLE_RADIUS = ConvertUnitRealField((int)UnitRealField.Type.ELEVATION_SAMPLE_RADIUS);
        public static readonly UnitRealField UNIT_RF_FOG_OF_WAR_SAMPLE_RADIUS = ConvertUnitRealField((int)UnitRealField.Type.FOG_OF_WAR_SAMPLE_RADIUS);
        public static readonly UnitRealField UNIT_RF_MAXIMUM_PITCH_ANGLE_DEGREES = ConvertUnitRealField((int)UnitRealField.Type.MAXIMUM_PITCH_ANGLE_DEGREES);
        public static readonly UnitRealField UNIT_RF_MAXIMUM_ROLL_ANGLE_DEGREES = ConvertUnitRealField((int)UnitRealField.Type.MAXIMUM_ROLL_ANGLE_DEGREES);
        public static readonly UnitRealField UNIT_RF_SCALING_VALUE = ConvertUnitRealField((int)UnitRealField.Type.SCALING_VALUE);
        public static readonly UnitRealField UNIT_RF_ANIMATION_RUN_SPEED = ConvertUnitRealField((int)UnitRealField.Type.ANIMATION_RUN_SPEED);
        public static readonly UnitRealField UNIT_RF_SELECTION_SCALE = ConvertUnitRealField((int)UnitRealField.Type.SELECTION_SCALE);
        public static readonly UnitRealField UNIT_RF_SELECTION_CIRCLE_HEIGHT = ConvertUnitRealField((int)UnitRealField.Type.SELECTION_CIRCLE_HEIGHT);
        public static readonly UnitRealField UNIT_RF_SHADOW_IMAGE_HEIGHT = ConvertUnitRealField((int)UnitRealField.Type.SHADOW_IMAGE_HEIGHT);
        public static readonly UnitRealField UNIT_RF_SHADOW_IMAGE_WIDTH = ConvertUnitRealField((int)UnitRealField.Type.SHADOW_IMAGE_WIDTH);
        public static readonly UnitRealField UNIT_RF_SHADOW_IMAGE_CENTER_X = ConvertUnitRealField((int)UnitRealField.Type.SHADOW_IMAGE_CENTER_X);
        public static readonly UnitRealField UNIT_RF_SHADOW_IMAGE_CENTER_Y = ConvertUnitRealField((int)UnitRealField.Type.SHADOW_IMAGE_CENTER_Y);
        public static readonly UnitRealField UNIT_RF_ANIMATION_WALK_SPEED = ConvertUnitRealField((int)UnitRealField.Type.ANIMATION_WALK_SPEED);
        public static readonly UnitRealField UNIT_RF_DEFENSE = ConvertUnitRealField((int)UnitRealField.Type.DEFENSE);
        public static readonly UnitRealField UNIT_RF_SIGHT_RADIUS = ConvertUnitRealField((int)UnitRealField.Type.SIGHT_RADIUS);
        public static readonly UnitRealField UNIT_RF_PRIORITY = ConvertUnitRealField((int)UnitRealField.Type.PRIORITY);
        public static readonly UnitRealField UNIT_RF_SPEED = ConvertUnitRealField((int)UnitRealField.Type.SPEED);
        public static readonly UnitRealField UNIT_RF_OCCLUDER_HEIGHT = ConvertUnitRealField((int)UnitRealField.Type.OCCLUDER_HEIGHT);
        public static readonly UnitRealField UNIT_RF_HP = ConvertUnitRealField((int)UnitRealField.Type.HP);
        public static readonly UnitRealField UNIT_RF_MANA = ConvertUnitRealField((int)UnitRealField.Type.MANA);
        public static readonly UnitRealField UNIT_RF_ACQUISITION_RANGE = ConvertUnitRealField((int)UnitRealField.Type.ACQUISITION_RANGE);
        public static readonly UnitRealField UNIT_RF_CAST_BACK_SWING = ConvertUnitRealField((int)UnitRealField.Type.CAST_BACK_SWING);
        public static readonly UnitRealField UNIT_RF_CAST_POINT = ConvertUnitRealField((int)UnitRealField.Type.CAST_POINT);
        public static readonly UnitRealField UNIT_RF_MINIMUM_ATTACK_RANGE = ConvertUnitRealField((int)UnitRealField.Type.MINIMUM_ATTACK_RANGE);

        public static UnitRealField ConvertUnitRealField(int i)
        {
            return UnitRealField.GetUnitRealField(i);
        }
    }
}