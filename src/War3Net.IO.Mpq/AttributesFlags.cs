// ------------------------------------------------------------------------------
// <copyright file="AttributesFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Mpq
{
    [Flags]
    public enum AttributesFlags
    {
        Crc32 = 0x01,
        DateTime = 0x02,
    }
}