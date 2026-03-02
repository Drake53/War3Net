using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class AbilityObjectDataTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.AbilityObjectData)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<AbilityObjectData>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.AbilityObjectData)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<AbilityObjectData>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.AbilityObjectData)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<AbilityObjectData>.RunJsonRWTest(filePath, true);
        }
    }
}