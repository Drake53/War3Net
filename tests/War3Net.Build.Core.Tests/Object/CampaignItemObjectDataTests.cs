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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class CampaignItemObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignItemObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignItemObjectData(string campaignItemObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(campaignItemObjectDataFilePath, typeof(CampaignItemObjectData), nameof(BinaryReaderExtensions.ReadItemObjectData), true);
        }

        private static IEnumerable<object[]> GetCampaignItemObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignItemObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Item"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignItemObjectData.FileName,
                SearchOption.AllDirectories,
                "Campaigns"));
        }
    }
}