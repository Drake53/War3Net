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
            Assert.AreEqual(0, header.MustBe0F);
            Assert.IsFalse(header.IsMultiChunk);
            Assert.IsFalse(header.HasFrameHashes);
            Assert.AreEqual(0u, header.FrameCount);
            Assert.AreEqual(0, header.Frames.Count);
        }

        [TestMethod]
        public void TestParseMultiChunkHeader()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE signature (little-endian)
            writer.Write(BlteHeader.Signature);

            // Write header size (big-endian)
            // Header size = 4 (Signature) + 4 (HeaderSize) + 1 (MustBe0F) + 3 (frame count) + frames
            // For 2 frames without hashes: 12 + 2 * 8 = 28 bytes
            writer.Write(new byte[] { 0, 0, 0, 28 });

            // Write MustBe0F byte (must be 0x0F)
            writer.Write((byte)0x0F);

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
            Assert.AreEqual(28u, header.HeaderSize);
            Assert.AreEqual(0x0F, header.MustBe0F);
            Assert.IsTrue(header.IsMultiChunk);
            Assert.IsFalse(header.HasFrameHashes); // No hashes in this test
            Assert.AreEqual(2u, header.FrameCount);
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

            // Header size for 1 frame with hash: 12 + 1 * 24 = 36 bytes
            writer.Write(new byte[] { 0, 0, 0, 36 });

            // Write MustBe0F byte (must be 0x0F)
            writer.Write((byte)0x0F);

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
            Assert.AreEqual(36u, header.HeaderSize);
            Assert.AreEqual(0x0F, header.MustBe0F);
            Assert.IsTrue(header.IsMultiChunk);
            Assert.IsTrue(header.HasFrameHashes);
            Assert.AreEqual(1u, header.FrameCount);
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
        public void TestParseInvalidMustBe0F()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE signature (little-endian)
            writer.Write(BlteHeader.Signature);

            // Write header size (big-endian) - non-zero
            writer.Write(new byte[] { 0, 0, 0, 20 });

            // Write invalid MustBe0F byte (not 0x0F)
            writer.Write((byte)0xFF);

            stream.Position = 0;
            BlteHeader.Parse(stream);
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

            // Header size = 12 bytes (includes signature and header size field, no frames)
            writer.Write(new byte[] { 0, 0, 0, 12 });

            // Write MustBe0F byte (must be 0x0F)
            writer.Write((byte)0x0F);

            // Write frame count = 0
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)0);

            stream.Position = 0;
            var header = BlteHeader.Parse(stream);

            Assert.IsNotNull(header);
            Assert.AreEqual(12u, header.HeaderSize);
            Assert.AreEqual(0, header.Frames.Count);
            Assert.AreEqual(0u, header.FrameCount);
        }

        [TestMethod]
        public void TestParseMultipleFramesWithHashes()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE signature
            writer.Write(BlteHeader.Signature);

            // Header size for 3 frames with hashes: 12 + 3 * 24 = 84 bytes
            writer.Write(new byte[] { 0, 0, 0, 84 });

            // Write MustBe0F byte (must be 0x0F)
            writer.Write((byte)0x0F);

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
            Assert.AreEqual(84u, header.HeaderSize);
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
                MustBe0F = 0x0F,
                FrameCount = 5,
                HasFrameHashes = true,
            };

            Assert.AreEqual(100u, header.HeaderSize);
            Assert.AreEqual(0x0F, header.MustBe0F);
            Assert.AreEqual(5u, header.FrameCount);
            Assert.IsTrue(header.IsMultiChunk);
            Assert.IsTrue(header.HasFrameHashes);
            Assert.IsNotNull(header.Frames);
        }

        [TestMethod]
        public void TestHeaderSizeValidation()
        {
            // Test that header size validation matches expected format
            // HeaderSize should be: 1 (MustBe0F) + 3 (FrameCount) + frames
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE signature
            writer.Write(BlteHeader.Signature);

            // Test with 3 frames, no hashes
            // HeaderSize = 12 + (3 * 8) = 36 bytes
            writer.Write(new byte[] { 0, 0, 0, 36 });

            // MustBe0F
            writer.Write((byte)0x0F);

            // Frame count (3)
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)3);

            // Write 3 frames without hashes (8 bytes each)
            for (var i = 0; i < 3; i++)
            {
                writer.Write(new byte[] { 0, 0, 0, 100 }); // Encoded size
                writer.Write(new byte[] { 0, 0, 0, 200 }); // Content size
            }

            stream.Position = 0;
            var header = BlteHeader.Parse(stream);

            Assert.AreEqual(36u, header.HeaderSize);
            Assert.AreEqual(3u, header.FrameCount);
            Assert.IsFalse(header.HasFrameHashes);
            Assert.AreEqual(3, header.Frames.Count);
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
            var headerSize = 12 + (frameCount * 8); // 12 base bytes + frames (no hashes)

            // Write header size (big-endian)
            writer.Write((byte)((headerSize >> 24) & 0xFF));
            writer.Write((byte)((headerSize >> 16) & 0xFF));
            writer.Write((byte)((headerSize >> 8) & 0xFF));
            writer.Write((byte)(headerSize & 0xFF));

            // Write MustBe0F byte (must be 0x0F)
            writer.Write((byte)0x0F);

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
            Assert.AreEqual((uint)frameCount, header.FrameCount);
            Assert.AreEqual(frameCount, header.Frames.Count);
        }
    }
}