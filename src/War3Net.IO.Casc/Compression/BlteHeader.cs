// ------------------------------------------------------------------------------
// <copyright file="BlteHeader.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using War3Net.IO.Casc.Utilities;

namespace War3Net.IO.Casc.Compression
{
    /// <summary>
    /// Represents the header of BLTE-encoded data.
    /// </summary>
    public class BlteHeader
    {
        /// <summary>
        /// The BLTE signature.
        /// </summary>
        public const uint Signature = 0x45544C42; // 'BLTE'

        /// <summary>
        /// Initializes a new instance of the <see cref="BlteHeader"/> class.
        /// </summary>
        public BlteHeader()
        {
            Frames = new List<BlteFrame>();
        }

        /// <summary>
        /// Gets or sets the header size.
        /// </summary>
        public uint HeaderSize { get; set; }

        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        public byte Flags { get; set; }

        /// <summary>
        /// Gets or sets the chunk count (if multi-chunk).
        /// </summary>
        public uint ChunkCount { get; set; }

        /// <summary>
        /// Gets the list of frames.
        /// </summary>
        public List<BlteFrame> Frames { get; }

        /// <summary>
        /// Gets a value indicating whether this BLTE data has multiple chunks.
        /// </summary>
        public bool IsMultiChunk => (Flags & 0xF0) != 0;

        /// <summary>
        /// Gets a value indicating whether frame hashes are present.
        /// </summary>
        public bool HasFrameHashes => (Flags & 0x10) != 0;

        /// <summary>
        /// Parses a BLTE header from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The parsed header.</returns>
        public static BlteHeader Parse(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.UTF8, true);
            return Parse(reader);
        }

        /// <summary>
        /// Parses a BLTE header from a binary reader.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <returns>The parsed header.</returns>
        public static BlteHeader Parse(BinaryReader reader)
        {
            var startPos = reader.BaseStream.Position;

            // Read and verify signature
            var signature = reader.ReadUInt32BE();
            if (signature != Signature)
            {
                throw new CascParserException($"Invalid BLTE signature: 0x{signature:X8}, expected 0x{Signature:X8}");
            }

            var header = new BlteHeader
            {
                HeaderSize = reader.ReadUInt32BE(),
                Flags = reader.ReadByte(),
            };

            if (header.IsMultiChunk)
            {
                // Multi-chunk format
                var chunkCount = reader.ReadByte() << 16;
                chunkCount |= reader.ReadByte() << 8;
                chunkCount |= reader.ReadByte();
                header.ChunkCount = (uint)chunkCount;

                // Read frame info
                for (uint i = 0; i < header.ChunkCount; i++)
                {
                    var frame = new BlteFrame
                    {
                        EncodedSize = reader.ReadUInt32BE(),
                        ContentSize = reader.ReadUInt32BE(),
                    };

                    if (header.HasFrameHashes)
                    {
                        frame.Hash = reader.ReadMD5Hash();
                    }

                    header.Frames.Add(frame);
                }
            }
            else
            {
                // Single chunk - create a single frame entry
                // The frame size will be determined from the data
                header.ChunkCount = 1;
                header.Frames.Add(new BlteFrame());
            }

            // Ensure we've read the entire header
            var bytesRead = reader.BaseStream.Position - startPos;
            if (bytesRead < header.HeaderSize)
            {
                reader.Skip((int)(header.HeaderSize - bytesRead));
            }

            return header;
        }

        /// <summary>
        /// Writes the header to a binary writer.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        public void WriteTo(BinaryWriter writer)
        {
            writer.WriteUInt32BE(Signature);
            writer.WriteUInt32BE(HeaderSize);
            writer.Write(Flags);

            if (IsMultiChunk)
            {
                // Write 24-bit chunk count
                writer.Write((byte)((ChunkCount >> 16) & 0xFF));
                writer.Write((byte)((ChunkCount >> 8) & 0xFF));
                writer.Write((byte)(ChunkCount & 0xFF));

                // Write frame info
                foreach (var frame in Frames)
                {
                    writer.WriteUInt32BE(frame.EncodedSize);
                    writer.WriteUInt32BE(frame.ContentSize);

                    if (HasFrameHashes && frame.Hash != null)
                    {
                        writer.WriteMD5Hash(frame.Hash);
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the header size.
        /// </summary>
        /// <returns>The calculated header size.</returns>
        public uint CalculateHeaderSize()
        {
            uint size = 8 + 1; // Signature + HeaderSize + Flags

            if (IsMultiChunk)
            {
                size += 3; // 24-bit chunk count
                size += (uint)(Frames.Count * 8); // EncodedSize + ContentSize per frame

                if (HasFrameHashes)
                {
                    size += (uint)(Frames.Count * CascConstants.MD5HashSize);
                }
            }

            return size;
        }
    }
}