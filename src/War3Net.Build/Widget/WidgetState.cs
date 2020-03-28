// ------------------------------------------------------------------------------
// <copyright file="WidgetState.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Widget
{
    [Flags]
    public enum DoodadState
    {
        NonSolidInvisible = 0,
        NonSolidVisible = 1,
        Normal = 2,

        WithZ = 4,
    }
}