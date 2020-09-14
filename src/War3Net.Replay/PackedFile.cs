// ------------------------------------------------------------------------------
// <copyright file="PackedFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;
using War3Net.IO.Compression;

namespace War3Net.Replay
{
    public sealed class PackedFile
    {
        private PackedFileHeader _header;
        private MemoryStream _decompressedData;

        private PackedFile()
        {
        }

        public PackedFileHeader Header => _header;

        public Stream Data => _decompressedData;

        public static PackedFile Parse(Stream stream, bool leaveOpen = false)
        {
            var file = new PackedFile();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                file._header = PackedFileHeader.FromReader(reader);
                file._decompressedData = Decompress(reader, file._header);
            }

            return file;
        }

        private static MemoryStream Decompress(BinaryReader reader, PackedFileHeader header)
        {
            var blockCount = header.DataBlocksCount;
            var decompressedSize = header.DecompressedSize;
            var isReforged = header.IsReforgedReplay;

            var decompressedData = new MemoryStream();
            for (var chunk = 0; chunk < blockCount; chunk++)
            {
                var compressedChunkSize = reader.ReadUInt16();
                if (isReforged)
                {
                    reader.BaseStream.Seek(2, SeekOrigin.Current);
                }

                var decompressedChunkSize = reader.ReadUInt16();
                var checksum = reader.ReadUInt32();
                if (isReforged)
                {
                    reader.BaseStream.Seek(2, SeekOrigin.Current);
                }

                using var compressedDataStream = new MemoryStream();
                reader.BaseStream.CopyTo(compressedDataStream, compressedChunkSize, StreamExtensions.DefaultBufferSize);
                compressedDataStream.Position = 0;

                var decompressedDataBytes = ZLibCompression.Decompress(compressedDataStream, decompressedChunkSize);
                var decompressedDataLength = (chunk + 1 == blockCount) ? (int)(decompressedSize - decompressedData.Length) : decompressedChunkSize;
                decompressedData.Write(decompressedDataBytes, 0, decompressedDataLength);
            }

            decompressedData.Position = 0;
            return decompressedData;
        }
    }
}