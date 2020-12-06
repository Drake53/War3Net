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

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class MapUpgradeObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapUpgradeObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapUpgradeObjectData(string mapUpgradeObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(mapUpgradeObjectDataFilePath, typeof(MapUpgradeObjectData), nameof(BinaryReaderExtensions.ReadUpgradeObjectData), false);
        }

        private static IEnumerable<object[]> GetMapUpgradeObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapUpgradeObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Upgrade"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapUpgradeObjectData.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}