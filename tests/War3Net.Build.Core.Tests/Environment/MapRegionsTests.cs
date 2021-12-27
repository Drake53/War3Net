// ------------------------------------------------------------------------------
// <copyright file="MapRegionsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapRegionsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetRegionFiles), DynamicDataSourceType.Method)]
        public void TestParseMapRegions(string regionsFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(regionsFilePath, typeof(MapRegions));
        }

        private static IEnumerable<object[]> GetRegionFiles()
        {
            return TestDataProvider.GetDynamicData(
                MapRegions.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Region"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapRegions.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}