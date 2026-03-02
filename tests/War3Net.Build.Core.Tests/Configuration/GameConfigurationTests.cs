namespace War3Net.Build.Core.Tests.Configuration
{
    [TestClass]
    public class GameConfigurationTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.GameConfiguration)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<GameConfiguration>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.GameConfiguration)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<GameConfiguration>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.GameConfiguration)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<GameConfiguration>.RunJsonRWTest(filePath, true);
        }
    }
}