using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.IO.Mpq.Tests
{
    [TestClass]
    public class BlpImageTest
    {
        [DataTestMethod]
        [DataRow("TestData/Lorem Ipsum.txt", (ushort)8)]
        public void TestStoreThenRetrieveFile(string filename, ushort blockSize)
        {
            var fileStream = File.OpenRead(filename);
            var mpqFile = new MpqFile(fileStream, filename, MpqFileFlags.Exists, blockSize);
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
        [DataRow("TestData/Lorem Ipsum.txt", (ushort)8)]
        public void TestStoreCompressedThenRetrieveFile(string filename, ushort blockSize)
        {
            var fileStream = File.OpenRead(filename);
            var mpqFile = new MpqFile(fileStream, filename, MpqFileFlags.Exists | MpqFileFlags.Compressed, blockSize);
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
    }
}