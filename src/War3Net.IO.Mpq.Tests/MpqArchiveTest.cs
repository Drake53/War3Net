// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveTest.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.IO.Mpq.Tests
{
    [TestClass]
    public class MpqArchiveTest
    {
        private const ushort BlockSize = 8;

        [DataTestMethod]
        [DynamicData(nameof(GetTestFiles), DynamicDataSourceType.Method)]
        public void TestStoreThenRetrieveFile(string filename)
        {
            var fileStream = File.OpenRead(filename);
            var mpqFile = new MpqFile(fileStream, filename, MpqFileFlags.Exists, BlockSize);
            var archive = MpqArchive.Create(new MemoryStream(), new List<MpqFile>() { mpqFile });

            var openedArchive = MpqArchive.Open(archive.BaseStream);
            var openedStream = openedArchive.OpenFile(filename);

            // Reload file, since the filestream gets disposed when the mpqfile is added to an mpqarchive.
            fileStream = File.OpenRead(filename);

            Assert.AreEqual(fileStream.Length, openedStream.Length);

            using (var fileStreamReader = new StreamReader(fileStream))
            {
                using (var openedStreamReader = new StreamReader(openedStream))
                {
                    StringAssert.Equals(fileStreamReader.ReadToEnd(), openedStreamReader.ReadToEnd());
                }
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestFiles), DynamicDataSourceType.Method)]
        public void TestStoreCompressedThenRetrieveFile(string filename)
        {
            var fileStream = File.OpenRead(filename);
            var mpqFile = new MpqFile(fileStream, filename, MpqFileFlags.Exists | MpqFileFlags.Compressed, BlockSize);
            var archive = MpqArchive.Create(new MemoryStream(), new List<MpqFile>() { mpqFile });

            var openedArchive = MpqArchive.Open(archive.BaseStream);
            var openedStream = openedArchive.OpenFile(filename);

            // Reload file, since the filestream gets disposed when the mpqfile is added to an mpqarchive.
            fileStream = File.OpenRead(filename);

            Assert.AreEqual(fileStream.Length, openedStream.Length);

            using (var fileStreamReader = new StreamReader(fileStream))
            {
                using (var openedStreamReader = new StreamReader(openedStream))
                {
                    StringAssert.Equals(fileStreamReader.ReadToEnd(), openedStreamReader.ReadToEnd());
                }
            }
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