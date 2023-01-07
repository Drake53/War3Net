// ------------------------------------------------------------------------------
// <copyright file="TriggerStringsTests.cs" company="Drake53">
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
    public class TriggerStringsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTriggerStringsData), DynamicDataSourceType.Method)]
        public void TestParseTriggerStrings(string triggerStringsFilePath)
        {
            ParseTestHelper.RunStreamRWTest(
                triggerStringsFilePath,
                typeof(TriggerStrings),
                nameof(StreamWriterExtensions.WriteTriggerStrings));
        }

        private static IEnumerable<object[]> GetTriggerStringsData()
        {
            return TestDataProvider.GetDynamicData(
                $"*{TriggerStrings.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Triggers"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                TriggerStrings.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                TriggerStrings.MapFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}