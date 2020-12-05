// ------------------------------------------------------------------------------
// <copyright file="ImportedFileFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Import
{
    [Flags]
    public enum ImportedFileFlags
    {
        UNK1 = 1 << 0,
        UNK2 = 1 << 1,
        UNK4 = 1 << 2,
        UNK8 = 1 << 3,
        UNK16 = 1 << 4,
    }
}