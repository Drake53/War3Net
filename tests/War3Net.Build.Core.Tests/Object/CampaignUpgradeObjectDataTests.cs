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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class CampaignUpgradeObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignUpgradeObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignUpgradeObjectData(string campaignUpgradeObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(campaignUpgradeObjectDataFilePath, typeof(CampaignUpgradeObjectData), nameof(BinaryReaderExtensions.ReadUpgradeObjectData), true);
        }

        private static IEnumerable<object[]> GetCampaignUpgradeObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignUpgradeObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Upgrade"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignUpgradeObjectData.FileName,
                SearchOption.AllDirectories,
                "Campaigns"));
        }
    }
}