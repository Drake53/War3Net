using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapPathingMapTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.MapPathingMap)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapPathingMap>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapPathingMap)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapPathingMap>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapPathingMap)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapPathingMap>.RunJsonRWTest(filePath, true);
        }
    }
}