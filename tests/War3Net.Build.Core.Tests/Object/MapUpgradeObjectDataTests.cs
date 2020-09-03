// ------------------------------------------------------------------------------
// <copyright file="MapUpgradeObjectDataTests.cs" company="Drake53">
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
    public class MapUpgradeObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapUpgradeObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapUpgradeObjectData(string mapUpgradeObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapUpgradeObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapUpgradeObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetMapUpgradeObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapUpgradeObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Upgrade"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapUpgradeObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}