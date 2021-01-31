// ------------------------------------------------------------------------------
// <copyright file="RandomUnitTableExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Info;

namespace War3Net.Build.Extensions
{
    public static class RandomUnitTableExtensions
    {
        public static string GetVariableName(this RandomUnitTable randomUnitTable)
        {
            return $"gg_rg_{randomUnitTable.Index:D3}";
        }

        public static string GetVariableName(this RandomUnitTable randomUnitTable, int id)
        {
            return $"gg_rg_{id:D3}";
        }
    }
}