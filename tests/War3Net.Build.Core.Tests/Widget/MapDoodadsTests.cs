// ------------------------------------------------------------------------------
// <copyright file="MapDoodadsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Widget;

namespace War3Net.Build.Core.Tests.Widget
{
    [TestClass]
    public class MapDoodadsTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapDoodadsFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseMapDoodads(string mapDoodadsFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                mapDoodadsFilePath,
                typeof(MapDoodads));
        }
    }
}