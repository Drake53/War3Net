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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class MapDoodadObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapDoodadObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapDoodadObjectData(string mapDoodadObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(mapDoodadObjectDataFilePath, typeof(MapDoodadObjectData), nameof(BinaryReaderExtensions.ReadDoodadObjectData), false);
        }

        private static IEnumerable<object[]> GetMapDoodadObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapDoodadObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Doodad"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapDoodadObjectData.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}