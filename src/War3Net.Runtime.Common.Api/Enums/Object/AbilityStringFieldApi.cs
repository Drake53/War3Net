// ------------------------------------------------------------------------------
// <copyright file="AbilityStringFieldApi.cs" company="Drake53">
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
    public static class AbilityStringFieldApi
    {
        public static readonly AbilityStringField ABILITY_SF_NAME = ConvertAbilityStringField((int)AbilityStringField.Type.NAME);
        public static readonly AbilityStringField ABILITY_SF_ICON_ACTIVATED = ConvertAbilityStringField((int)AbilityStringField.Type.ICON_ACTIVATED);
        public static readonly AbilityStringField ABILITY_SF_ICON_RESEARCH = ConvertAbilityStringField((int)AbilityStringField.Type.ICON_RESEARCH);
        public static readonly AbilityStringField ABILITY_SF_EFFECT_SOUND = ConvertAbilityStringField((int)AbilityStringField.Type.EFFECT_SOUND);
        public static readonly AbilityStringField ABILITY_SF_EFFECT_SOUND_LOOPING = ConvertAbilityStringField((int)AbilityStringField.Type.EFFECT_SOUND_LOOPING);

        public static AbilityStringField ConvertAbilityStringField(int i)
        {
            return AbilityStringField.GetAbilityStringField(i);
        }
    }
}