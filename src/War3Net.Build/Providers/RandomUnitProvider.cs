// ------------------------------------------------------------------------------
// <copyright file="RandomUnitProvider.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Providers
{
    public static class RandomUnitProvider
    {
        // use -1 for 'any level'
        public static char[] GetRandomUnitTypeCode(int level)
        {
            return new[] { 'Y', 'Y', 'U', (char)(level + 48) };
        }
    }
}