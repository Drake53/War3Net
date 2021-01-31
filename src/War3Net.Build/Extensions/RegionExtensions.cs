// ------------------------------------------------------------------------------
// <copyright file="RegionExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Environment;

namespace War3Net.Build.Extensions
{
    public static class RegionExtensions
    {
        public static string GetVariableName(this Region region)
        {
            return $"gg_rct_{region.Name.Replace(' ', '_')}";
        }
    }
}