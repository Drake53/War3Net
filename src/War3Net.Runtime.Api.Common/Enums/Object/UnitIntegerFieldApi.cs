// ------------------------------------------------------------------------------
// <copyright file="UnitIntegerFieldApi.cs" company="Drake53">
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
    public static class UnitIntegerFieldApi
    {
        public static readonly UnitIntegerField UNIT_IF_DEFENSE_TYPE = ConvertUnitIntegerField((int)UnitIntegerField.Type.DEFENSE_TYPE);
        public static readonly UnitIntegerField UNIT_IF_ARMOR_TYPE = ConvertUnitIntegerField((int)UnitIntegerField.Type.ARMOR_TYPE);
        public static readonly UnitIntegerField UNIT_IF_LOOPING_FADE_IN_RATE = ConvertUnitIntegerField((int)UnitIntegerField.Type.LOOPING_FADE_IN_RATE);
        public static readonly UnitIntegerField UNIT_IF_LOOPING_FADE_OUT_RATE = ConvertUnitIntegerField((int)UnitIntegerField.Type.LOOPING_FADE_OUT_RATE);
        public static readonly UnitIntegerField UNIT_IF_AGILITY = ConvertUnitIntegerField((int)UnitIntegerField.Type.AGILITY);
        public static readonly UnitIntegerField UNIT_IF_INTELLIGENCE = ConvertUnitIntegerField((int)UnitIntegerField.Type.INTELLIGENCE);
        public static readonly UnitIntegerField UNIT_IF_STRENGTH = ConvertUnitIntegerField((int)UnitIntegerField.Type.STRENGTH);
        public static readonly UnitIntegerField UNIT_IF_AGILITY_PERMANENT = ConvertUnitIntegerField((int)UnitIntegerField.Type.AGILITY_PERMANENT);
        public static readonly UnitIntegerField UNIT_IF_INTELLIGENCE_PERMANENT = ConvertUnitIntegerField((int)UnitIntegerField.Type.INTELLIGENCE_PERMANENT);
        public static readonly UnitIntegerField UNIT_IF_STRENGTH_PERMANENT = ConvertUnitIntegerField((int)UnitIntegerField.Type.STRENGTH_PERMANENT);
        public static readonly UnitIntegerField UNIT_IF_AGILITY_WITH_BONUS = ConvertUnitIntegerField((int)UnitIntegerField.Type.AGILITY_WITH_BONUS);
        public static readonly UnitIntegerField UNIT_IF_INTELLIGENCE_WITH_BONUS = ConvertUnitIntegerField((int)UnitIntegerField.Type.INTELLIGENCE_WITH_BONUS);
        public static readonly UnitIntegerField UNIT_IF_STRENGTH_WITH_BONUS = ConvertUnitIntegerField((int)UnitIntegerField.Type.STRENGTH_WITH_BONUS);
        public static readonly UnitIntegerField UNIT_IF_GOLD_BOUNTY_AWARDED_NUMBER_OF_DICE = ConvertUnitIntegerField((int)UnitIntegerField.Type.GOLD_BOUNTY_AWARDED_NUMBER_OF_DICE);
        public static readonly UnitIntegerField UNIT_IF_GOLD_BOUNTY_AWARDED_BASE = ConvertUnitIntegerField((int)UnitIntegerField.Type.GOLD_BOUNTY_AWARDED_BASE);
        public static readonly UnitIntegerField UNIT_IF_GOLD_BOUNTY_AWARDED_SIDES_PER_DIE = ConvertUnitIntegerField((int)UnitIntegerField.Type.GOLD_BOUNTY_AWARDED_SIDES_PER_DIE);
        public static readonly UnitIntegerField UNIT_IF_LUMBER_BOUNTY_AWARDED_NUMBER_OF_DICE = ConvertUnitIntegerField((int)UnitIntegerField.Type.LUMBER_BOUNTY_AWARDED_NUMBER_OF_DICE);
        public static readonly UnitIntegerField UNIT_IF_LUMBER_BOUNTY_AWARDED_BASE = ConvertUnitIntegerField((int)UnitIntegerField.Type.LUMBER_BOUNTY_AWARDED_BASE);
        public static readonly UnitIntegerField UNIT_IF_LUMBER_BOUNTY_AWARDED_SIDES_PER_DIE = ConvertUnitIntegerField((int)UnitIntegerField.Type.LUMBER_BOUNTY_AWARDED_SIDES_PER_DIE);
        public static readonly UnitIntegerField UNIT_IF_LEVEL = ConvertUnitIntegerField((int)UnitIntegerField.Type.LEVEL);
        public static readonly UnitIntegerField UNIT_IF_FORMATION_RANK = ConvertUnitIntegerField((int)UnitIntegerField.Type.FORMATION_RANK);
        public static readonly UnitIntegerField UNIT_IF_ORIENTATION_INTERPOLATION = ConvertUnitIntegerField((int)UnitIntegerField.Type.ORIENTATION_INTERPOLATION);
        public static readonly UnitIntegerField UNIT_IF_ELEVATION_SAMPLE_POINTS = ConvertUnitIntegerField((int)UnitIntegerField.Type.ELEVATION_SAMPLE_POINTS);
        public static readonly UnitIntegerField UNIT_IF_TINTING_COLOR_RED = ConvertUnitIntegerField((int)UnitIntegerField.Type.TINTING_COLOR_RED);
        public static readonly UnitIntegerField UNIT_IF_TINTING_COLOR_GREEN = ConvertUnitIntegerField((int)UnitIntegerField.Type.TINTING_COLOR_GREEN);
        public static readonly UnitIntegerField UNIT_IF_TINTING_COLOR_BLUE = ConvertUnitIntegerField((int)UnitIntegerField.Type.TINTING_COLOR_BLUE);
        public static readonly UnitIntegerField UNIT_IF_TINTING_COLOR_ALPHA = ConvertUnitIntegerField((int)UnitIntegerField.Type.TINTING_COLOR_ALPHA);
        public static readonly UnitIntegerField UNIT_IF_MOVE_TYPE = ConvertUnitIntegerField((int)UnitIntegerField.Type.MOVE_TYPE);
        public static readonly UnitIntegerField UNIT_IF_TARGETED_AS = ConvertUnitIntegerField((int)UnitIntegerField.Type.TARGETED_AS);
        public static readonly UnitIntegerField UNIT_IF_UNIT_CLASSIFICATION = ConvertUnitIntegerField((int)UnitIntegerField.Type.UNIT_CLASSIFICATION);
        public static readonly UnitIntegerField UNIT_IF_HIT_POINTS_REGENERATION_TYPE = ConvertUnitIntegerField((int)UnitIntegerField.Type.HIT_POINTS_REGENERATION_TYPE);
        public static readonly UnitIntegerField UNIT_IF_PLACEMENT_PREVENTED_BY = ConvertUnitIntegerField((int)UnitIntegerField.Type.PLACEMENT_PREVENTED_BY);
        public static readonly UnitIntegerField UNIT_IF_PRIMARY_ATTRIBUTE = ConvertUnitIntegerField((int)UnitIntegerField.Type.PRIMARY_ATTRIBUTE);

        public static UnitIntegerField ConvertUnitIntegerField(int i)
        {
            return UnitIntegerField.GetUnitIntegerField(i);
        }
    }
}