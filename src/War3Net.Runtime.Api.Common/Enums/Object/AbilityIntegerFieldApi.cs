// ------------------------------------------------------------------------------
// <copyright file="AbilityIntegerFieldApi.cs" company="Drake53">
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
    public static class AbilityIntegerFieldApi
    {
        public static readonly AbilityIntegerField ABILITY_IF_BUTTON_POSITION_NORMAL_X = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.BUTTON_POSITION_NORMAL_X);
        public static readonly AbilityIntegerField ABILITY_IF_BUTTON_POSITION_NORMAL_Y = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.BUTTON_POSITION_NORMAL_Y);
        public static readonly AbilityIntegerField ABILITY_IF_BUTTON_POSITION_ACTIVATED_X = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.BUTTON_POSITION_ACTIVATED_X);
        public static readonly AbilityIntegerField ABILITY_IF_BUTTON_POSITION_ACTIVATED_Y = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.BUTTON_POSITION_ACTIVATED_Y);
        public static readonly AbilityIntegerField ABILITY_IF_BUTTON_POSITION_RESEARCH_X = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.BUTTON_POSITION_RESEARCH_X);
        public static readonly AbilityIntegerField ABILITY_IF_BUTTON_POSITION_RESEARCH_Y = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.BUTTON_POSITION_RESEARCH_Y);
        public static readonly AbilityIntegerField ABILITY_IF_MISSILE_SPEED = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.MISSILE_SPEED);
        public static readonly AbilityIntegerField ABILITY_IF_TARGET_ATTACHMENTS = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.TARGET_ATTACHMENTS);
        public static readonly AbilityIntegerField ABILITY_IF_CASTER_ATTACHMENTS = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.CASTER_ATTACHMENTS);
        public static readonly AbilityIntegerField ABILITY_IF_PRIORITY = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.PRIORITY);
        public static readonly AbilityIntegerField ABILITY_IF_LEVELS = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.LEVELS);
        public static readonly AbilityIntegerField ABILITY_IF_REQUIRED_LEVEL = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.REQUIRED_LEVEL);
        public static readonly AbilityIntegerField ABILITY_IF_LEVEL_SKIP_REQUIREMENT = ConvertAbilityIntegerField((int)AbilityIntegerField.Type.LEVEL_SKIP_REQUIREMENT);

        public static AbilityIntegerField ConvertAbilityIntegerField(int i)
        {
            return AbilityIntegerField.GetAbilityIntegerField(i);
        }
    }
}