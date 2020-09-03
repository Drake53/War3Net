// ------------------------------------------------------------------------------
// <copyright file="MapBuffObjectDataTests.cs" company="Drake53">
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
    public class MapBuffObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapBuffObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapBuffObjectData(string mapBuffObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapBuffObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapBuffObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetMapBuffObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapBuffObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Buff"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapBuffObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}