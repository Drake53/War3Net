// ------------------------------------------------------------------------------
// <copyright file="MapShadowMapTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapShadowMapTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapShadowMapFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseMapShadowMap(string mapShadowMapFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                mapShadowMapFilePath,
                typeof(MapShadowMap));
        }
    }
}