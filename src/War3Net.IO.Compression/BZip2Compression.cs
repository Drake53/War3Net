// ------------------------------------------------------------------------------
// <copyright file="BZip2Compression.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using ICSharpCode.SharpZipLib.BZip2;

namespace War3Net.IO.Compression
{
    public static class BZip2Compression
    {
        public static byte[] Decompress(Stream compressedData, int expectedLength)
        {
            using (var output = new MemoryStream(expectedLength))
            {
                BZip2.Decompress(compressedData, output, false);
                return output.ToArray();
            }
        }
    }
}