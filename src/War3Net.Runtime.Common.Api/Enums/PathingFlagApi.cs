// ------------------------------------------------------------------------------
// <copyright file="PathingFlagApi.cs" company="Drake53">
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
    public static class PathingFlagApi
    {
        public static readonly PathingFlag PATHING_FLAG_UNWALKABLE = ConvertPathingFlag((int)PathingFlag.Type.Unwalkable);
        public static readonly PathingFlag PATHING_FLAG_UNFLYABLE = ConvertPathingFlag((int)PathingFlag.Type.Unflyable);
        public static readonly PathingFlag PATHING_FLAG_UNBUILDABLE = ConvertPathingFlag((int)PathingFlag.Type.Unbuildable);
        public static readonly PathingFlag PATHING_FLAG_UNPEONHARVEST = ConvertPathingFlag((int)PathingFlag.Type.UnPeonHarvest);
        public static readonly PathingFlag PATHING_FLAG_BLIGHTED = ConvertPathingFlag((int)PathingFlag.Type.Blighted);
        public static readonly PathingFlag PATHING_FLAG_UNFLOATABLE = ConvertPathingFlag((int)PathingFlag.Type.Unfloatable);
        public static readonly PathingFlag PATHING_FLAG_UNAMPHIBIOUS = ConvertPathingFlag((int)PathingFlag.Type.Unamphibious);
        public static readonly PathingFlag PATHING_FLAG_UNITEMPLACABLE = ConvertPathingFlag((int)PathingFlag.Type.UnItemPlacable);

        public static PathingFlag ConvertPathingFlag(int i)
        {
            return PathingFlag.GetPathingFlag(i);
        }
    }
}