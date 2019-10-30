// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.IO.Mpq.Tests
{
    [TestClass]
    public class MpqArchiveTest
    {
        private const ushort BlockSize = 3;

        [DataTestMethod]
        [DynamicData(nameof(GetTestFiles), DynamicDataSourceType.Method)]
        public void TestStoreThenRetrieveFile(string filename)
        {
            var fileStream = File.OpenRead(filename);
            // var mpqFile = new MpqFile(fileStream, filename, MpqLocale.Neutral, MpqFileFlags.Exists, BlockSize);
            var mpqFile = new MpqKnownFile(filename, fileStream, MpqFileFlags.Exists, MpqLocale.Neutral);
            var archive = MpqArchive.Create(new MemoryStream(), new List<MpqFile>() { mpqFile });

            var openedArchive = MpqArchive.Open(archive.BaseStream);
            var openedStream = openedArchive.OpenFile(filename);

            // Reload file, since the filestream gets disposed when the mpqfile is added to an mpqarchive.
            fileStream = File.OpenRead(filename);

            StreamAssert.AreEqual(fileStream, openedStream);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestFilesAndFlags), DynamicDataSourceType.Method)]
        public void TestStoreThenRetrieveFileWithFlags(string filename, MpqFileFlags flags)
        {
            var fileStream = File.OpenRead(filename);
            // var mpqFile = new MpqFile(fileStream, filename, MpqLocale.Neutral, flags, BlockSize);
            var mpqFile = new MpqKnownFile(filename, fileStream, flags, MpqLocale.Neutral);
            var archive = MpqArchive.Create(new MemoryStream(), new List<MpqFile>() { mpqFile }, blockSize: BlockSize);

            var openedArchive = MpqArchive.Open(archive.BaseStream);
            var openedStream = openedArchive.OpenFile(filename);

            // Reload file, since the filestream gets disposed when the mpqfile is added to an mpqarchive.
            fileStream = File.OpenRead(filename);

            StreamAssert.AreEqual(fileStream, openedStream);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestFlags), DynamicDataSourceType.Method)]
        public void TestStoreThenRetrieveEmptyFileWithFlags(MpqFileFlags flags)
        {
            const string FileName = "someRandomFile.empty";

            // var mpqFile = new MpqFile(null, FileName, MpqLocale.Neutral, flags, BlockSize);
            var mpqFile = new MpqKnownFile(FileName, null, flags, MpqLocale.Neutral);
            var archive = MpqArchive.Create(new MemoryStream(), new List<MpqFile>() { mpqFile }, blockSize: BlockSize);

            var openedArchive = MpqArchive.Open(archive.BaseStream);
            var openedStream = openedArchive.OpenFile(FileName);

            Assert.IsTrue(openedStream.Length == 0);
        }

        [TestMethod]
        public void TestRecreatePKCompressed()
        {
            const string inputArchivePath = @"TestArchives\PKCompressed.w3x";

            using var inputArchive = MpqArchive.Open(inputArchivePath);
            using var recreatedArchive = MpqArchive.Create(
                (Stream?)null,
                inputArchive.GetMpqFiles().ToArray(),
                (ushort)inputArchive.Header.HashTableSize,
                inputArchive.Header.BlockSize);

            for (var i = 0; i < inputArchive.Header.BlockTableSize; i++)
            {
                inputArchive.BaseStream.Position = inputArchive[i].FilePosition!.Value;
                recreatedArchive.BaseStream.Position = recreatedArchive[i].FilePosition!.Value;

                var size1 = inputArchive[i].CompressedSize!.Value;
                var size2 = recreatedArchive[i].CompressedSize!.Value;
                StreamAssert.AreEqual(inputArchive.BaseStream, recreatedArchive.BaseStream, size1 > size2 ? size1 : size2);
            }

            StreamAssert.AreEqual(inputArchive.BaseStream, recreatedArchive.BaseStream);
        }

        private static IEnumerable<object[]> GetTestFiles()
        {
            foreach (var file in Directory.EnumerateFiles("TestData", "*", SearchOption.TopDirectoryOnly))
            {
                yield return new object[] { file };
            }
        }

        private static IEnumerable<object[]> GetTestFilesAndFlags()
        {
            foreach (var file in Directory.EnumerateFiles("TestData", "*", SearchOption.TopDirectoryOnly))
            {
                if (new FileInfo(file).Name == "noise.png")
                {
                    continue;
                }

                foreach (var flags in GetFlagCombinations())
                {
                    yield return new object[] { file, flags };
                }
            }
        }

        private static IEnumerable<object[]> GetTestFlags()
        {
            foreach (var flags in GetFlagCombinations())
            {
                yield return new object[] { flags };
            }
        }

        private static IEnumerable<MpqFileFlags> GetFlagCombinations()
        {
            yield return MpqFileFlags.Exists;
            yield return MpqFileFlags.Exists | MpqFileFlags.SingleUnit;
            yield return MpqFileFlags.Exists | MpqFileFlags.Encrypted;
            yield return MpqFileFlags.Exists | MpqFileFlags.Encrypted | MpqFileFlags.SingleUnit;
            yield return MpqFileFlags.Exists | MpqFileFlags.CompressedMulti;
            yield return MpqFileFlags.Exists | MpqFileFlags.CompressedMulti | MpqFileFlags.SingleUnit;
            yield return MpqFileFlags.Exists | MpqFileFlags.CompressedMulti | MpqFileFlags.Encrypted;
            yield return MpqFileFlags.Exists | MpqFileFlags.CompressedMulti | MpqFileFlags.Encrypted | MpqFileFlags.SingleUnit;
            yield return MpqFileFlags.Exists | MpqFileFlags.BlockOffsetAdjustedKey;
            yield return MpqFileFlags.Exists | MpqFileFlags.BlockOffsetAdjustedKey | MpqFileFlags.SingleUnit;
            yield return MpqFileFlags.Exists | MpqFileFlags.BlockOffsetAdjustedKey | MpqFileFlags.Encrypted;
            yield return MpqFileFlags.Exists | MpqFileFlags.BlockOffsetAdjustedKey | MpqFileFlags.Encrypted | MpqFileFlags.SingleUnit;
            yield return MpqFileFlags.Exists | MpqFileFlags.BlockOffsetAdjustedKey | MpqFileFlags.CompressedMulti;
            yield return MpqFileFlags.Exists | MpqFileFlags.BlockOffsetAdjustedKey | MpqFileFlags.CompressedMulti | MpqFileFlags.SingleUnit;
            yield return MpqFileFlags.Exists | MpqFileFlags.BlockOffsetAdjustedKey | MpqFileFlags.CompressedMulti | MpqFileFlags.Encrypted;
            yield return MpqFileFlags.Exists | MpqFileFlags.BlockOffsetAdjustedKey | MpqFileFlags.CompressedMulti | MpqFileFlags.Encrypted | MpqFileFlags.SingleUnit;
        }
    }
}