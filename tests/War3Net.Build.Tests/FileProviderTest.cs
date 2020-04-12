// ------------------------------------------------------------------------------
// <copyright file="FileProviderTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Info;
using War3Net.Build.Providers;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class FileProviderTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetFileProviderFileExistsTestCases), DynamicDataSourceType.Method)]
        public void TestFileProviderFileExists(string path, bool expected)
        {
            Assert.AreEqual(expected, FileProvider.FileExists(path));
        }

        [DataTestMethod]
        [DynamicData(nameof(GetFileProviderFileExistsTestCases), DynamicDataSourceType.Method)]
        public void TestFileProviderGetFile(string path, bool expected)
        {
            if (expected)
            {
                using var file = FileProvider.GetFile(path);
                Assert.IsNotNull(file);
            }
            else
            {
                try
                {
                    using (var file = FileProvider.GetFile(path))
                    {
                        Assert.Fail();
                    }
                }
                catch (FileNotFoundException)
                {
                }
            }
        }

        private static IEnumerable<object[]> GetFileProviderFileExistsTestCases()
        {
            yield return new object[] { Path.Combine(TestDataProvider.TestDataFolder, "Campaigns", "ToDOv0.027.w3n"), true };
            yield return new object[] { Path.Combine(TestDataProvider.TestDataFolder, "Campaigns", "ToDOv0.027.w3n", CampaignInfo.FileName), true };
            yield return new object[] { Path.Combine(TestDataProvider.TestDataFolder, "Campaigns", "ToDOv0.027.w3n", "OrcD06.w3x", MapInfo.FileName), true };

            yield return new object[] { Path.Combine(TestDataProvider.TestDataFolder, "Campaigns", "doesnotexist"), false };
            yield return new object[] { Path.Combine(TestDataProvider.TestDataFolder, "Campaigns", "ToDOv0.027.w3n", "doesnotexist"), false };
            yield return new object[] { Path.Combine(TestDataProvider.TestDataFolder, "Campaigns", "ToDOv0.027.w3n", "OrcD06.w3x", "doesnotexist"), false };
        }
    }
}