// ------------------------------------------------------------------------------
// <copyright file="ZLibCompression.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using Ionic.Zlib;

using War3Net.Common.Extensions;

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
            return Compress(inputStream, bytes, CompressionLevel.BestCompression, leaveOpen);
        }

        /// <summary>
        /// Compresses data using DEFLATE.
        /// </summary>
        /// <param name="inputStream">The stream containing data to be compressed.</param>
        /// <param name="bytesToCompress">The amount of bytes from the <paramref name="inputStream"/> to compress.</param>
        /// <param name="compressionLevel">A tuning knob to trade speed for effectiveness.</param>
        /// <param name="leaveOpen"><see langword="true"/> to leave the <paramref name="inputStream"/> open; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="Stream"/> containing the compressed data.</returns>
        public static Stream Compress(Stream inputStream, int bytesToCompress, CompressionLevel compressionLevel, bool leaveOpen)
        {
            _ = inputStream ?? throw new ArgumentNullException(nameof(inputStream));

            var outputStream = new MemoryStream();

            using (var deflater = new ZlibStream(outputStream, CompressionMode.Compress, compressionLevel, true))
            {
                inputStream.CopyTo(deflater, bytesToCompress, StreamExtensions.DefaultBufferSize);
            }

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
        /// <param name="throwOnLessBytesThanExpected">
        /// If <see langword="true"/> and the decompressed data is less than <paramref name="expectedLength"/>, an exception of type <see cref="ArgumentException"/> is thrown.
        /// If <see langword="false"/> and the decompressed data is less than <paramref name="expectedLength"/>, the result array will be resized to the amount of bytes read.
        /// </param>
        /// <returns>Byte array containing the decompressed data.</returns>
        public static byte[] Decompress(byte[] data, uint expectedLength, bool throwOnLessBytesThanExpected = true)
        {
            using var memoryStream = new MemoryStream(data);
            return Decompress(memoryStream, expectedLength, throwOnLessBytesThanExpected);
        }

        /// <summary>
        /// Decompresses the input stream.
        /// </summary>
        /// <param name="data">Stream containing compressed data.</param>
        /// <param name="expectedLength">The expected length (in bytes) of the decompressed data.</param>
        /// <param name="throwOnLessBytesThanExpected">
        /// If <see langword="true"/> and the decompressed data is less than <paramref name="expectedLength"/>, an exception of type <see cref="ArgumentException"/> is thrown.
        /// If <see langword="false"/> and the decompressed data is less than <paramref name="expectedLength"/>, the result array will be resized to the amount of bytes read.
        /// </param>
        /// <returns>Byte array containing the decompressed data.</returns>
        public static byte[] Decompress(Stream data, uint expectedLength, bool throwOnLessBytesThanExpected = true)
        {
            using var inflater = new ZlibStream(data, CompressionMode.Decompress, true);

            if (throwOnLessBytesThanExpected)
            {
                var output = new byte[expectedLength];
                inflater.CopyTo(output, 0, (int)expectedLength);
                return output;
            }

            return inflater.Copy((int)expectedLength);
        }
    }
}