// ------------------------------------------------------------------------------
// <copyright file="CampaignDestructableObjectDataTests.cs" company="Drake53">
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
    public class CampaignDestructableObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignDestructableObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignDestructableObjectData(string campaignDestructableObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignDestructableObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignDestructableObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetCampaignDestructableObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignDestructableObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Destructable"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignDestructableObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Campaigns"));
        }
    }
}