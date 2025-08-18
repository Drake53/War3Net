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
        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapCamerasFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapCameras>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapCamerasFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapCameras>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapCamerasFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapCameras>.RunJsonRWTest(filePath, true);
        }
    }
}