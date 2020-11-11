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

using War3Net.Build.Script;
using War3Net.Common.Testing;
using War3Net.IO.Mpq;

namespace War3Net.Build.Core.Tests.Script
{
    [TestClass]
    public class MapTriggerStringsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapTriggerStringsData), DynamicDataSourceType.Method)]
        public void TestParseMapTriggerStrings(string mapTriggerStringsFilePath)
        {
            using var original = FileProvider.GetFile(mapTriggerStringsFilePath);
            using var recreated = new MemoryStream();

            MapTriggerStrings.Parse(original, null, true).SerializeTo(recreated, true);
            StreamAssert.AreEqualText(original, recreated, true);
        }

        private static IEnumerable<object[]> GetMapTriggerStringsData()
        {
            return TestDataProvider.GetDynamicData(
                MapTriggerStrings.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Triggers"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapTriggerStrings.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}