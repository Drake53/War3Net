// ------------------------------------------------------------------------------
// <copyright file="BZip2Compression.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Compression
{
    /// <summary>
    /// Provides methods to decompress BZip2 compressed data.
    /// </summary>
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
            using var memoryStream = new MemoryStream(data);
            return Decompress(memoryStream, expectedLength);
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
#if true
                using var bZip2InputStream = new Ionic.BZip2.BZip2InputStream(data, true);
                bZip2InputStream.CopyTo(output);
#else
                ICSharpCode.SharpZipLib.BZip2.BZip2.Decompress(data, output, false);
#endif
                return output.ToArray();
            }
        }
    }
}