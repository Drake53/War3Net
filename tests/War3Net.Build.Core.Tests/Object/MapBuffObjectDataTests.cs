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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class MapBuffObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapBuffObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapBuffObjectData(string mapBuffObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(mapBuffObjectDataFilePath, typeof(MapBuffObjectData), nameof(BinaryReaderExtensions.ReadBuffObjectData), false);
        }

        private static IEnumerable<object[]> GetMapBuffObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapBuffObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Buff"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapBuffObjectData.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}