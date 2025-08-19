// ------------------------------------------------------------------------------
// <copyright file="BlteHeaderTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Compression;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class BlteHeaderTests
    {
        [TestMethod]
        public void TestParseSingleChunkHeader()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE signature (little-endian)
            writer.Write(BlteHeader.Signature);

            // Write header size (big-endian) - 0 means single chunk
            writer.Write(new byte[] { 0, 0, 0, 0 });

            stream.Position = 0;
            var header = BlteHeader.Parse(stream);

            Assert.IsNotNull(header);
            Assert.AreEqual(0u, header.HeaderSize);
            Assert.AreEqual(0, header.Flags);
            Assert.IsFalse(header.IsMultiChunk);
            Assert.IsFalse(header.HasFrameHashes);
            Assert.AreEqual(0, header.Frames.Count);
        }

        [TestMethod]
        public void TestParseMultiChunkHeader()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE signature (little-endian)
            writer.Write(BlteHeader.Signature);

            // Write header size (big-endian) - includes flags byte and frame count
            // Header size = 1 (flags) + 3 (frame count) + frames
            // For 2 frames: 4 + 2 * 8 = 20 bytes
            writer.Write(new byte[] { 0, 0, 0, 20 });

            // Write flags (0x80 = multi-chunk bit set, no frame hashes)
            writer.Write((byte)0x80);

            // Write frame count (big-endian, 3 bytes)
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)2);

            // Write frame 1 (8 bytes each: 4 encoded size + 4 content size)
            writer.Write(new byte[] { 0, 0, 1, 0 }); // Encoded size = 256 (big-endian)
            writer.Write(new byte[] { 0, 0, 2, 0 }); // Content size = 512 (big-endian)

            // Write frame 2
            writer.Write(new byte[] { 0, 0, 0, 100 }); // Encoded size = 100 (big-endian)
            writer.Write(new byte[] { 0, 0, 0, 200 }); // Content size = 200 (big-endian)

            stream.Position = 0;
            var header = BlteHeader.Parse(stream);

            Assert.IsNotNull(header);
            Assert.AreEqual(20u, header.HeaderSize);
            Assert.AreEqual(0x80, header.Flags);
            Assert.IsTrue(header.IsMultiChunk);
            Assert.IsFalse(header.HasFrameHashes); // 0x80 doesn't include the 0x10 bit
            Assert.AreEqual(2u, header.ChunkCount);
            Assert.AreEqual(2, header.Frames.Count);

            // Verify frame 1
            Assert.AreEqual(256u, header.Frames[0].EncodedSize);
            Assert.AreEqual(512u, header.Frames[0].ContentSize);

            // Verify frame 2
            Assert.AreEqual(100u, header.Frames[1].EncodedSize);
            Assert.AreEqual(200u, header.Frames[1].ContentSize);
        }

        [TestMethod]
        public void TestParseHeaderWithFrameHashes()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE signature (little-endian)
            writer.Write(BlteHeader.Signature);

            // Header size for 1 frame with hash: 1 (flags) + 3 (frame count) + 1 * (8 + 16) = 28 bytes
            writer.Write(new byte[] { 0, 0, 0, 28 });

            // Write flags (0x10 = has frame hashes, also sets multi-chunk bit)
            writer.Write((byte)0x10);

            // Write frame count (1 frame)
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)1);

            // Write frame with hash
            writer.Write(new byte[] { 0, 0, 0, 50 }); // Encoded size
            writer.Write(new byte[] { 0, 0, 0, 75 }); // Content size

            // Write frame hash (16 bytes MD5)
            for (var i = 0; i < 16; i++)
            {
                writer.Write((byte)i);
            }

            stream.Position = 0;
            var header = BlteHeader.Parse(stream);

            Assert.IsNotNull(header);
            Assert.AreEqual(28u, header.HeaderSize);
            Assert.AreEqual(0x10, header.Flags);
            Assert.IsTrue(header.IsMultiChunk);
            Assert.IsTrue(header.HasFrameHashes);
            Assert.AreEqual(1u, header.ChunkCount);
            Assert.AreEqual(1, header.Frames.Count);

            // Verify frame
            var frame = header.Frames[0];
            Assert.AreEqual(50u, frame.EncodedSize);
            Assert.AreEqual(75u, frame.ContentSize);
            Assert.IsNotNull(frame.Hash);
            Assert.AreEqual(16, frame.Hash.Length);

            // Verify hash bytes
            for (var i = 0; i < 16; i++)
            {
                Assert.AreEqual((byte)i, frame.Hash[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CascParserException))]
        public void TestParseInvalidSignature()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write invalid signature
            writer.Write(0x12345678);
            writer.Write(new byte[] { 0, 0, 0, 0 });

            stream.Position = 0;
            BlteHeader.Parse(stream);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void TestParseTruncatedHeader()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE signature only
            writer.Write(BlteHeader.Signature);
            // Missing header size

            stream.Position = 0;
            BlteHeader.Parse(stream);
        }

        [TestMethod]
        public void TestParseEmptyFrameList()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE signature
            writer.Write(BlteHeader.Signature);

            // Header size = 4 bytes (just flags and frame count, no frames)
            writer.Write(new byte[] { 0, 0, 0, 4 });

            // Write flags (multi-chunk but no frames)
            writer.Write((byte)0x80);

            // Write frame count = 0
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)0);

            stream.Position = 0;
            var header = BlteHeader.Parse(stream);

            Assert.IsNotNull(header);
            Assert.AreEqual(4u, header.HeaderSize);
            Assert.AreEqual(0, header.Frames.Count);
            Assert.AreEqual(0u, header.ChunkCount);
        }

        [TestMethod]
        public void TestParseMultipleFramesWithHashes()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE signature
            writer.Write(BlteHeader.Signature);

            // Header size for 3 frames with hashes: 1 (flags) + 3 (frame count) + 3 * 24 = 76 bytes
            writer.Write(new byte[] { 0, 0, 0, 76 });

            // Flags with hash bit
            writer.Write((byte)0x10);

            // Frame count = 3
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)3);

            // Write 3 frames
            for (var frame = 0; frame < 3; frame++)
            {
                writer.Write(new byte[] { 0, 0, (byte)(frame + 1), 0 }); // Encoded size
                writer.Write(new byte[] { 0, 0, (byte)(frame + 2), 0 }); // Content size

                // Frame hash
                for (var i = 0; i < 16; i++)
                {
                    writer.Write((byte)(frame * 16 + i));
                }
            }

            stream.Position = 0;
            var header = BlteHeader.Parse(stream);

            Assert.IsNotNull(header);
            Assert.AreEqual(76u, header.HeaderSize);
            Assert.AreEqual(3, header.Frames.Count);

            // Verify each frame
            for (var frame = 0; frame < 3; frame++)
            {
                Assert.AreEqual((uint)((frame + 1) * 256), header.Frames[frame].EncodedSize);
                Assert.AreEqual((uint)((frame + 2) * 256), header.Frames[frame].ContentSize);

                // Verify hash
                for (var i = 0; i < 16; i++)
                {
                    Assert.AreEqual((byte)(frame * 16 + i), header.Frames[frame].Hash![i]);
                }
            }
        }

        [TestMethod]
        public void TestBlteFrameProperties()
        {
            var frame = new BlteFrame
            {
                EncodedSize = 1024,
                ContentSize = 2048,
                CompressionType = Enums.CompressionType.ZLib,
                Data = new byte[] { 1, 2, 3, 4, 5 },
                Hash = new byte[16],
            };

            Assert.AreEqual(1024u, frame.EncodedSize);
            Assert.AreEqual(2048u, frame.ContentSize);
            Assert.AreEqual(Enums.CompressionType.ZLib, frame.CompressionType);
            Assert.AreEqual(5, frame.Data.Length);
            Assert.AreEqual(16, frame.Hash.Length);
        }

        [TestMethod]
        public void TestBlteHeaderProperties()
        {
            var header = new BlteHeader
            {
                HeaderSize = 100,
                Flags = 0x1F,
                ChunkCount = 5,
            };

            Assert.AreEqual(100u, header.HeaderSize);
            Assert.AreEqual(0x1F, header.Flags);
            Assert.AreEqual(5u, header.ChunkCount);
            Assert.IsTrue(header.IsMultiChunk);
            Assert.IsTrue(header.HasFrameHashes);
            Assert.IsNotNull(header.Frames);
        }

        [TestMethod]
        public void TestParseLargeFrameCount()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE signature
            writer.Write(BlteHeader.Signature);

            // Large frame count (max 3-byte value)
            var frameCount = 1000;
            var headerSize = 1 + 3 + (frameCount * 8); // flags + frame count + frames (no hashes)

            // Write header size (big-endian)
            writer.Write((byte)((headerSize >> 24) & 0xFF));
            writer.Write((byte)((headerSize >> 16) & 0xFF));
            writer.Write((byte)((headerSize >> 8) & 0xFF));
            writer.Write((byte)(headerSize & 0xFF));

            // Flags (multi-chunk, no hashes)
            writer.Write((byte)0x80);

            // Frame count (3 bytes, big-endian)
            writer.Write((byte)((frameCount >> 16) & 0xFF));
            writer.Write((byte)((frameCount >> 8) & 0xFF));
            writer.Write((byte)(frameCount & 0xFF));

            // Write frames
            for (var i = 0; i < frameCount; i++)
            {
                writer.Write(new byte[] { 0, 0, 1, 0 }); // Encoded size
                writer.Write(new byte[] { 0, 0, 2, 0 }); // Content size
            }

            stream.Position = 0;
            var header = BlteHeader.Parse(stream);

            Assert.IsNotNull(header);
            Assert.AreEqual((uint)headerSize, header.HeaderSize);
            Assert.AreEqual((uint)frameCount, header.ChunkCount);
            Assert.AreEqual(frameCount, header.Frames.Count);
        }
    }
}