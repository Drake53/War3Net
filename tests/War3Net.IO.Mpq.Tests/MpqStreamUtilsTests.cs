// ------------------------------------------------------------------------------
// <copyright file="MpqStreamUtilsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
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

        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestCompressMpqStream(int fileSize)
        {
            var input = RandomNumberGenerator.GetBytes(fileSize);
            using var inputStream = new MemoryStream(input);

            using var compressedStream = MpqStreamUtils.Compress(inputStream, null, BlockSize);

            var mpqEntry = new MpqEntry(null, 0, 0, (uint)compressedStream.Length, (uint)input.Length, MpqFileFlags.Exists | MpqFileFlags.Compressed);
            using var mpqStream = new MpqStream(mpqEntry, compressedStream, BlockSize);

            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestEncryptMpqStream(int fileSize)
        {
            var input = RandomNumberGenerator.GetBytes(fileSize);
            using var inputStream = new MemoryStream(input);

            var encryptionSeed = MpqEntry.CalculateEncryptionSeed(FileName);
            using var encryptedStream = MpqStreamUtils.Encrypt(inputStream, encryptionSeed, null, BlockSize, null);

            var mpqEntry = new MpqEntry(FileName, 0, 0, (uint)encryptedStream.Length, (uint)input.Length, MpqFileFlags.Exists | MpqFileFlags.Encrypted);
            using var mpqStream = new MpqStream(mpqEntry, encryptedStream, BlockSize);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqEntry.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestCompressAndEncryptMpqStream(int fileSize)
        {
            var input = RandomNumberGenerator.GetBytes(fileSize);
            using var inputStream = new MemoryStream(input);

            using var compressedStream = MpqStreamUtils.Compress(inputStream, null, BlockSize);

            var encryptionSeed = MpqEntry.CalculateEncryptionSeed(FileName);
            var uncompressedSize = (uint)input.Length;
            using var encryptedStream = MpqStreamUtils.Encrypt(compressedStream, encryptionSeed, null, BlockSize, uncompressedSize);

            var mpqEntry = new MpqEntry(FileName, 0, 0, (uint)encryptedStream.Length, uncompressedSize, MpqFileFlags.Exists | MpqFileFlags.Compressed | MpqFileFlags.Encrypted);
            using var mpqStream = new MpqStream(mpqEntry, encryptedStream, BlockSize);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqEntry.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestCompressMpqStreamSingleUnit(int fileSize)
        {
            var input = RandomNumberGenerator.GetBytes(fileSize);
            using var inputStream = new MemoryStream(input);

            using var compressedStream = MpqStreamUtils.Compress(inputStream, null, null);

            var mpqEntry = new MpqEntry(null, 0, 0, (uint)compressedStream.Length, (uint)input.Length, MpqFileFlags.Exists | MpqFileFlags.SingleUnit | MpqFileFlags.Compressed);
            using var mpqStream = new MpqStream(mpqEntry, compressedStream, BlockSize);

            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestEncryptMpqStreamSingleUnit(int fileSize)
        {
            var input = RandomNumberGenerator.GetBytes(fileSize);
            using var inputStream = new MemoryStream(input);

            var encryptionSeed = MpqEntry.CalculateEncryptionSeed(FileName);
            using var encryptedStream = MpqStreamUtils.Encrypt(inputStream, encryptionSeed, null, null, null);

            var mpqEntry = new MpqEntry(FileName, 0, 0, (uint)encryptedStream.Length, (uint)input.Length, MpqFileFlags.Exists | MpqFileFlags.SingleUnit | MpqFileFlags.Encrypted);
            using var mpqStream = new MpqStream(mpqEntry, encryptedStream, BlockSize);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqEntry.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestCompressAndEncryptMpqStreamSingleUnit(int fileSize)
        {
            var input = RandomNumberGenerator.GetBytes(fileSize);
            using var inputStream = new MemoryStream(input);

            using var compressedStream = MpqStreamUtils.Compress(inputStream, null, null);

            var encryptionSeed = MpqEntry.CalculateEncryptionSeed(FileName);
            var uncompressedSize = (uint)input.Length;
            using var encryptedStream = MpqStreamUtils.Encrypt(compressedStream, encryptionSeed, null, null, uncompressedSize);

            var mpqEntry = new MpqEntry(FileName, 0, 0, (uint)encryptedStream.Length, uncompressedSize, MpqFileFlags.Exists | MpqFileFlags.SingleUnit | MpqFileFlags.Compressed | MpqFileFlags.Encrypted);
            using var mpqStream = new MpqStream(mpqEntry, encryptedStream, BlockSize);

            Assert.IsTrue(mpqStream.CanRead, "Unable to decrypt stream.");
            Assert.AreEqual(encryptionSeed, mpqEntry.BaseEncryptionSeed);
            StreamAssert.AreEqual(inputStream, mpqStream, true, false);
        }

        private static IEnumerable<object?[]> GetTestData()
        {
            yield return new object?[] { 0 };
            yield return new object?[] { 100 };
            yield return new object?[] { 1000 };
            yield return new object?[] { 4095 };
            yield return new object?[] { 4096 };
            yield return new object?[] { 4097 };
            yield return new object?[] { 4098 };
            yield return new object?[] { 4099 };
            yield return new object?[] { 4100 };
            yield return new object?[] { 10000 };
        }
    }
}