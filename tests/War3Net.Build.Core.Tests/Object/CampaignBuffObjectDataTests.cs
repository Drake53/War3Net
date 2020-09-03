// ------------------------------------------------------------------------------
// <copyright file="CampaignBuffObjectDataTests.cs" company="Drake53">
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
    public class CampaignBuffObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignBuffObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignBuffObjectData(string campaignBuffObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignBuffObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignBuffObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetCampaignBuffObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignBuffObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Buff"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignBuffObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Campaigns"));
        }
    }
}