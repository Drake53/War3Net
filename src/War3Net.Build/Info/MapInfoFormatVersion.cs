// ------------------------------------------------------------------------------
// <copyright file="MapInfoFormatVersion.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Info
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