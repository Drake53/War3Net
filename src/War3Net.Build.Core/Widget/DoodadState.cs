// ------------------------------------------------------------------------------
// <copyright file="DoodadState.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Widget
{
    [Flags]
    public enum DoodadState : byte
    {
        NonSolidInvisible = 0,
        NonSolidVisible = 1 << 0,
        Normal = 1 << 1,
        WithZ = 1 << 2,
    }
}