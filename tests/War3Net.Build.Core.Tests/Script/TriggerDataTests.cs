namespace War3Net.Build.Core.Tests.Script
{
    [TestClass]
    public class TriggerDataTests
    {
        [TestMethod]
        public void TestParseEmbeddedTriggerData()
        {
            _ = TriggerData.Default;
        }

        [TestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestParseTriggerData(string triggerDataPath)
        {
            var triggerDataText = File.ReadAllText(triggerDataPath);
            var reader = new StringReader(triggerDataText);

            reader.ReadTriggerData();
        }

        private static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { TestDataProvider.GetPath("Script/TriggerData/triggerdata_1_31_1_PTR.txt") };
            yield return new object[] { TestDataProvider.GetPath("Script/TriggerData/triggerdata_1_32_9.txt") };
        }
    }
}