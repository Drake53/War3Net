// ------------------------------------------------------------------------------
// <copyright file="CampaignItemObjectDataTests.cs" company="Drake53">
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
    public class CampaignItemObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignItemObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignItemObjectData(string campaignItemObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignItemObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignItemObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetCampaignItemObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignItemObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Item"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignItemObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Campaigns"));
        }
    }
}