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
            var mpqFile = new MpqFile(fileStream, filename, MpqLocale.Neutral, MpqFileFlags.Exists, BlockSize);
            var archive = MpqArchive.Create(new MemoryStream(), new List<MpqFile>() { mpqFile });

            var openedArchive = MpqArchive.Open(archive.BaseStream);
            var openedStream = openedArchive.OpenFile(filename);

            // Reload file, since the filestream gets disposed when the mpqfile is added to an mpqarchive.
            fileStream = File.OpenRead(filename);

            StreamAssert.AreEqual(fileStream, openedStream);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestFiles), DynamicDataSourceType.Method)]
        public void TestStoreCompressedThenRetrieveFile(string filename)
        {
            TestStoreThenRetrieveFile(filename, MpqFileFlags.Exists | MpqFileFlags.Compressed);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestFiles), DynamicDataSourceType.Method)]
        public void TestStoreCompressedSingleUnitThenRetrieveFile(string filename)
        {
            TestStoreThenRetrieveFile(filename, MpqFileFlags.Exists | MpqFileFlags.Compressed | MpqFileFlags.SingleUnit);
        }

        private void TestStoreThenRetrieveFile(string filename, MpqFileFlags flags)
        {
            var fileStream = File.OpenRead(filename);
            var mpqFile = new MpqFile(fileStream, filename, MpqLocale.Neutral, flags, BlockSize);
            var archive = MpqArchive.Create(new MemoryStream(), new List<MpqFile>() { mpqFile }, blockSize: BlockSize);

            var openedArchive = MpqArchive.Open(archive.BaseStream);
            var openedStream = openedArchive.OpenFile(filename);

            // Reload file, since the filestream gets disposed when the mpqfile is added to an mpqarchive.
            fileStream = File.OpenRead(filename);

            StreamAssert.AreEqual(fileStream, openedStream);
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

                var size1 = inputArchive[i].CompressedSize;
                var size2 = recreatedArchive[i].CompressedSize;
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
    }
}