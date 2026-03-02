using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class UpgradeObjectDataTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.UpgradeObjectData)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<UpgradeObjectData>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.UpgradeObjectData)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<UpgradeObjectData>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.UpgradeObjectData)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<UpgradeObjectData>.RunJsonRWTest(filePath, true);
        }
    }
}