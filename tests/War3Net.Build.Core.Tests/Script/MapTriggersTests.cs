namespace War3Net.Build.Core.Tests.Script
{
    [TestClass]
    public class MapTriggersTests
    {
        [FlakyTestMethod]
        [DynamicTestData(TestDataFileType.MapTriggers)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapTriggers>.RunBinaryRWTest(filePath);
        }

        [FlakyTestMethod]
        [DynamicTestData(TestDataFileType.MapTriggers)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapTriggers>.RunJsonRWTest(filePath, false);
        }

        [FlakyTestMethod]
        [DynamicTestData(TestDataFileType.MapTriggers)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapTriggers>.RunJsonRWTest(filePath, true);
        }
    }
}