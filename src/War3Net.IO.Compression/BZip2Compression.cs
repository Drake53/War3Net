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
        /// <summary>
        /// Decompresses the input data.
        /// </summary>
        /// <param name="data">Byte array containing compressed data.</param>
        /// <param name="expectedLength">The expected length (in bytes) of the decompressed data.</param>
        /// <returns>Byte array containing the decompressed data.</returns>
        public static byte[] Decompress(byte[] data, uint expectedLength)
        {
            return Decompress(new MemoryStream(data), expectedLength);
        }

        /// <summary>
        /// Decompresses the input stream.
        /// </summary>
        /// <param name="data">Stream containing compressed data.</param>
        /// <param name="expectedLength">The expected length (in bytes) of the decompressed data.</param>
        /// <returns>Byte array containing the decompressed data.</returns>
        public static byte[] Decompress(Stream data, uint expectedLength)
        {
            using (var output = new MemoryStream((int)expectedLength))
            {
                BZip2.Decompress(data, output, false);
                return output.ToArray();
            }
        }
    }
}