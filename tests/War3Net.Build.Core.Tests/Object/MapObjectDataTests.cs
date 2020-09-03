// ------------------------------------------------------------------------------
// <copyright file="MapObjectDataTests.cs" company="Drake53">
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
    public class MapObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapObjectData(string mapObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetMapObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "All"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}