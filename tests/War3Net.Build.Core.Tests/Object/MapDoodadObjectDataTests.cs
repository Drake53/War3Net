// ------------------------------------------------------------------------------
// <copyright file="MapDoodadObjectDataTests.cs" company="Drake53">
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
    public class MapDoodadObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapDoodadObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapDoodadObjectData(string mapDoodadObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapDoodadObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapDoodadObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetMapDoodadObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapDoodadObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Doodad"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapDoodadObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}