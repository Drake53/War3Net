// ------------------------------------------------------------------------------
// <copyright file="PathingType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Environment
{
    [Flags]
    public enum PathingType : byte
    {
        Walk = 1 << 1,
        Fly = 1 << 2,
        Build = 1 << 3,

        Blight = 1 << 5,
        Water = 1 << 6,
        UNK = 1 << 7,
    }
}