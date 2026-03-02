using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Info;

namespace War3Net.Build.Core.Tests.Info
{
    [TestClass]
    public class CampaignInfoTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.CampaignInfo)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<CampaignInfo>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.CampaignInfo)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<CampaignInfo>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.CampaignInfo)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<CampaignInfo>.RunJsonRWTest(filePath, true);
        }
    }
}