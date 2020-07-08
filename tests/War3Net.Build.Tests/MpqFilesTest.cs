// ------------------------------------------------------------------------------
// <copyright file="MpqFilesTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Mpq;

// TODO: move this to War3Net.IO.Mpq.Tests
namespace War3Net.Build.Tests
{
    [TestClass]
    public class MpqFilesTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestGetMpqFiles(string mpqFilePath)
        {
            Assert.IsTrue(TestDataProvider.IsArchiveFile(mpqFilePath, out _));

            using var archive = MpqArchive.Open(mpqFilePath, true);
            var mpqFiles = archive.GetMpqFiles();

            Assert.AreEqual(archive.Header.BlockTableSize, (uint)mpqFiles.Count());
        }

        private static IEnumerable<object[]> GetTestData()
        {
            return TestDataProvider.GetDynamicData("*", SearchOption.TopDirectoryOnly, "Maps");
        }
    }
}