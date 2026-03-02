using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapCamerasTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.MapCameras)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapCameras>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapCameras)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapCameras>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapCameras)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapCameras>.RunJsonRWTest(filePath, true);
        }
    }
}