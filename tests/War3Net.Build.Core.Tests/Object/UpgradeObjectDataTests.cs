// ------------------------------------------------------------------------------
// <copyright file="UpgradeObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class UpgradeObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetUpgradeObjectData), DynamicDataSourceType.Method)]
        public void TestParseUpgradeObjectData(string upgradeObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                upgradeObjectDataFilePath,
                typeof(UpgradeObjectData),
                nameof(BinaryReaderExtensions.ReadUpgradeObjectData));
        }

        private static IEnumerable<object[]> GetUpgradeObjectData()
        {
            return TestDataProvider.GetDynamicData(
                $"*{UpgradeObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Upgrade"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UpgradeObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UpgradeObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UpgradeObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UpgradeObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}