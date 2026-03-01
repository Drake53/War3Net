// ------------------------------------------------------------------------------
// <copyright file="Crc32Checksum.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.IO.Hashing;

namespace War3Net.IO.Compression
{
    public static class Crc32Checksum
    {
        public static uint Compute(Stream stream)
        {
            var crc = new Crc32();
            crc.Append(stream);
            return crc.GetCurrentHashAsUInt32();
        }
    }
}