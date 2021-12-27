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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class CampaignUnitObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignUnitObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignUnitObjectData(string campaignUnitObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(campaignUnitObjectDataFilePath, typeof(CampaignUnitObjectData), nameof(BinaryReaderExtensions.ReadUnitObjectData), true);
        }

        private static IEnumerable<object[]> GetCampaignUnitObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignUnitObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Unit"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignUnitObjectData.FileName,
                SearchOption.AllDirectories,
                "Campaigns"));
        }
    }
}