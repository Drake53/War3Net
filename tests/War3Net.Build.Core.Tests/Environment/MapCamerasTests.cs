// ------------------------------------------------------------------------------
// <copyright file="MapCamerasTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapCamerasTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapCamerasFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseMapCameras(string mapCamerasFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                mapCamerasFilePath,
                typeof(MapCameras));
        }
    }
}