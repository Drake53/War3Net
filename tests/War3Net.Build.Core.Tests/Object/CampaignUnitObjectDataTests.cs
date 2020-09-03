// ------------------------------------------------------------------------------
// <copyright file="CampaignUnitObjectDataTests.cs" company="Drake53">
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
    public class CampaignUnitObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignUnitObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignUnitObjectData(string campaignUnitObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignUnitObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignUnitObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetCampaignUnitObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignUnitObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Unit"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignUnitObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Campaigns"));
        }
    }
}