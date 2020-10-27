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
        v8 = 8,
        v10 = 10,
        v11 = 11,
        v15 = 15,
        v18 = RoC,
        v23 = 23,
        v24 = 24,
        v25 = Tft,
        v26 = 26,
        v27 = 27,
        v28 = Lua,
        v31 = Reforged,

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

        /// <summary>
        /// Warcraft III Reforged format, introduced in patch 1.32.
        /// </summary>
        Reforged = 31,
    }
}