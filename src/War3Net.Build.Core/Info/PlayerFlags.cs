// ------------------------------------------------------------------------------
// <copyright file="PlayerFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Info
{
    [Flags]
    public enum PlayerFlags
    {
        FixedStartPosition = 0x01,
        RaceSelectable = 0x02,
    }
}