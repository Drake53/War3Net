// ------------------------------------------------------------------------------
// <copyright file="AbilityBooleanFieldApi.cs" company="Drake53">
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
    public static class AbilityBooleanFieldApi
    {
        public static readonly AbilityBooleanField ABILITY_BF_HERO_ABILITY = ConvertAbilityBooleanField((int)AbilityBooleanField.Type.HERO_ABILITY);
        public static readonly AbilityBooleanField ABILITY_BF_ITEM_ABILITY = ConvertAbilityBooleanField((int)AbilityBooleanField.Type.ITEM_ABILITY);
        public static readonly AbilityBooleanField ABILITY_BF_CHECK_DEPENDENCIES = ConvertAbilityBooleanField((int)AbilityBooleanField.Type.CHECK_DEPENDENCIES);

        public static AbilityBooleanField ConvertAbilityBooleanField(int i)
        {
            return AbilityBooleanField.GetAbilityBooleanField(i);
        }
    }
}