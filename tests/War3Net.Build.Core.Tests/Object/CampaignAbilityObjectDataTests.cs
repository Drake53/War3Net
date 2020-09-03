// ------------------------------------------------------------------------------
// <copyright file="CampaignAbilityObjectDataTests.cs" company="Drake53">
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
    public class CampaignAbilityObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignAbilityObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignAbilityObjectData(string campaignAbilityObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignAbilityObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignAbilityObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetCampaignAbilityObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignAbilityObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Ability"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignAbilityObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Campaigns"));
        }
    }
}