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
    public static class ZLibCompression
    {
        public static uint CompressTo(Stream inputStream, Stream outputStream, int bytes, bool leaveOpen)
        {
            _ = inputStream ?? throw new ArgumentNullException(nameof(inputStream));
            _ = outputStream ?? throw new ArgumentNullException(nameof(outputStream));

#if USING_DOTNETZIP
            using (var deflater = new ZlibStream(outputStream, CompressionMode.Compress, true))
            {
                for (var i = 0; i < bytes; i++)
                {
                    var r = inputStream.ReadByte();
                    if (r == -1)
                    {
                        break;
                    }

                    deflater.WriteByte((byte)r);
                }
            }
#else
            using (var stream = new MemoryStream())
            {
                using (var deflater = new DeflaterOutputStream(stream))
                {
                    for (var i = 0; i < bytes; i++)
                    {
                        var r = inputStream.ReadByte();
                        if (r == -1)
                        {
                            break;
                        }

                        deflater.WriteByte((byte)r);
                    }

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

            return (uint)outputStream.Position;
        }

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