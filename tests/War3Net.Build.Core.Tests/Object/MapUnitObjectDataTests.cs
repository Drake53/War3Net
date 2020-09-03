// ------------------------------------------------------------------------------
// <copyright file="MapUnitObjectDataTests.cs" company="Drake53">
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
    public class MapUnitObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapUnitObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapUnitObjectData(string mapUnitObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapUnitObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapUnitObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetMapUnitObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapUnitObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Unit"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapUnitObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}