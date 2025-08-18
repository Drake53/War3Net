// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Mpq.Extensions;
using War3Net.TestTools.UnitTesting;

namespace War3Net.IO.Mpq.Tests
{
    [TestClass]
    public class MpqArchiveTests
    {
        private const ushort BlockSize = 3;

        [TestMethod]
        public void TestWithPreArchiveDataAndNoFiles()
        {
            var memoryStream = new MemoryStream();
            var randomData = new byte[999];
            memoryStream.Write(randomData, 0, randomData.Length);

            using var a = MpqArchive.Create(memoryStream, Array.Empty<MpqFile>(), new MpqArchiveCreateOptions());

            memoryStream.Position = 0;
            MpqArchive.Open(memoryStream).Dispose();
        }

        [TestMethod]
        public void TestWithPreArchiveData()
        {
            var memoryStream = new MemoryStream();
            var randomData = new byte[999];
            randomData[100] = 99;
            memoryStream.Write(randomData, 0, randomData.Length);

            var randomFiles = new List<MpqFile>();
            for (var i = 0; i < 35; i++)
            {
                var fileStream = new MemoryStream();
                fileStream.Write(randomData, 0, randomData.Length);
                fileStream.Position = 0;
                randomFiles.Add(MpqFile.New(fileStream, $"file{i}"));
            }

            using var a = MpqArchive.Create(memoryStream, randomFiles, new MpqArchiveCreateOptions() { ListFileCreateMode = MpqFileCreateMode.None, AttributesCreateMode = MpqFileCreateMode.None });

            memoryStream.Position = 0;
            var archive = MpqArchive.Open(memoryStream);
            foreach (var file in archive.GetMpqFiles())
            {
                file.MpqStream.Seek(100, SeekOrigin.Begin);
                Assert.AreEqual(99, file.MpqStream.ReadByte());
            }

            archive.Dispose();
        }

        [TestMethod]
        [DynamicData(nameof(GetTestFilesAndFlags), DynamicDataSourceType.Method)]
        public void TestStoreThenRetrieveFileWithFlags(string fileName, MpqFileFlags flags)
        {
            using var fileStream = File.OpenRead(fileName);
            var mpqFile = MpqFile.New(fileStream, fileName, true);
            mpqFile.TargetFlags = flags;
            using var archive = MpqArchive.Create(new MemoryStream(), new List<MpqFile>() { mpqFile }, new MpqArchiveCreateOptions { BlockSize = BlockSize });

            using var openedArchive = MpqArchive.Open(archive.BaseStream);
            var openedStream = openedArchive.OpenFile(fileName);

            StreamAssert.AreEqual(fileStream, openedStream, true);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestFlags), DynamicDataSourceType.Method)]
        public void TestStoreThenRetrieveEmptyFileWithFlags(MpqFileFlags flags)
        {
            const string FileName = "someRandomFile.empty";

            using var mpqFile = MpqFile.New(null, FileName);
            mpqFile.TargetFlags = flags;
            using var archive = MpqArchive.Create(new MemoryStream(), new List<MpqFile>() { mpqFile }, new MpqArchiveCreateOptions { BlockSize = BlockSize });

            using var openedArchive = MpqArchive.Open(archive.BaseStream);
            var openedStream = openedArchive.OpenFile(FileName);

            Assert.IsTrue(openedStream.Length == 0);
        }

        [FlakyTestMethod]
        [DynamicData(nameof(GetTestArchivesAndSettings), DynamicDataSourceType.Method)]
        public void TestRecreateArchive(string inputArchivePath, bool loadListFile)
        {
            using var inputArchive = MpqArchive.Open(inputArchivePath, loadListFile);
            if (loadListFile && !inputArchive.FileExists(ListFile.FileName))
            {
                return;
            }

            var mpqFiles = inputArchive.GetMpqFiles().ToArray();
            using var recreatedArchive = MpqArchive.Create((Stream?)null, mpqFiles, new MpqArchiveCreateOptions { BlockSize = inputArchive.Header.BlockSize, ListFileCreateMode = MpqFileCreateMode.None, AttributesCreateMode = MpqFileCreateMode.None });

            // TODO: fix assumption that recreated archive's hashtable cannot be smaller than original
            // TODO: fix assumption of how recreated blocktable's entries are laid out relative to input mpqFiles array? (aka: replace the 'offset' variable)
            var offsetPerUnknownFile = (recreatedArchive.HashTable.Size / inputArchive.HashTable.Size) - 1;
            var mpqEncryptedFileCount = mpqFiles.Where(mpqFile => mpqFile.IsFilePositionFixed).Count();
            var offset = mpqEncryptedFileCount * (offsetPerUnknownFile + 1);
            mpqEncryptedFileCount = 0;
            for (var index = 0; index < mpqFiles.Length; index++)
            {
                var mpqFile = mpqFiles[index];
                MpqEntry? inputEntry;
                if (mpqFile is MpqKnownFile knownFile)
                {
                    inputEntry = inputArchive.GetMpqEntries(knownFile.FileName).FirstOrDefault();
                }
                else if (mpqFile is MpqUnknownFile unknownFile)
                {
                    inputArchive.TryGetEntryFromHashTable(mpqFile.HashIndex & inputArchive.HashTable.Mask, out inputEntry);
                }
                else if (mpqFile is MpqOrphanedFile orphanedFile)
                {
                    // TODO
                    throw new NotSupportedException("found orphaned mpqfile");
                }
                else
                {
                    throw new NotImplementedException();
                }

                var blockIndex = index + (int)offset;
                if (mpqFile.IsFilePositionFixed)
                {
                    blockIndex = mpqEncryptedFileCount * ((int)offsetPerUnknownFile + 1);
                    mpqEncryptedFileCount++;
                }

                var recreatedEntry = recreatedArchive[blockIndex];

                if (inputEntry is not null)
                {
                    if (!mpqFile.MpqStream.CanBeDecrypted)
                    {
                        // Check if both files have the same encryption seed.
                        Assert.IsTrue(!mpqFile.TargetFlags.HasFlag(MpqFileFlags.BlockOffsetAdjustedKey) || inputEntry.FileOffset == recreatedEntry.FileOffset);

                        inputArchive.BaseStream.Position = inputEntry.FilePosition;
                        recreatedArchive.BaseStream.Position = recreatedEntry.FilePosition;

                        var size1 = inputEntry.CompressedSize;
                        var size2 = recreatedEntry.CompressedSize;
                        StreamAssert.AreEqual(inputArchive.BaseStream, recreatedArchive.BaseStream, size1 > size2 ? size1 : size2);
                    }
                    else
                    {
                        using var inputStream = inputArchive.OpenFile(inputEntry);
                        using var recreatedStream = recreatedArchive.OpenFile(recreatedEntry);

                        StreamAssert.AreEqual(inputStream, recreatedStream, mpqFile is MpqKnownFile known ? known.FileName : "<unknown file>");
                    }
                }
                else
                {
                    Assert.IsFalse(recreatedEntry.Flags.HasFlag(MpqFileFlags.Exists));
                }

                if (mpqFile is MpqUnknownFile && !mpqFile.IsFilePositionFixed)
                {
                    offset += offsetPerUnknownFile;
                }
            }
        }

        [TestMethod]
        public void TestDeleteFile()
        {
            var inputArchivePath = TestDataProvider.GetPath("Maps/NewLuaMap.w3m");
            const string fileName = "war3map.lua";

            using var inputArchive = MpqArchive.Open(inputArchivePath);
            var newFile = MpqFile.New(null, fileName);

            var mpqFiles = inputArchive.GetMpqFiles();
            var oldFile = mpqFiles.FirstOrDefault(file => file.Equals(newFile)) ?? throw new FileNotFoundException($"File not found: {fileName}");
            var newFiles = mpqFiles.Select(file => ReferenceEquals(file, oldFile) ? newFile : file).ToArray();

            using var outputArchive = MpqArchive.Create((Stream?)null, newFiles, new MpqArchiveCreateOptions { BlockSize = inputArchive.Header.BlockSize, HashTableSize = (ushort)inputArchive.Header.HashTableSize, AttributesFlags = AttributesFlags.DateTime | AttributesFlags.Crc32 });

            var entries = outputArchive.GetMpqEntries(fileName);
            Assert.IsTrue(entries.Any());
            Assert.AreEqual(0U, entries.Single().FileSize);
        }

        [FlakyTestMethod]
        public void TestRecreatePKCompressed()
        {
            var inputArchivePath = TestDataProvider.GetPath("Maps/PKCompressed.w3x");

            using var inputArchive = MpqArchive.Open(inputArchivePath);
            using var recreatedArchive = MpqArchive.Create((Stream?)null, inputArchive.GetMpqFiles().ToArray(), new MpqArchiveCreateOptions { BlockSize = inputArchive.Header.BlockSize, HashTableSize = (ushort)inputArchive.Header.HashTableSize, ListFileCreateMode = MpqFileCreateMode.None, AttributesCreateMode = MpqFileCreateMode.None });

            for (var i = 0; i < inputArchive.Header.BlockTableSize; i++)
            {
                inputArchive.BaseStream.Position = inputArchive[i].FilePosition;
                recreatedArchive.BaseStream.Position = recreatedArchive[i].FilePosition;

                // var size1 = inputArchive[i].CompressedSize;
                // var size2 = recreatedArchive[i].CompressedSize;
                // StreamAssert.AreEqual(inputArchive.BaseStream, recreatedArchive.BaseStream, size1 > size2 ? size1 : size2);
                StreamAssert.AreEqual(inputArchive.BaseStream, recreatedArchive.BaseStream);
            }

            inputArchive.BaseStream.Position = 0;
            recreatedArchive.BaseStream.Position = 0;
            StreamAssert.AreEqual(inputArchive.BaseStream, recreatedArchive.BaseStream, MpqHeader.Size);
        }

        [TestMethod]
        [DynamicData(nameof(GetTestArchivesAttributes), DynamicDataSourceType.Method)]
        public void TestAttributes(string archivePath)
        {
            using var archive = MpqArchive.Open(archivePath);
            Assert.IsTrue(archive.VerifyAttributes());
        }

        private static IEnumerable<object[]> GetTestMaps()
        {
            return TestDataProvider.GetDynamicData("*", SearchOption.TopDirectoryOnly, "Maps");
        }

        private static IEnumerable<object[]> GetTestArchives()
        {
            return TestDataProvider.GetDynamicData("*", SearchOption.TopDirectoryOnly, "Mpq");
        }

        private static IEnumerable<object[]> GetTestArchivesAttributes()
        {
            foreach (var archive in GetTestMaps())
            {
                using (var mpqArchive = MpqArchive.Open((string)archive[0]))
                {
                    if (!mpqArchive.FileExists(Attributes.FileName))
                    {
                        continue;
                    }
                }

                yield return new object[] { archive[0] };
            }

            foreach (var archive in GetTestArchives())
            {
                using (var mpqArchive = MpqArchive.Open((string)archive[0]))
                {
                    if (!mpqArchive.FileExists(Attributes.FileName))
                    {
                        continue;
                    }
                }

                yield return new object[] { archive[0] };
            }
        }

        private static IEnumerable<object[]> GetTestArchivesAndSettings()
        {
            foreach (var archive in GetTestMaps())
            {
                yield return new object[] { archive[0], true };
                yield return new object[] { archive[0], false };
            }
        }

        private static IEnumerable<object[]> GetTestFilesAndFlags()
        {
            foreach (var file in TestDataProvider.GetDynamicData("*", SearchOption.TopDirectoryOnly, "Text"))
            {
                foreach (var flags in GetFlagCombinations())
                {
                    yield return new object[] { file[0], flags };
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