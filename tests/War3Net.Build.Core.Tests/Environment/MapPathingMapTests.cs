// ------------------------------------------------------------------------------
// <copyright file="MapPathingMapTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapPathingMapTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapPathingMapFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseMapPathingMap(string mapPathingMapFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                mapPathingMapFilePath,
                typeof(MapPathingMap));
        }
    }
}