// ------------------------------------------------------------------------------
// <copyright file="TileFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Environment
{
    [Flags]
    public enum TileFlags : byte
    {
        Ramp = 1 << 0,
        Blighted = 1 << 1,
        Water = 1 << 2,
        Boundary = 1 << 3,
    }
}