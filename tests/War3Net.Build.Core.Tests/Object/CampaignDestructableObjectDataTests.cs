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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class CampaignDestructableObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignDestructableObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignDestructableObjectData(string campaignDestructableObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(campaignDestructableObjectDataFilePath, typeof(CampaignDestructableObjectData), nameof(BinaryReaderExtensions.ReadDestructableObjectData), true);
        }

        private static IEnumerable<object[]> GetCampaignDestructableObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignDestructableObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Destructable"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignDestructableObjectData.FileName,
                SearchOption.AllDirectories,
                "Campaigns"));
        }
    }
}