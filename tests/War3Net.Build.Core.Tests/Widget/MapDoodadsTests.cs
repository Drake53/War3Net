// ------------------------------------------------------------------------------
// <copyright file="MapDoodadsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Widget;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Widget
{
    [TestClass]
    public class MapDoodadsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapDoodadsData), DynamicDataSourceType.Method)]
        public void TestParseMapDoodads(string mapDoodadsFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(mapDoodadsFilePath, typeof(MapDoodads));
        }

        private static IEnumerable<object[]> GetMapDoodadsData()
        {
            return TestDataProvider.GetDynamicData(
                MapDoodads.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Widget", "Doodads"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapDoodads.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}