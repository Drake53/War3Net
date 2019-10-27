// ------------------------------------------------------------------------------
// <copyright file="RandomItemProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Info;

namespace War3Net.Build.Providers
{
    public static class RandomItemProvider
    {
        // use -1 for 'any level'
        public static char[] GetRandomItemTypeCode(ItemClass itemClass, int level)
        {
            return new[] { 'Y', (char)itemClass, 'I', (char)(level + 48) };
        }
    }
}