// ------------------------------------------------------------------------------
// <copyright file="ZLibCompression.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#define USING_DOTNETZIP

using System;
using System.IO;

#if USING_DOTNETZIP
using Ionic.Zlib;
#else
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
#endif

namespace War3Net.IO.Compression
{
    /// <summary>
    /// Provides methods to compress and decompress using ZLib.
    /// </summary>
    public static class ZLibCompression
    {
        /// <summary>
        /// Compresses data using DEFLATE.
        /// </summary>
        /// <param name="inputStream">The stream containing data to be compressed.</param>
        /// <param name="bytes">The amount of bytes from the <paramref name="inputStream"/> to compress.</param>
        /// <param name="leaveOpen"><see langword="true"/> to leave the <paramref name="inputStream"/> open; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="Stream"/> containing the compressed data.</returns>
        public static Stream Compress(Stream inputStream, int bytes, bool leaveOpen)
        {
            _ = inputStream ?? throw new ArgumentNullException(nameof(inputStream));

            var outputStream = new MemoryStream();

#if USING_DOTNETZIP
            using (var deflater = new ZlibStream(outputStream, CompressionMode.Compress, CompressionLevel.BestCompression, true))
            {
                inputStream.CopyTo(deflater, bytes, StreamExtensions.DefaultBufferSize);
            }
#else
            using (var stream = new MemoryStream())
            {
                using (var deflater = new DeflaterOutputStream(stream))
                {
                    inputStream.CopyTo(deflater, bytes, StreamExtensions.DefaultBufferSize);

                    deflater.Finish();
                    deflater.Flush();

                    stream.Position = 0;
                    stream.CopyTo(outputStream);
                }
            }
#endif

            if (!leaveOpen)
            {
                inputStream.Dispose();
            }

            return outputStream;
        }

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
            var output = new byte[expectedLength];
#if USING_DOTNETZIP
            using var inflater = new ZlibStream(data, CompressionMode.Decompress, true);
#else
            var inflater = new InflaterInputStream(data);
#endif
            var offset = 0;

            // expectedLength makes this unable to be combined with other compression algorithms?
            var remaining = (int)expectedLength;
            while (remaining > 0)
            {
                var size = inflater.Read(output, offset, remaining);
                if (size == 0)
                {
                    break;
                }

                offset += size;
                remaining -= size;
            }

            return output;
        }
    }
}