// ------------------------------------------------------------------------------
// <copyright file="RandomUnitGlobalTableExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Widget;

namespace War3Net.Build.Extensions
{
    public static class RandomUnitGlobalTableExtensions
    {
        public static string GetVariableName(this RandomUnitGlobalTable randomUnitGlobalTable)
        {
            return $"gg_rg_{randomUnitGlobalTable.TableId:D3}";
        }
    }
}