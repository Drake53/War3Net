// ------------------------------------------------------------------------------
// <copyright file="MpqFilesTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Common.Testing;

namespace War3Net.IO.Mpq.Tests
{
    [TestClass]
    public class MpqFilesTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestGetMpqFiles(string mpqFilePath)
        {
            Assert.IsTrue(TestDataProvider.IsArchiveFile(mpqFilePath, out _));

            using var archive = MpqArchive.Open(mpqFilePath, true);
            var mpqFiles = archive.GetMpqFiles();

            Assert.AreEqual((int)archive.BlockTable.Size, mpqFiles.Count());
            foreach (var mpqFile in mpqFiles)
            {
                Assert.IsNotNull(mpqFile);
            }
        }

        private static IEnumerable<object[]> GetTestData()
        {
            return TestDataProvider.GetDynamicData("*", SearchOption.TopDirectoryOnly, "Maps");
        }
    }
}