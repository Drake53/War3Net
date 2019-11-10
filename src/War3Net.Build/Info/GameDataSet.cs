// ------------------------------------------------------------------------------
// <copyright file="GameDataSet.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Info
{
    public enum GameDataSet
    {
        /// <summary>
        /// Based on map melee status.
        /// </summary>
        Default = 0,

        /// <summary>
        /// RoC 1.01, Tft 1.07.
        /// </summary>
        Custom = 1,

        /// <summary>
        /// Latest patch.
        /// </summary>
        Melee = 2,

        Unset = -1,
    }
}