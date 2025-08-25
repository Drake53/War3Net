// ------------------------------------------------------------------------------
// <copyright file="MpqStreamUtilsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.TestTools.UnitTesting;

namespace War3Net.IO.Mpq.Tests
{
    [TestClass]
    public class MpqStreamUtilsTests
    {
        private const int BlockSize = 0x200 << MpqArchiveCreateOptions.DefaultBlockSize;
        private const string FileName = "Hello world.png";
        private const byte FixedInputByte = 100;
        private const uint FileEncryptionOffset = 10000;

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestCompressMpqStream(int fileSize, bool randomizedInput)
        {
            var input = randomizedInput ? RandomNumberGenerator.GetBytes(fileSize) : Enumerable.Repeat(FixedInputByte, fileSize).ToArray();
            using var inputStream = new MemoryStream(input);

            using var compressedStream = MpqStreamUtils.Compress(inputStream, null, BlockSize);

            using var mpqStream = MpqStreamFactory.FromStream(compressedStream, null, (uint)input.Length, BlockSize, false, null);

            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestEncryptMpqStream(int fileSize, bool randomizedInput)
        {
            var input = randomizedInput ? RandomNumberGenerator.GetBytes(fileSize) : Enumerable.Repeat(FixedInputByte, fileSize).ToArray();
            using var inputStream = new MemoryStream(input);

            var encryptionSeed = MpqEncryptionUtils.CalculateEncryptionSeed(FileName);
            using var encryptedStream = MpqStreamUtils.Encrypt(inputStream, encryptionSeed, null, BlockSize, null);

            using var mpqStream = MpqStreamFactory.FromStream(encryptedStream, FileName, null, BlockSize, true, null);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqStream.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestCompressAndEncryptMpqStream(int fileSize, bool randomizedInput)
        {
            var input = randomizedInput ? RandomNumberGenerator.GetBytes(fileSize) : Enumerable.Repeat(FixedInputByte, fileSize).ToArray();
            using var inputStream = new MemoryStream(input);

            using var compressedStream = MpqStreamUtils.Compress(inputStream, null, BlockSize);

            var encryptionSeed = MpqEncryptionUtils.CalculateEncryptionSeed(FileName);
            var uncompressedSize = (uint)input.Length;
            using var encryptedStream = MpqStreamUtils.Encrypt(compressedStream, encryptionSeed, null, BlockSize, uncompressedSize);

            using var mpqStream = MpqStreamFactory.FromStream(encryptedStream, FileName, uncompressedSize, BlockSize, true, null);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqStream.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestEncryptMpqStreamWithOffsetAdjustedKey(int fileSize, bool randomizedInput)
        {
            var input = randomizedInput ? RandomNumberGenerator.GetBytes(fileSize) : Enumerable.Repeat(FixedInputByte, fileSize).ToArray();
            using var inputStream = new MemoryStream(input);

            var encryptionSeed = MpqEncryptionUtils.CalculateEncryptionSeed(FileName);
            using var encryptedStream = MpqStreamUtils.Encrypt(inputStream, encryptionSeed, FileEncryptionOffset, BlockSize, null);

            using var mpqStream = MpqStreamFactory.FromStream(encryptedStream, FileName, null, BlockSize, true, FileEncryptionOffset);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqStream.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestCompressAndEncryptMpqStreamWithOffsetAdjustedKey(int fileSize, bool randomizedInput)
        {
            var input = randomizedInput ? RandomNumberGenerator.GetBytes(fileSize) : Enumerable.Repeat(FixedInputByte, fileSize).ToArray();
            using var inputStream = new MemoryStream(input);

            using var compressedStream = MpqStreamUtils.Compress(inputStream, null, BlockSize);

            var encryptionSeed = MpqEncryptionUtils.CalculateEncryptionSeed(FileName);
            var uncompressedSize = (uint)input.Length;
            using var encryptedStream = MpqStreamUtils.Encrypt(compressedStream, encryptionSeed, FileEncryptionOffset, BlockSize, uncompressedSize);

            using var mpqStream = MpqStreamFactory.FromStream(encryptedStream, FileName, uncompressedSize, BlockSize, true, FileEncryptionOffset);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqStream.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestCompressMpqStreamSingleUnit(int fileSize, bool randomizedInput)
        {
            var input = randomizedInput ? RandomNumberGenerator.GetBytes(fileSize) : Enumerable.Repeat(FixedInputByte, fileSize).ToArray();
            using var inputStream = new MemoryStream(input);

            using var compressedStream = MpqStreamUtils.Compress(inputStream, null, null);

            var mpqEntry = new MpqEntry(null, 0, 0, (uint)compressedStream.Length, (uint)input.Length, MpqFileFlags.Exists | MpqFileFlags.SingleUnit | MpqFileFlags.CompressedMulti);
            using var mpqStream = MpqStreamFactory.FromStream(compressedStream, null, (uint)input.Length, null, false, null);

            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestEncryptMpqStreamSingleUnit(int fileSize, bool randomizedInput)
        {
            var input = randomizedInput ? RandomNumberGenerator.GetBytes(fileSize) : Enumerable.Repeat(FixedInputByte, fileSize).ToArray();
            using var inputStream = new MemoryStream(input);

            var encryptionSeed = MpqEncryptionUtils.CalculateEncryptionSeed(FileName);
            using var encryptedStream = MpqStreamUtils.Encrypt(inputStream, encryptionSeed, null, null, null);

            using var mpqStream = MpqStreamFactory.FromStream(encryptedStream, FileName, null, null, true, null);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqStream.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestCompressAndEncryptMpqStreamSingleUnit(int fileSize, bool randomizedInput)
        {
            var input = randomizedInput ? RandomNumberGenerator.GetBytes(fileSize) : Enumerable.Repeat(FixedInputByte, fileSize).ToArray();
            using var inputStream = new MemoryStream(input);

            using var compressedStream = MpqStreamUtils.Compress(inputStream, null, null);

            var encryptionSeed = MpqEncryptionUtils.CalculateEncryptionSeed(FileName);
            var uncompressedSize = (uint)input.Length;
            using var encryptedStream = MpqStreamUtils.Encrypt(compressedStream, encryptionSeed, null, null, uncompressedSize);

            using var mpqStream = MpqStreamFactory.FromStream(encryptedStream, FileName, uncompressedSize, null, true, null);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqStream.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestEncryptMpqStreamSingleUnitWithOffsetAdjustedKey(int fileSize, bool randomizedInput)
        {
            var input = randomizedInput ? RandomNumberGenerator.GetBytes(fileSize) : Enumerable.Repeat(FixedInputByte, fileSize).ToArray();
            using var inputStream = new MemoryStream(input);

            var encryptionSeed = MpqEncryptionUtils.CalculateEncryptionSeed(FileName);
            using var encryptedStream = MpqStreamUtils.Encrypt(inputStream, encryptionSeed, FileEncryptionOffset, null, null);

            using var mpqStream = MpqStreamFactory.FromStream(encryptedStream, FileName, null, null, true, FileEncryptionOffset);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqStream.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestCompressAndEncryptMpqStreamSingleUnitWithOffsetAdjustedKey(int fileSize, bool randomizedInput)
        {
            var input = randomizedInput ? RandomNumberGenerator.GetBytes(fileSize) : Enumerable.Repeat(FixedInputByte, fileSize).ToArray();
            using var inputStream = new MemoryStream(input);

            using var compressedStream = MpqStreamUtils.Compress(inputStream, null, null);

            var encryptionSeed = MpqEncryptionUtils.CalculateEncryptionSeed(FileName);
            var uncompressedSize = (uint)input.Length;
            using var encryptedStream = MpqStreamUtils.Encrypt(compressedStream, encryptionSeed, FileEncryptionOffset, null, uncompressedSize);

            using var mpqStream = MpqStreamFactory.FromStream(encryptedStream, FileName, uncompressedSize, null, true, FileEncryptionOffset);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqStream.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        private static IEnumerable<object?[]> GetTestData()
        {
            yield return new object?[] { 0, true };
            yield return new object?[] { 1, true };
            yield return new object?[] { 2, true };
            yield return new object?[] { 3, true };
            yield return new object?[] { 4, true };
            yield return new object?[] { 5, true };
            yield return new object?[] { 100, true };
            yield return new object?[] { 1000, true };
            yield return new object?[] { 4095, true };
            yield return new object?[] { 4096, true };
            yield return new object?[] { 4097, true };
            yield return new object?[] { 4098, true };
            yield return new object?[] { 4099, true };
            yield return new object?[] { 4100, true };
            yield return new object?[] { 10000, true };

            yield return new object?[] { 0, false };
            yield return new object?[] { 1, false };
            yield return new object?[] { 2, false };
            yield return new object?[] { 3, false };
            yield return new object?[] { 4, false };
            yield return new object?[] { 5, false };
            yield return new object?[] { 100, false };
            yield return new object?[] { 1000, false };
            yield return new object?[] { 4095, false };
            yield return new object?[] { 4096, false };
            yield return new object?[] { 4097, false };
            yield return new object?[] { 4098, false };
            yield return new object?[] { 4099, false };
            yield return new object?[] { 4100, false };
            yield return new object?[] { 10000, false };
        }
    }
}