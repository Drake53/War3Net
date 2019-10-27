// ------------------------------------------------------------------------------
// <copyright file="GameDataSet.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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
    }
}