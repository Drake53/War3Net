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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class MapItemObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapItemObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapItemObjectData(string mapItemObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(mapItemObjectDataFilePath, typeof(MapItemObjectData), nameof(BinaryReaderExtensions.ReadItemObjectData), false);
        }

        private static IEnumerable<object[]> GetMapItemObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapItemObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Item"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapItemObjectData.FileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                "war3mapSkin.w3t",
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}