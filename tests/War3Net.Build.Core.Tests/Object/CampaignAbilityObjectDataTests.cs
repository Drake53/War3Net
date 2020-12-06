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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class CampaignAbilityObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignAbilityObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignAbilityObjectData(string campaignAbilityObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(campaignAbilityObjectDataFilePath, typeof(CampaignAbilityObjectData), nameof(BinaryReaderExtensions.ReadAbilityObjectData), true);
        }

        private static IEnumerable<object[]> GetCampaignAbilityObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignAbilityObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Ability"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignAbilityObjectData.FileName,
                SearchOption.AllDirectories,
                "Campaigns"));
        }
    }
}