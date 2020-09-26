// ------------------------------------------------------------------------------
// <copyright file="PlacementApi.cs" company="Drake53">
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
    public static class PlacementApi
    {
        public static readonly Placement MAP_PLACEMENT_RANDOM = ConvertPlacement((int)Placement.Type.Random);
        public static readonly Placement MAP_PLACEMENT_FIXED = ConvertPlacement((int)Placement.Type.Fixed);
        public static readonly Placement MAP_PLACEMENT_USE_MAP_SETTINGS = ConvertPlacement((int)Placement.Type.UseMapSettings);
        public static readonly Placement MAP_PLACEMENT_TEAMS_TOGETHER = ConvertPlacement((int)Placement.Type.TeamsTogether);

        public static Placement ConvertPlacement(int i)
        {
            return Placement.GetPlacement(i);
        }
    }
}