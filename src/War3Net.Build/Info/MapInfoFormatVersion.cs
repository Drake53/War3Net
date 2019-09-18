// ------------------------------------------------------------------------------
// <copyright file="MapInfoFormatVersion.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build
{
    public enum MapInfoFormatVersion
    {
        /// <summary>
        /// Reign of Chaos format.
        /// </summary>
        RoC = 18,

        /// <summary>
        /// The Frozen Throne format.
        /// </summary>
        Tft = 25,

        /// <summary>
        /// Format introduced with lua in patch 1.31.
        /// </summary>
        Lua = 28,
    }
}