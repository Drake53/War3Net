// ------------------------------------------------------------------------------
// <copyright file="CampaignUpgradeObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Common.Testing;
using War3Net.IO.Mpq;

namespace War3Net.Build.Object
{
    [TestClass]
    public class CampaignUpgradeObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignUpgradeObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignUpgradeObjectData(string campaignUpgradeObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignUpgradeObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignUpgradeObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetCampaignUpgradeObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignUpgradeObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Upgrade"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignUpgradeObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Campaigns"));
        }
    }
}