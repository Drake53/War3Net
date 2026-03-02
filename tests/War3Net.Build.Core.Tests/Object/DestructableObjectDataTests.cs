using Microsoft.VisualStudio.TestTools.UnitTesting;
using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class DestructableObjectDataTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.DestructableObjectData)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<DestructableObjectData>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.DestructableObjectData)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<DestructableObjectData>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.DestructableObjectData)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<DestructableObjectData>.RunJsonRWTest(filePath, true);
        }
    }
}