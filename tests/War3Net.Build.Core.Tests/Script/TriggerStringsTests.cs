using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Build.Script;
using War3Net.TestTools.UnitTesting;

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