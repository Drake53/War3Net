// ------------------------------------------------------------------------------
// <copyright file="DefenseTypeApi.cs" company="Drake53">
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
    public static class DefenseTypeApi
    {
        public static readonly DefenseType DEFENSE_TYPE_LIGHT = ConvertDefenseType((int)DefenseType.Type.Light);
        public static readonly DefenseType DEFENSE_TYPE_MEDIUM = ConvertDefenseType((int)DefenseType.Type.Medium);
        public static readonly DefenseType DEFENSE_TYPE_LARGE = ConvertDefenseType((int)DefenseType.Type.Large);
        public static readonly DefenseType DEFENSE_TYPE_FORT = ConvertDefenseType((int)DefenseType.Type.Fortified);
        public static readonly DefenseType DEFENSE_TYPE_NORMAL = ConvertDefenseType((int)DefenseType.Type.Normal);
        public static readonly DefenseType DEFENSE_TYPE_HERO = ConvertDefenseType((int)DefenseType.Type.Hero);
        public static readonly DefenseType DEFENSE_TYPE_DIVINE = ConvertDefenseType((int)DefenseType.Type.Divine);
        public static readonly DefenseType DEFENSE_TYPE_NONE = ConvertDefenseType((int)DefenseType.Type.None);

        public static DefenseType ConvertDefenseType(int i)
        {
            return DefenseType.GetDefenseType(i);
        }
    }
}