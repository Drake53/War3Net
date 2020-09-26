// ------------------------------------------------------------------------------
// <copyright file="MapDensityApi.cs" company="Drake53">
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
    public static class MapDensityApi
    {
        public static readonly MapDensity MAP_DENSITY_NONE = ConvertMapDensity((int)MapDensity.Type.None);
        public static readonly MapDensity MAP_DENSITY_LIGHT = ConvertMapDensity((int)MapDensity.Type.Light);
        public static readonly MapDensity MAP_DENSITY_MEDIUM = ConvertMapDensity((int)MapDensity.Type.Medium);
        public static readonly MapDensity MAP_DENSITY_HEAVY = ConvertMapDensity((int)MapDensity.Type.Heavy);

        public static MapDensity ConvertMapDensity(int i)
        {
            return MapDensity.GetMapDensity(i);
        }
    }
}