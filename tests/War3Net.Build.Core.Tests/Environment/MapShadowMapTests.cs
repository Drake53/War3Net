// ------------------------------------------------------------------------------
// <copyright file="MapShadowMapTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;
using War3Net.Common.Testing;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapShadowMapTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMapShadowMapFiles), DynamicDataSourceType.Method)]
        public void TestParseMapShadowMap(string mapShadowMapFile)
        {
            ParseTestHelper.RunBinaryRWTest(mapShadowMapFile, typeof(MapShadowMap));
        }

        private static IEnumerable<object[]> GetMapShadowMapFiles()
        {
            return TestDataProvider.GetDynamicArchiveData(
                MapShadowMap.FileName,
                SearchOption.AllDirectories,
                "Maps");
        }
    }
}