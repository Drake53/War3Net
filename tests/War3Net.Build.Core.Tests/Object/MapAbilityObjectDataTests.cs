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

using War3Net.Common.Testing;
using War3Net.IO.Mpq;

namespace War3Net.Build.Object
{
    [TestClass]
    public class MapAbilityObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapAbilityObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapAbilityObjectData(string mapAbilityObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(mapAbilityObjectDataFilePath);
            using var recreated = new MemoryStream();

            MapAbilityObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetMapAbilityObjectData()
        {
            return TestDataProvider.GetDynamicData(
                MapAbilityObjectData.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", "Ability"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapAbilityObjectData.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}