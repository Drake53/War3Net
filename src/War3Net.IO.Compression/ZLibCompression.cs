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
        public static uint TryCompress(Stream inputStream, Stream outputStream, uint bytes, bool singleUnit)
        {
            _ = inputStream ?? throw new ArgumentNullException(nameof(inputStream));
            _ = outputStream ?? throw new ArgumentNullException(nameof(outputStream));

            var offset = inputStream.Position;
            var compressed = new MemoryStream();
            CompressTo(inputStream, compressed, (int)bytes, true);

            // Add one because CompressionType byte not written yet.
            var length = compressed.Length + 1;
            if (length == bytes || (!singleUnit && length > bytes))
            {
                compressed.Dispose();

                inputStream.Position = offset;
                for (var i = 0; i < bytes; i++)
                {
                    var r = inputStream.ReadByte();
                    if (r == -1)
                    {
                        break;
                    }

                    outputStream.WriteByte((byte)r);
                }
            }
            else
            {
                outputStream.WriteByte((byte)CompressionType.ZLib);
                compressed.Position = 0;
                compressed.CopyTo(outputStream);
                compressed.Dispose();
            }

            if (singleUnit)
            {
                inputStream.Dispose();
            }

            var result = (uint)outputStream.Position;
            return result;
        }

        private static uint CompressTo(Stream inputStream, Stream outputStream, int bytes, bool leaveOpen)
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

        public static byte[] Decompress(Stream compressedData, int expectedLength)
        {
            var output = new byte[expectedLength];
#if USING_DOTNETZIP
            using var inflater = new ZlibStream(compressedData, CompressionMode.Decompress, true);
#else
            var inflater = new InflaterInputStream(compressedData);
#endif
            var offset = 0;
            while (expectedLength > 0)
            {
                var size = inflater.Read(output, offset, expectedLength);
                if (size == 0)
                {
                    break;
                }

                offset += size;
                expectedLength -= size;
            }

            return output;
        }
    }
}