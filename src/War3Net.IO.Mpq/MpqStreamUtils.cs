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
    public static class MpqStreamUtils
    {
        /// <summary>
        /// Compresses the <paramref name="baseStream"/> to a new stream.
        /// </summary>
        /// <param name="baseStream">The stream which will be compressed.</param>
        /// <param name="compressor">Determines how the <paramref name="baseStream"/> will be compressed.</param>
        /// <param name="targetBlockSize">Sets the size of the blocks in which the <paramref name="baseStream"/> is subdivided. Valid values are 512, 1024, 2048, 4096, et cetera. Set this parameter to <see langword="null"/> to not subdivide the <paramref name="baseStream"/> (which corresponds to the <see cref="MpqFileFlags.SingleUnit"/> flag).</param>
        /// <returns>A <see cref="MemoryStream"/> where the data from <paramref name="baseStream"/> has been compressed.</returns>
        public static Stream Compress(Stream baseStream, IMpqCompressor? compressor, int? targetBlockSize)
        {
            compressor ??= MpqZLibCompressor.Default;

            var resultStream = new MemoryStream();

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

                    Compress(baseStream, resultStream, (int)bytesToCompress, compressor, true);
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
                Compress(baseStream, resultStream, (int)length, compressor, false);
            }

            resultStream.Position = 0;
            return resultStream;
        }

        /// <summary>
        /// Encrypts the <paramref name="baseStream"/> to a new stream.
        /// </summary>
        /// <param name="baseStream">The stream which will be encrypted. If you want to create a stream which is both compressed and encrypted, then compression must be applied first.</param>
        /// <param name="encryptionSeed">The encryption seed to use. It's recommended to generate this value by using <see cref="MpqEncryptionUtils.CalculateEncryptionSeed(string?)"/>.</param>
        /// <param name="fileEncryptionOffset">If this value is not <see langword="null"/>, the <paramref name="encryptionSeed"/> will be adjusted based on <paramref name="fileEncryptionOffset"/> and <paramref name="uncompressedSize"/>. If the latter is <see langword="null"/>, the <paramref name="baseStream"/>'s size is used instead. The value of <paramref name="fileEncryptionOffset"/> must be set to the file's position relative the the <see cref="MpqHeader"/> inside an <see cref="MpqArchive"/>. Setting this value corresponds to an MPQ file with the <see cref="MpqFileFlags.BlockOffsetAdjustedKey"/> flag.</param>
        /// <param name="targetBlockSize">Sets the size of the blocks in which the <paramref name="baseStream"/> is subdivided. If the <paramref name="baseStream"/> has been compressed, the same value must be used as during compresion. Valid values are 512, 1024, 2048, 4096, et cetera. Set this parameter to <see langword="null"/> to not subdivide the <paramref name="baseStream"/> (which corresponds to the <see cref="MpqFileFlags.SingleUnit"/> flag).</param>
        /// <param name="uncompressedSize">The <paramref name="baseStream"/>'s uncompressed size. If the data in <paramref name="baseStream"/> is not compressed, set this parameter to <see langword="null"/>.</param>
        /// <returns>A <see cref="MemoryStream"/> where the data from <paramref name="baseStream"/> has been encrypted.</returns>
        public static Stream Encrypt(Stream baseStream, uint encryptionSeed, uint? fileEncryptionOffset, int? targetBlockSize, uint? uncompressedSize)
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
                encryptionSeed = MpqEncryptionUtils.AdjustEncryptionSeed(encryptionSeed, fileEncryptionOffset.Value, fileSize);
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

        internal static byte[] DecompressMulti(byte[] input, uint outputLength)
        {
            using var memoryStream = new MemoryStream(input);
            return GetDecompressionFunction((MpqCompressionType)memoryStream.ReadByte(), outputLength).Invoke(memoryStream);
        }

        private static void Compress(Stream baseStream, Stream targetStream, int bytesToCompress, IMpqCompressor compressor, bool isBlock)
        {
            var offset = baseStream.Position;
            using var compressedStream = compressor.Compress(baseStream, bytesToCompress);

            // Add one because CompressionType byte not written yet.
            if (isBlock && compressedStream.Length + 1 >= bytesToCompress)
            {
                // Compression did not result in smaller size, so leave the block uncompressed.
                baseStream.CopyTo(targetStream, offset, bytesToCompress, StreamExtensions.DefaultBufferSize);
            }
            else
            {
                targetStream.WriteByte((byte)compressor.CompressionType);
                compressedStream.Position = 0;
                compressedStream.CopyTo(targetStream);
            }
        }

        private static Func<Stream, byte[]> GetDecompressionFunction(MpqCompressionType compressionType, uint outputLength)
        {
            return compressionType switch
            {
                MpqCompressionType.Huffman => HuffmanCoding.Decompress,
                MpqCompressionType.ZLib => (stream) => ZLibCompression.Decompress(stream, outputLength),
                MpqCompressionType.PKLib => (stream) => PKDecompress(stream, outputLength),
                MpqCompressionType.BZip2 => (stream) => BZip2Compression.Decompress(stream, outputLength),
                MpqCompressionType.Lzma => throw new NotImplementedException("LZMA compression is not yet supported"),
                MpqCompressionType.Sparse => throw new NotImplementedException("Sparse compression is not yet supported"),
                MpqCompressionType.ImaAdpcmMono => (stream) => AdpcmCompression.Decompress(stream, 1),
                MpqCompressionType.ImaAdpcmStereo => (stream) => AdpcmCompression.Decompress(stream, 2),

                MpqCompressionType.Sparse | MpqCompressionType.ZLib => throw new NotImplementedException("Sparse compression + Deflate compression is not yet supported"),
                MpqCompressionType.Sparse | MpqCompressionType.BZip2 => throw new NotImplementedException("Sparse compression + BZip2 compression is not yet supported"),

                MpqCompressionType.ImaAdpcmMono | MpqCompressionType.Huffman => (stream) => AdpcmCompression.Decompress(HuffmanCoding.Decompress(stream), 1),
                MpqCompressionType.ImaAdpcmMono | MpqCompressionType.PKLib => (stream) => AdpcmCompression.Decompress(PKDecompress(stream, outputLength), 1),

                MpqCompressionType.ImaAdpcmStereo | MpqCompressionType.Huffman => (stream) => AdpcmCompression.Decompress(HuffmanCoding.Decompress(stream), 2),
                MpqCompressionType.ImaAdpcmStereo | MpqCompressionType.PKLib => (stream) => AdpcmCompression.Decompress(PKDecompress(stream, outputLength), 2),

                _ => throw new NotSupportedException($"Compression of type 0x{compressionType.ToString("X")} is not yet supported"),
            };
        }

        private static byte[] PKDecompress(Stream data, uint expectedLength)
        {
            var b1 = data.ReadByte();
            var b2 = data.ReadByte();
            var b3 = data.ReadByte();
            if (b1 == 0 && b2 == 0 && b3 == 0)
            {
                using (var reader = new BinaryReader(data))
                {
                    var expectedStreamLength = reader.ReadUInt32();
                    if (expectedStreamLength != data.Length)
                    {
                        throw new InvalidDataException("Unexpected stream length value");
                    }

                    if (expectedLength + 8 == expectedStreamLength)
                    {
                        // Assume data is not compressed.
                        return reader.ReadBytes((int)expectedLength);
                    }

                    var compressionType = (MpqCompressionType)reader.ReadByte();
                    if (compressionType != MpqCompressionType.ZLib)
                    {
                        throw new NotImplementedException();
                    }

                    return ZLibCompression.Decompress(data, expectedLength);
                }
            }
            else
            {
                data.Seek(-3, SeekOrigin.Current);
                return PKLibCompression.Decompress(data, expectedLength);
            }
        }
    }
}