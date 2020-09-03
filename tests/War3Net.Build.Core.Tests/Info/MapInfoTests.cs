// ------------------------------------------------------------------------------
// <copyright file="MapInfoTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Info;
using War3Net.Common.Testing;
using War3Net.IO.Mpq;

namespace War3Net.Build.Core.Tests.Info
{
    [TestClass]
    public class MapInfoTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapInfoData), DynamicDataSourceType.Method)]
        public void TestParseMapInfo(string mapInfoFilePath)
        {
            using var original = FileProvider.GetFile(mapInfoFilePath);
            using var recreated = new MemoryStream();

            MapInfo.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true);
        }

        private static IEnumerable<object[]> GetMapInfoData()
        {
            return TestDataProvider.GetDynamicData(
                MapInfo.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Info"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapInfo.FileName,
                SearchOption.TopDirectoryOnly,
                "Maps"));
        }
    }
}