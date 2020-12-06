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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class CampaignDoodadObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignDoodadObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignDoodadObjectData(string campaignDoodadObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(campaignDoodadObjectDataFilePath, typeof(CampaignDoodadObjectData), nameof(BinaryReaderExtensions.ReadDoodadObjectData), true);
        }

        private static IEnumerable<object[]> GetCampaignDoodadObjectData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignDoodadObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Doodad"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignDoodadObjectData.FileName,
                SearchOption.AllDirectories,
                "Campaigns"));
        }
    }
}