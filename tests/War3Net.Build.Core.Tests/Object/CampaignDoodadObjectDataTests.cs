// ------------------------------------------------------------------------------
// <copyright file="CampaignDoodadObjectDataTests.cs" company="Drake53">
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
    public class CampaignDoodadObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignDoodadObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignDoodadObjectData(string campaignDoodadObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignDoodadObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignDoodadObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetCampaignDoodadObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignDoodadObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Doodad"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignDoodadObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Campaigns"));
        }
    }
}