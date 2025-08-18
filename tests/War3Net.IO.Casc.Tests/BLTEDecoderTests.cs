// ------------------------------------------------------------------------------
// <copyright file="BLTEDecoderTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.IO.Compression;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Compression;
using War3Net.IO.Casc.Enums;
using War3Net.IO.Compression;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class BLTEDecoderTests
    {
        [TestMethod]
        public void TestBLTESignatureDetection()
        {
            // Valid BLTE data
            var validBLTE = new byte[] { (byte)'B', (byte)'L', (byte)'T', (byte)'E', 0, 0, 0, 8 };
            Assert.IsTrue(BLTEDecoder.IsBLTE(validBLTE));

            // Invalid data
            var invalidData = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            Assert.IsFalse(BLTEDecoder.IsBLTE(invalidData));

            // Too short
            var shortData = new byte[] { (byte)'B', (byte)'L' };
            Assert.IsFalse(BLTEDecoder.IsBLTE(shortData));
        }

        [TestMethod]
        public void TestBLTEUncompressedData()
        {
            var testData = new byte[] { 1, 2, 3, 4, 5 };
            var blteData = CreateUncompressedBLTE(testData);
            
            var decoded = BLTEDecoder.Decode(blteData);
            CollectionAssert.AreEqual(testData, decoded);
        }

        [TestMethod]
        public void TestBLTECompressedData()
        {
            var testData = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var blteData = CreateCompressedBLTE(testData);
            
            var decoded = BLTEDecoder.Decode(blteData);
            CollectionAssert.AreEqual(testData, decoded);
        }

        [TestMethod]
        public void TestBLTEStreamDecoding()
        {
            var testData = new byte[] { 10, 20, 30, 40, 50 };
            var blteData = CreateUncompressedBLTE(testData);
            
            using var inputStream = new MemoryStream(blteData);
            using var outputStream = new MemoryStream();
            
            BLTEDecoder.Decode(inputStream, outputStream);
            
            var decoded = outputStream.ToArray();
            CollectionAssert.AreEqual(testData, decoded);
        }

        [TestMethod]
        public void TestBLTEFrameDecoding()
        {
            var testData = new byte[] { 100, 101, 102, 103, 104 };
            
            // Create a frame with uncompressed data
            var frame = new BLTEFrame
            {
                CompressionType = CompressionType.None,
                Data = new byte[testData.Length + 1],
            };
            
            frame.Data[0] = (byte)CompressionType.None;
            Array.Copy(testData, 0, frame.Data, 1, testData.Length);
            
            var decoded = BLTEDecoder.DecodeFrame(frame);
            CollectionAssert.AreEqual(testData, decoded);
        }

        [TestMethod]
        public void TestBLTEEmptyData()
        {
            var blteData = CreateUncompressedBLTE(Array.Empty<byte>());
            var decoded = BLTEDecoder.Decode(blteData);
            Assert.AreEqual(0, decoded.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(CascParserException))]
        public void TestBLTEInvalidSignature()
        {
            var invalidData = new byte[] { 0, 0, 0, 0, 0, 0, 0, 8, 0 };
            BLTEDecoder.Decode(invalidData);
        }

        private static byte[] CreateUncompressedBLTE(byte[] data)
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

        private static byte[] CreateCompressedBLTE(byte[] data)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            
            // Compress data with zlib
            byte[] compressedData;
            using (var compressStream = new MemoryStream())
            {
                using (var zlib = new ZLibStream(compressStream, CompressionMode.Compress))
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