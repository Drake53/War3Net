// ------------------------------------------------------------------------------
// <copyright file="MpqFileTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.TestTools.UnitTesting;

namespace War3Net.IO.Mpq.Tests
{
    [TestClass]
    public class MpqFileTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetFileProviderFileExistsTestCases), DynamicDataSourceType.Method)]
        public void TestFileProviderFileExists(string path, bool expected)
        {
            Assert.AreEqual(expected, MpqFile.Exists(path));
        }

        [DataTestMethod]
        [DynamicData(nameof(GetFileProviderFileExistsTestCases), DynamicDataSourceType.Method)]
        public void TestFileProviderGetFile(string path, bool expected)
        {
            if (expected)
            {
                using var file = MpqFile.OpenRead(path);
                Assert.IsNotNull(file);
            }
            else
            {
                Assert.ThrowsException<FileNotFoundException>(() =>
                {
                    using var file = MpqFile.OpenRead(path);
                });
            }
        }

        private static IEnumerable<object[]> GetFileProviderFileExistsTestCases()
        {
            yield return new object[] { TestDataProvider.GetPath(Path.Combine("Campaigns", "ToDOv0.027.w3n")), true };
            yield return new object[] { TestDataProvider.GetPath(Path.Combine("Campaigns", "ToDOv0.027.w3n", "war3campaign.w3f")), true };
            yield return new object[] { TestDataProvider.GetPath(Path.Combine("Campaigns", "ToDOv0.027.w3n", "OrcD06.w3x", "war3map.w3i")), true };

            yield return new object[] { TestDataProvider.GetPath(Path.Combine("Campaigns", "doesnotexist")), false };
            yield return new object[] { TestDataProvider.GetPath(Path.Combine("Campaigns", "ToDOv0.027.w3n", "doesnotexist")), false };
            yield return new object[] { TestDataProvider.GetPath(Path.Combine("Campaigns", "ToDOv0.027.w3n", "OrcD06.w3x", "doesnotexist")), false };
        }
    }
}