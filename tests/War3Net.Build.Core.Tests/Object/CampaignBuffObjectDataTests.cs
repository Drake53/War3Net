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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class CampaignBuffObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignBuffObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignBuffObjectData(string campaignBuffObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(campaignBuffObjectDataFilePath, typeof(CampaignBuffObjectData), nameof(BinaryReaderExtensions.ReadBuffObjectData), true);
        }

        private static IEnumerable<object[]> GetCampaignBuffObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignBuffObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Buff"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignBuffObjectData.FileName,
                SearchOption.AllDirectories,
                "Campaigns"));
        }
    }
}