namespace War3Net.Build.Core.Tests.Script
{
    [TestClass]
    public class TriggerStringsTests
    {
        [FlakyTestMethod]
        [DynamicTestData(TestDataFileType.TriggerStrings)]
        public void TestParseTriggerStrings(string triggerStringsFilePath)
        {
            ParseTestHelper.RunStreamRWTest(
                triggerStringsFilePath,
                typeof(TriggerStrings),
                nameof(StreamWriterExtensions.WriteTriggerStrings));
        }
    }
}