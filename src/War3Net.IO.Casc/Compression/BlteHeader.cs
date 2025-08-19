// ------------------------------------------------------------------------------
// <copyright file="BlteHeader.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public const uint Signature = 0x45544C42; // 'BLTE' read as little-endian

        /// <summary>
        /// The required value for the MustBe0F field when HeaderSize != 0.
        /// </summary>
        public const byte MustBe0FValue = 0x0F;

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
        /// Gets or sets the MustBe0F byte (must be 0x0F when HeaderSize != 0).
        /// </summary>
        public byte MustBe0F { get; set; }

        /// <summary>
        /// Gets or sets the frame count.
        /// </summary>
        public uint FrameCount { get; set; }

        /// <summary>
        /// Gets the list of frames.
        /// </summary>
        public List<BlteFrame> Frames { get; }

        /// <summary>
        /// Gets a value indicating whether this BLTE data has multiple chunks.
        /// </summary>
        public bool IsMultiChunk => HeaderSize != 0;

        /// <summary>
        /// Gets or sets a value indicating whether frame hashes are present.
        /// </summary>
        /// <remarks>
        /// This is determined by the header size during parsing.
        /// </remarks>
        public bool HasFrameHashes { get; set; }

        /// <summary>
        /// Parses a BLTE header from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The parsed header.</returns>
        public static BlteHeader Parse(Stream stream)
        {
            using var reader = new BinaryReader(stream, System.Text.Encoding.UTF8, true);
            return Parse(reader);
        }

        /// <summary>
        /// Parses a BLTE header from a binary reader.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <returns>The parsed header.</returns>
        public static BlteHeader Parse(BinaryReader reader)
        {
            // Read and verify signature (little-endian)
            var signature = reader.ReadUInt32();
            if (signature != Signature)
            {
                throw new CascParserException($"Invalid BLTE signature: 0x{signature:X8}, expected 0x{Signature:X8}");
            }

            var header = new BlteHeader
            {
                HeaderSize = reader.ReadUInt32BE(),
            };

            // If HeaderSize is 0, this is a single-chunk BLTE with no additional header
            if (header.HeaderSize == 0)
            {
                header.MustBe0F = 0;
                header.FrameCount = 0;

                // Single chunk - no frame metadata
                return header;
            }

            // Read and verify MustBe0F byte (must be 0x0F when HeaderSize != 0)
            header.MustBe0F = reader.ReadByte();
            if (header.MustBe0F != MustBe0FValue)
            {
                throw new CascParserException($"Invalid MustBe0F value: 0x{header.MustBe0F:X2}, expected 0x{MustBe0FValue:X2}");
            }

            // Read frame count (3 bytes, big-endian)
            header.FrameCount = (uint)((reader.ReadByte() << 16) | (reader.ReadByte() << 8) | reader.ReadByte());

            // Verify header size matches expected size
            // Expected: 4 bytes (MustBe0F + FrameCount) + FrameCount * sizeof(BLTE_FRAME)
            // Note: sizeof(BLTE_FRAME) = 8 bytes (EncodedSize + ContentSize) + 16 bytes (MD5 hash) = 24 bytes
            var expectedHeaderSize = 4 + (header.FrameCount * 24);
            if (header.HeaderSize != expectedHeaderSize)
            {
                // Some BLTE files may not have frame hashes, so check without them
                expectedHeaderSize = 4 + (header.FrameCount * 8);
                if (header.HeaderSize != expectedHeaderSize)
                {
                    throw new CascParserException($"Invalid header size: {header.HeaderSize}, expected {4 + (header.FrameCount * 24)} or {expectedHeaderSize}");
                }
            }

            // Determine if frames have hashes based on header size
            header.HasFrameHashes = header.HeaderSize == (4 + (header.FrameCount * 24));

            // Read frame info
            for (uint i = 0; i < header.FrameCount; i++)
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

            return header;
        }

        /// <summary>
        /// Writes the header to a binary writer.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        public void WriteTo(BinaryWriter writer)
        {
            // Write signature (little-endian)
            writer.Write(Signature);

            // Write header size (big-endian)
            writer.WriteUInt32BE(HeaderSize);

            if (HeaderSize != 0)
            {
                // Write MustBe0F byte
                writer.Write(MustBe0FValue);

                // Write frame count (3 bytes, big-endian)
                writer.Write((byte)((FrameCount >> 16) & 0xFF));
                writer.Write((byte)((FrameCount >> 8) & 0xFF));
                writer.Write((byte)(FrameCount & 0xFF));

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
            if (FrameCount == 0)
            {
                return 0;
            }

            // 4 bytes (MustBe0F + FrameCount) + frames
            uint size = 4;

            // Add frame sizes (8 bytes per frame for EncodedSize + ContentSize)
            size += FrameCount * 8;

            // Check if all frames have hashes
            if (Frames.Count > 0 && Frames.All(f => f.Hash != null && f.Hash.Length == CascConstants.MD5HashSize))
            {
                size += FrameCount * CascConstants.MD5HashSize;
            }

            return size;
        }
    }
}