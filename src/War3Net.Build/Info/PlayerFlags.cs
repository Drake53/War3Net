// ------------------------------------------------------------------------------
// <copyright file="PlayerFlags.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build
{
    [Flags]
    public enum PlayerFlags
    {
        FixedStartPosition = 0x01,
        RaceSelectable = 0x02,
    }
}