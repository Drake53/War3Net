// ------------------------------------------------------------------------------
// <copyright file="MapRegionsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapRegionsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapRegionsFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseMapRegions(string mapRegionsFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                mapRegionsFilePath,
                typeof(MapRegions));
        }
    }
}