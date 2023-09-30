// ------------------------------------------------------------------------------
// <copyright file="MpqStreamUtils.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

using War3Net.Common.Extensions;
using War3Net.Common.Providers;
using War3Net.IO.Compression;

namespace War3Net.IO.Mpq
{
    internal static class MpqStreamUtils
    {
        internal static Stream Compress(Stream baseStream, MpqCompressionType compressionType, int? targetBlockSize)
        {
            var resultStream = new MemoryStream();
            var singleUnit = !targetBlockSize.HasValue;

            void TryCompress(uint bytes)
            {
                var offset = baseStream.Position;
                var compressedStream = compressionType switch
                {
                    MpqCompressionType.ZLib => ZLibCompression.Compress(baseStream, (int)bytes, true),

                    _ => throw new NotSupportedException($"Compression type '{compressionType}' is not supported. Currently only ZLib is supported."),
                };

                // Add one because CompressionType byte not written yet.
                var length = compressedStream.Length + 1;
                if (!singleUnit && length >= bytes)
                {
                    baseStream.CopyTo(resultStream, offset, (int)bytes, StreamExtensions.DefaultBufferSize);
                }
                else
                {
                    resultStream.WriteByte((byte)compressionType);
                    compressedStream.Position = 0;
                    compressedStream.CopyTo(resultStream);
                }

                compressedStream.Dispose();
            }

            var length = (uint)baseStream.Length;

            if (targetBlockSize.HasValue)
            {
                var blockCount = (uint)((length + targetBlockSize - 1) / targetBlockSize) + 1;
                var blockOffsets = new uint[blockCount];

                blockOffsets[0] = 4 * blockCount;
                resultStream.Position = blockOffsets[0];

                for (var blockIndex = 1; blockIndex < blockCount; blockIndex++)
                {
                    var bytesToCompress = blockIndex + 1 == blockCount ? (uint)(baseStream.Length - baseStream.Position) : (uint)targetBlockSize;

                    TryCompress(bytesToCompress);
                    blockOffsets[blockIndex] = (uint)resultStream.Position;
                }

                resultStream.Position = 0;
                using (var writer = new BinaryWriter(resultStream, UTF8EncodingProvider.StrictUTF8, true))
                {
                    for (var blockIndex = 0; blockIndex < blockCount; blockIndex++)
                    {
                        writer.Write(blockOffsets[blockIndex]);
                    }
                }
            }
            else
            {
                TryCompress(length);
            }

            resultStream.Position = 0;
            return resultStream;
        }

        internal static Stream Encrypt(Stream baseStream, uint encryptionSeed, uint? fileEncryptionOffset, int? targetBlockSize, uint? uncompressedSize)
        {
            if (uncompressedSize.HasValue && uncompressedSize.Value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(uncompressedSize));
            }

            var compressedSize = (uint)baseStream.Length;
            var fileSize = uncompressedSize ?? compressedSize;

            var resultStream = new MemoryStream();

            int[] blockPositions;
            if (targetBlockSize.HasValue)
            {
                blockPositions = new int[(uint)(((int)fileSize + targetBlockSize.Value - 1) / targetBlockSize.Value) + 1];

                if (uncompressedSize.HasValue)
                {
                    using (var reader = new BinaryReader(baseStream, Encoding.UTF8, true))
                    {
                        for (var i = 0; i < blockPositions.Length; i++)
                        {
                            blockPositions[i] = (int)reader.ReadUInt32();
                        }
                    }

                    baseStream.Seek(0, SeekOrigin.Begin);
                }
                else
                {
                    for (var i = 0; i + 1 < blockPositions.Length; i++)
                    {
                        blockPositions[i] = targetBlockSize.Value * i;
                    }

                    blockPositions[^1] = (int)compressedSize;
                }
            }
            else
            {
                blockPositions = new int[2];
                blockPositions[0] = 0;
                blockPositions[^1] = (int)compressedSize;
            }

            if (fileEncryptionOffset.HasValue)
            {
                encryptionSeed = MpqEntry.AdjustEncryptionSeed(encryptionSeed, fileEncryptionOffset.Value, fileSize);
            }

            var currentOffset = 0;
            using (var writer = new BinaryWriter(resultStream, UTF8EncodingProvider.StrictUTF8, true))
            {
                for (var blockIndex = 0; blockIndex < blockPositions.Length; blockIndex++)
                {
                    var toWrite = blockPositions[blockIndex] - currentOffset;
                    if (toWrite == 0)
                    {
                        continue;
                    }

                    var data = StormBuffer.EncryptStream(baseStream, (uint)(encryptionSeed + blockIndex - 1), currentOffset, toWrite);
                    writer.Write(data);

                    currentOffset += toWrite;
                }
            }

            resultStream.Position = 0;
            return resultStream;
        }
    }
}