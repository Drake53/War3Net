// ------------------------------------------------------------------------------
// <copyright file="PathingTypeApi.cs" company="Drake53">
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
    public static class PathingTypeApi
    {
        public static readonly PathingType PATHING_TYPE_ANY = ConvertPathingType((int)PathingType.Type.Any);
        public static readonly PathingType PATHING_TYPE_WALKABILITY = ConvertPathingType((int)PathingType.Type.Walkability);
        public static readonly PathingType PATHING_TYPE_FLYABILITY = ConvertPathingType((int)PathingType.Type.Flyability);
        public static readonly PathingType PATHING_TYPE_BUILDABILITY = ConvertPathingType((int)PathingType.Type.Buildability);
        public static readonly PathingType PATHING_TYPE_PEONHARVESTPATHING = ConvertPathingType((int)PathingType.Type.PeonHarvestPathing);
        public static readonly PathingType PATHING_TYPE_BLIGHTPATHING = ConvertPathingType((int)PathingType.Type.BlightPathing);
        public static readonly PathingType PATHING_TYPE_FLOATABILITY = ConvertPathingType((int)PathingType.Type.Floatability);
        public static readonly PathingType PATHING_TYPE_AMPHIBIOUSPATHING = ConvertPathingType((int)PathingType.Type.AmphibiousPathing);

        public static PathingType ConvertPathingType(int i)
        {
            return PathingType.GetPathingType(i);
        }
    }
}