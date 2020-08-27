// ------------------------------------------------------------------------------
// <copyright file="RarityControlApi.cs" company="Drake53">
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
    public static class RarityControlApi
    {
        public static readonly RarityControl RARITY_FREQUENT = ConvertRarityControl((int)RarityControl.Type.Frequent);
        public static readonly RarityControl RARITY_RARE = ConvertRarityControl((int)RarityControl.Type.Rare);

        public static RarityControl ConvertRarityControl(int i)
        {
            return RarityControl.GetRarityControl(i);
        }
    }
}