// ------------------------------------------------------------------------------
// <copyright file="MapTriggerStringsTests.cs" company="Drake53">
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
    public class MapTriggerStringsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapTriggerStringsData), DynamicDataSourceType.Method)]
        public void TestParseMapTriggerStrings(string mapTriggerStringsFilePath)
        {
            ParseTestHelper.RunStreamRWTest(
                mapTriggerStringsFilePath,
                typeof(MapTriggerStrings),
                nameof(StreamWriterExtensions.WriteTriggerStrings));
        }

        private static IEnumerable<object[]> GetMapTriggerStringsData()
        {
            return TestDataProvider.GetDynamicData(
                MapTriggerStrings.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Triggers"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapTriggerStrings.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}