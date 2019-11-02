// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
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
            var mpqFile = new MpqKnownFile(filename, fileStream, MpqFileFlags.Exists, MpqLocale.Neutral, true);
            var archive = MpqArchive.Create(new MemoryStream(), new List<MpqFile>() { mpqFile });

            var openedArchive = MpqArchive.Open(archive.BaseStream);
            var openedStream = openedArchive.OpenFile(filename);

            fileStream.Position = 0;
            StreamAssert.AreEqual(fileStream, openedStream);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestFilesAndFlags), DynamicDataSourceType.Method)]
        public void TestStoreThenRetrieveFileWithFlags(string filename, MpqFileFlags flags)
        {
            var fileStream = File.OpenRead(filename);
            // var mpqFile = new MpqFile(fileStream, filename, MpqLocale.Neutral, flags, BlockSize);
            var mpqFile = new MpqKnownFile(filename, fileStream, flags, MpqLocale.Neutral, true);
            var archive = MpqArchive.Create(new MemoryStream(), new List<MpqFile>() { mpqFile }, blockSize: BlockSize);

            var openedArchive = MpqArchive.Open(archive.BaseStream);
            var openedStream = openedArchive.OpenFile(filename);

            fileStream.Position = 0;
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

        [DataTestMethod]
        [DynamicData(nameof(GetTestArchivesAndSettings), DynamicDataSourceType.Method)]
        public void TestRecreateArchive(string inputArchivePath, bool loadListFile)
        {
            using var inputArchive = MpqArchive.Open(inputArchivePath, loadListFile);
            if (loadListFile && !inputArchive.FileExists(ListFile.Key))
            {
                return;
            }

            var mpqFiles = inputArchive.GetMpqFiles().ToArray();
            using var recreatedArchive = MpqArchive.Create((Stream?)null, mpqFiles);

            static bool CheckEncrypted(MpqFile mpqFile)
            {
                return mpqFile is MpqEncryptedFile && mpqFile.Flags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey);
            }

            // TODO: fix assumption that recreated archive's hashtable cannot be smaller than original
            // TODO: fix assumption of how recreated blocktable's entries are laid out relative to input mpqFiles array? (aka: replace the 'offset' variable)
            var offsetPerUnknownFile = (recreatedArchive.HashTableSize / inputArchive.HashTableSize) - 1;
            var mpqEncryptedFileCount = mpqFiles.Where(CheckEncrypted).Count();
            var offset = mpqEncryptedFileCount * (offsetPerUnknownFile + 1);
            mpqEncryptedFileCount = 0;
            for (var index = 0; index < mpqFiles.Length; index++)
            {
                var mpqFile = mpqFiles[index];
                bool exists;
                MpqEntry? inputEntry;
                if (mpqFile is MpqKnownFile knownFile)
                {
                    exists = inputArchive.FileExists(knownFile.FileName, out var entryIndex);
                    inputEntry = exists ? inputArchive[entryIndex] : null;
                }
                else if (mpqFile is MpqUnknownFile unknownFile)
                {
                    exists = inputArchive.TryGetEntryFromHashTable(mpqFile.HashIndex & inputArchive.HashTableMask, out inputEntry);
                }
                else
                {
                    throw new NotImplementedException();
                }

                var blockIndex = index + (int)offset;
                if (CheckEncrypted(mpqFile))
                {
                    blockIndex = mpqEncryptedFileCount * ((int)offsetPerUnknownFile + 1);
                    mpqEncryptedFileCount++;
                }

                var recreatedEntry = recreatedArchive[blockIndex];

                if (exists)
                {
                    if (mpqFile is MpqEncryptedFile encryptedFile)
                    {
                        // Check if both files have the same encryption seed.
                        Assert.IsTrue(!mpqFile.Flags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey) || inputEntry.FileOffset == recreatedEntry.FileOffset);

                        inputArchive.BaseStream.Position = inputEntry.FilePosition!.Value;
                        recreatedArchive.BaseStream.Position = recreatedEntry.FilePosition!.Value;

                        var size1 = inputEntry.CompressedSize!.Value;
                        var size2 = recreatedEntry.CompressedSize!.Value;
                        StreamAssert.AreEqual(inputArchive.BaseStream, recreatedArchive.BaseStream, size1 > size2 ? size1 : size2);
                    }
                    else
                    {
                        using var inputStream = inputArchive.OpenFile(inputEntry);
                        using var recreatedStream = recreatedArchive.OpenFile(recreatedEntry);

                        StreamAssert.AreEqual(inputStream, recreatedStream);
                    }
                }
                else
                {
                    Assert.IsFalse(recreatedEntry.Flags.HasFlag(MpqFileFlags.Exists));
                }

                if (mpqFile is MpqUnknownFile && !CheckEncrypted(mpqFile))
                {
                    offset += offsetPerUnknownFile;
                }
            }
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
                inputArchive.BaseStream.Position = inputArchive[i].FilePosition!.Value + 8;
                recreatedArchive.BaseStream.Position = recreatedArchive[i].FilePosition!.Value;

                var size1 = inputArchive[i].CompressedSize!.Value;
                var size2 = recreatedArchive[i].CompressedSize!.Value;
                StreamAssert.AreEqual(inputArchive.BaseStream, recreatedArchive.BaseStream, size1 > size2 ? size1 : size2);
            }

            StreamAssert.AreEqual(inputArchive.BaseStream, recreatedArchive.BaseStream);
        }

        private static IEnumerable<object[]> GetTestArchives()
        {
            foreach (var archive in Directory.EnumerateFiles("TestArchives", "*", SearchOption.TopDirectoryOnly))
            {
                yield return new object[] { archive };
            }
        }

        private static IEnumerable<object[]> GetTestArchivesAndSettings()
        {
            foreach (var archive in GetTestArchives())
            {
                yield return new object[] { archive[0], true };
                yield return new object[] { archive[0], false };
            }
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