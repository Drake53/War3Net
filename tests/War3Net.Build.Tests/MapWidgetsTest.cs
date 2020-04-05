// ------------------------------------------------------------------------------
// <copyright file="MapWidgetsTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Providers;
using War3Net.Build.Widget;
using War3Net.Common.Testing;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class MapWidgetsTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapDoodadsData), DynamicDataSourceType.Method)]
        public void TestParseMapDoodads(string mapDoodadsFilePath)
        {
            using var original = FileProvider.GetFile(mapDoodadsFilePath);
            using var recreated = new MemoryStream();

            MapDoodads.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMapUnitsData), DynamicDataSourceType.Method)]
        public void TestParseMapUnits(string mapUnitsFilePath)
        {
            using var original = FileProvider.GetFile(mapUnitsFilePath);
            using var recreated = new MemoryStream();

            MapUnits.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        private static IEnumerable<object[]> GetMapDoodadsData()
        {
            return TestDataProvider.GetDynamicData(
                MapDoodads.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Widget", "Doodads"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapDoodads.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }

        private static IEnumerable<object[]> GetMapUnitsData()
        {
            return TestDataProvider.GetDynamicData(
                MapUnits.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Widget", "Units"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapUnits.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}