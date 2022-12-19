﻿// ------------------------------------------------------------------------------
// <copyright file="SupportedModes.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Info
{
    [Flags]
    public enum SupportedModes
    {
        /// <summary>Classic graphics.</summary>
        SD = 1 << 0,

        /// <summary>Reforged graphics.</summary>
        HD = 1 << 1,
    }
}