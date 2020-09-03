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
using War3Net.Common.Testing;
using War3Net.IO.Mpq;

namespace War3Net.Build.Core.Tests.Info
{
    [TestClass]
    public class CampaignInfoTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignInfoData), DynamicDataSourceType.Method)]
        public void TestParseCampaignInfo(string campaignInfoFilePath)
        {
            using var original = FileProvider.GetFile(campaignInfoFilePath);
            using var recreated = new MemoryStream();

            CampaignInfo.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        private static IEnumerable<object[]> GetCampaignInfoData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignInfo.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Info", "Campaign"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignInfo.FileName,
                SearchOption.TopDirectoryOnly,
                "Campaigns"));
        }
    }
}