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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class MapDestructableObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapDestructableObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapDestructableObjectData(string mapDestructableObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(mapDestructableObjectDataFilePath, typeof(MapDestructableObjectData), nameof(BinaryReaderExtensions.ReadDestructableObjectData), false);
        }

        private static IEnumerable<object[]> GetMapDestructableObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapDestructableObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Destructable"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapDestructableObjectData.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}