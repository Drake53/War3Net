// ------------------------------------------------------------------------------
// <copyright file="RandomItemTableExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Info;

namespace War3Net.Build.Extensions
{
    public static class RandomItemTableExtensions
    {
        public static string GetDropItemsFunctionName(this RandomItemTable randomItemTable)
        {
            return $"ItemTable{randomItemTable.Index:D6}_DropItems";
        }
    }
}