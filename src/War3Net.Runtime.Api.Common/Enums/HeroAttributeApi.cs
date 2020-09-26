// ------------------------------------------------------------------------------
// <copyright file="HeroAttributeApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums;

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class HeroAttributeApi
    {
        public static readonly HeroAttribute HERO_ATTRIBUTE_STR = ConvertHeroAttribute((int)HeroAttribute.Type.Strength);
        public static readonly HeroAttribute HERO_ATTRIBUTE_INT = ConvertHeroAttribute((int)HeroAttribute.Type.Intelligence);
        public static readonly HeroAttribute HERO_ATTRIBUTE_AGI = ConvertHeroAttribute((int)HeroAttribute.Type.Agility);

        public static HeroAttribute ConvertHeroAttribute(int i)
        {
            return HeroAttribute.GetHeroAttribute(i);
        }
    }
}