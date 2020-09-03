// ------------------------------------------------------------------------------
// <copyright file="MapDestructableObjectDataTests.cs" company="Drake53">
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
    public class MapDestructableObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapDestructableObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapDestructableObjectData(string mapDestructableObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapDestructableObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapDestructableObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetMapDestructableObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapDestructableObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Destructable"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapDestructableObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}