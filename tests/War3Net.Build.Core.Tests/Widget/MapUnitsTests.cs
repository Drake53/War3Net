// ------------------------------------------------------------------------------
// <copyright file="MapUnitsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Widget;
using War3Net.Common.Testing;
using War3Net.IO.Mpq;

namespace War3Net.Build.Core.Tests.Widget
{
    [TestClass]
    public class MapUnitsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapUnitsData), DynamicDataSourceType.Method)]
        public void TestParseMapUnits(string mapUnitsFilePath)
        {
            using var original = FileProvider.GetFile(mapUnitsFilePath);
            using var recreated = new MemoryStream();

            MapUnits.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
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