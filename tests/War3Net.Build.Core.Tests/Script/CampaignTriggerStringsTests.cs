// ------------------------------------------------------------------------------
// <copyright file="CampaignTriggerStringsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Build.Script;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Script
{
    [TestClass]
    public class CampaignTriggerStringsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetCampaignTriggerStringsData), DynamicDataSourceType.Method)]
        public void TestParseCampaignTriggerStrings(string campaignTriggerStringsFilePath)
        {
            ParseTestHelper.RunStreamRWTest(
                campaignTriggerStringsFilePath,
                typeof(CampaignTriggerStrings),
                nameof(StreamWriterExtensions.WriteTriggerStrings));
        }

        private static IEnumerable<object[]> GetCampaignTriggerStringsData()
        {
            return TestDataProvider.GetDynamicData(
                CampaignTriggerStrings.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Triggers"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignTriggerStrings.FileName,
                SearchOption.AllDirectories,
                "Campaigns"));
        }
    }
}