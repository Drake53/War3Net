// ------------------------------------------------------------------------------
// <copyright file="CompressedArchiveTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.IO.Mpq.Tests
{
    [TestClass]
    public class CompressedArchiveTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestFiles), DynamicDataSourceType.Method)]
        public void CompareCompressedAndUncompressedArchive(string compressedArchive, string uncompressedArchive)
        {
            var compArchive = MpqArchive.Open(compressedArchive, true);
            var uncompArchive = MpqArchive.Open(uncompressedArchive);

            foreach (var mpqFile in compArchive)
            {
                if (mpqFile.IsCompressed && mpqFile.Filename != null)
                {
                    using var compressedFileStream = compArchive.OpenFile(mpqFile.Filename);
                    using var uncompressedFileStream = uncompArchive.OpenFile(mpqFile.Filename);

                    StreamAssert.AreEqual(compressedFileStream, uncompressedFileStream);
                }
            }
        }

        private static IEnumerable<object[]> GetTestFiles()
        {
            yield return new object[] { @"TestArchives\Compressed Undecorated.w3x", @"TestArchives\Uncompressed Undecorated.w3x" };
            yield return new object[] { @"TestArchives\Compressed Decorated.w3x", @"TestArchives\Uncompressed Decorated.w3x" };
        }
    }
}