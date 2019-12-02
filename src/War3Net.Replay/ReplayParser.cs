// ------------------------------------------------------------------------------
// <copyright file="ReplayParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

using War3Net.IO.Compression;

namespace War3Net.Replay
{
    public sealed class ReplayParser
    {
        private ReplayHeader _header;

        public ReplayParser()
        {
        }

        public void Parse(Stream stream, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen);

            _header = ReplayHeader.FromReader(reader);

            Decompress(reader);
        }

        private Stream Decompress(BinaryReader reader)
        {
            var decompressedData = new MemoryStream();
            for (var chunk = 0; chunk < _header.DataBlocksCount; chunk++)
            {
                var compressedChunkSize = reader.ReadUInt16();
                var decompressedChunkSize = reader.ReadUInt16();
                var checksum = reader.ReadUInt32();

                using var compressedDataStream = new MemoryStream();
                reader.BaseStream.CopyTo(compressedDataStream, compressedChunkSize, StreamExtensions.DefaultBufferSize);
                compressedDataStream.Position = 0;

                var decompressedDataBytes = ZLibCompression.Decompress(compressedDataStream, decompressedChunkSize);
                decompressedData.Write(decompressedDataBytes, 0, decompressedDataBytes.Length);
            }

            return decompressedData;
        }
    }
}