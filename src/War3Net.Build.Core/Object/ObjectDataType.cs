﻿// ------------------------------------------------------------------------------
// <copyright file="ObjectDataType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Object
{
    public enum ObjectDataType
    {
        Int = 0,
        Real = 1,
        Unreal = 2,
        String = 3,
        [Obsolete] Bool = 4,
        [Obsolete] Char = 5,
    }
}