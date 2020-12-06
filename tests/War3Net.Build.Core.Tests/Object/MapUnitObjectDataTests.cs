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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class MapUnitObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapUnitObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapUnitObjectData(string mapUnitObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(mapUnitObjectDataFilePath, typeof(MapUnitObjectData), nameof(BinaryReaderExtensions.ReadUnitObjectData), false);
        }

        private static IEnumerable<object[]> GetMapUnitObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapUnitObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Unit"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapUnitObjectData.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}