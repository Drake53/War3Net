using Microsoft.VisualStudio.TestTools.UnitTesting;
using War3Net.Build.Audio;

namespace War3Net.Build.Core.Tests.Audio
{
    [TestClass]
    public class MapSoundsTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.MapSounds)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapSounds>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapSounds)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapSounds>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapSounds)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapSounds>.RunJsonRWTest(filePath, true);
        }
    }
}