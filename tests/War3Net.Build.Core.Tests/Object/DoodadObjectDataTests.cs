using Microsoft.VisualStudio.TestTools.UnitTesting;
using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class DoodadObjectDataTests
    {
        [TestMethod]
        [FlakyDynamicTestData(
            TestDataFileType.DoodadObjectData,
            "PKCompressed.w3x/war3map.w3d",
            "306913.w3x/war3map.w3d")]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<DoodadObjectData>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [FlakyDynamicTestData(
            TestDataFileType.DoodadObjectData,
            "PKCompressed.w3x/war3map.w3d",
            "306913.w3x/war3map.w3d")]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<DoodadObjectData>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [FlakyDynamicTestData(
            TestDataFileType.DoodadObjectData,
            "PKCompressed.w3x/war3map.w3d",
            "306913.w3x/war3map.w3d")]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<DoodadObjectData>.RunJsonRWTest(filePath, true);
        }
    }
}