// ------------------------------------------------------------------------------
// <copyright file="MapItemObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Common.Testing;
using War3Net.IO.Mpq;

namespace War3Net.Build.Object
{
    [TestClass]
    public class MapItemObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapItemObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapItemObjectData(string mapItemObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapItemObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapItemObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetMapItemObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapItemObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Item"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapItemObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}