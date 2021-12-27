// ------------------------------------------------------------------------------
// <copyright file="CampaignInfoTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Info;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Info
{
    [TestClass]
    public class CampaignInfoTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignInfoData), DynamicDataSourceType.Method)]
        public void TestParseCampaignInfo(string campaignInfoFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(campaignInfoFilePath, typeof(CampaignInfo));
        }

        private static IEnumerable<object[]> GetCampaignInfoData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignInfo.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Info", "Campaign"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignInfo.FileName,
                SearchOption.AllDirectories,
                "Campaigns"));
        }
    }
}