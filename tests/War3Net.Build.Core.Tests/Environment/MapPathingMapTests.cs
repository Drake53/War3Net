// ------------------------------------------------------------------------------
// <copyright file="MapPathingMapTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapPathingMapTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapPathingMapFiles), DynamicDataSourceType.Method)]
        public void TestParseMapPathingMap(string mapPathingMapFile)
        {
            ParseTestHelper.RunBinaryRWTest(mapPathingMapFile, typeof(MapPathingMap));
        }

        private static IEnumerable<object[]> GetMapPathingMapFiles()
        {
            return TestDataProvider.GetDynamicData(
                MapPathingMap.FileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Pathing"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapPathingMap.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}