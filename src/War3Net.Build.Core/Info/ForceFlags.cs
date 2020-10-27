// ------------------------------------------------------------------------------
// <copyright file="ForceFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Info
{
    [Flags]
    public enum ForceFlags
    {
        Allied = 0x01,
        AlliedVictory = 0x02,
        UNK4 = 0x04,
        ShareVision = 0x08,
        ShareUnitControl = 0x10,
        ShareAdvancedUnitControl = 0x20,
    }
}