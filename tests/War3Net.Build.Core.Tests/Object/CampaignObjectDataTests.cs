// ------------------------------------------------------------------------------
// <copyright file="CampaignObjectDataTests.cs" company="Drake53">
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
    public class CampaignObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignObjectData(string campaignObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetCampaignObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "All"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Campaigns"));
        }
    }
}