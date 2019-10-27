// ------------------------------------------------------------------------------
// <copyright file="RandomItemProvider.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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