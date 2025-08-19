// ------------------------------------------------------------------------------
// <copyright file="BlteDecoderTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Compression;
using War3Net.IO.Casc.Enums;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class BlteDecoderTests
    {
        [TestMethod]
        public void TestBlteSignatureDetection()
        {
            // Valid BLTE data
            var validBlte = new byte[] { (byte)'B', (byte)'L', (byte)'T', (byte)'E', 0, 0, 0, 8 };
            Assert.IsTrue(BlteDecoder.IsBlte(validBlte));

            // Invalid data
            var invalidData = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            Assert.IsFalse(BlteDecoder.IsBlte(invalidData));

            // Too short
            var shortData = new byte[] { (byte)'B', (byte)'L' };
            Assert.IsFalse(BlteDecoder.IsBlte(shortData));
        }

        [TestMethod]
        public void TestBlteUncompressedData()
        {
            var testData = new byte[] { 1, 2, 3, 4, 5 };
            var blteData = CreateUncompressedBlte(testData);

            var decoded = BlteDecoder.Decode(blteData);
            CollectionAssert.AreEqual(testData, decoded);
        }

        [TestMethod]
        public void TestBlteCompressedData()
        {
            var testData = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var blteData = CreateCompressedBlte(testData);

            var decoded = BlteDecoder.Decode(blteData);
            CollectionAssert.AreEqual(testData, decoded);
        }

        [TestMethod]
        public void TestBlteStreamDecoding()
        {
            var testData = new byte[] { 10, 20, 30, 40, 50 };
            var blteData = CreateUncompressedBlte(testData);

            using var inputStream = new MemoryStream(blteData);
            using var outputStream = new MemoryStream();

            BlteDecoder.Decode(inputStream, outputStream);

            var decoded = outputStream.ToArray();
            CollectionAssert.AreEqual(testData, decoded);
        }

        [TestMethod]
        public void TestBlteFrameDecoding()
        {
            var testData = new byte[] { 100, 101, 102, 103, 104 };

            // Create a frame with uncompressed data
            var frame = new BlteFrame
            {
                CompressionType = CompressionType.None,
                Data = new byte[testData.Length + 1],
            };

            frame.Data[0] = (byte)CompressionType.None;
            Array.Copy(testData, 0, frame.Data, 1, testData.Length);

            var decoded = BlteDecoder.DecodeFrame(frame);
            CollectionAssert.AreEqual(testData, decoded);
        }

        [TestMethod]
        public void TestBlteEmptyData()
        {
            var blteData = CreateUncompressedBlte(Array.Empty<byte>());
            var decoded = BlteDecoder.Decode(blteData);
            Assert.AreEqual(0, decoded.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(CascParserException))]
        public void TestBlteInvalidSignature()
        {
            var invalidData = new byte[] { 0, 0, 0, 0, 0, 0, 0, 8, 0 };
            BlteDecoder.Decode(invalidData);
        }

        private static byte[] CreateUncompressedBlte(byte[] data)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Write BLTE header
            writer.Write((byte)'B');
            writer.Write((byte)'L');
            writer.Write((byte)'T');
            writer.Write((byte)'E');

            // Header size (big-endian)
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)9); // 8 bytes header + 1 byte flags

            // Flags (single chunk)
            writer.Write((byte)0);

            // Compression type
            writer.Write((byte)CompressionType.None);

            // Data
            writer.Write(data);

            return stream.ToArray();
        }

        private static byte[] CreateCompressedBlte(byte[] data)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            // Compress data with zlib
            byte[] compressedData;
            using (var compressStream = new MemoryStream())
            {
                using (var zlib = new Ionic.Zlib.ZlibStream(compressStream, Ionic.Zlib.CompressionMode.Compress))
                {
                    zlib.Write(data, 0, data.Length);
                }

                compressedData = compressStream.ToArray();
            }

            // Write BLTE header
            writer.Write((byte)'B');
            writer.Write((byte)'L');
            writer.Write((byte)'T');
            writer.Write((byte)'E');

            // Header size (big-endian)
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)9); // 8 bytes header + 1 byte flags

            // Flags (single chunk)
            writer.Write((byte)0);

            // Compression type
            writer.Write((byte)CompressionType.ZLib);

            // Compressed data
            writer.Write(compressedData);

            return stream.ToArray();
        }
    }
}