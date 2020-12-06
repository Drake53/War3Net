// ------------------------------------------------------------------------------
// <copyright file="MapAbilityObjectDataTests.cs" company="Drake53">
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
    public class MapAbilityObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapAbilityObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapAbilityObjectData(string mapAbilityObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(mapAbilityObjectDataFilePath, typeof(MapAbilityObjectData), nameof(BinaryReaderExtensions.ReadAbilityObjectData), false);
        }

        private static IEnumerable<object[]> GetMapAbilityObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapAbilityObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Ability"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapAbilityObjectData.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}