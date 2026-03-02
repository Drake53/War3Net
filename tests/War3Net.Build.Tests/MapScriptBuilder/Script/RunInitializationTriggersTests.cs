namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataRunInitializationTriggers), DynamicDataSourceType.Method)]
        public void TestBodyRunInitializationTriggers(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.RunInitializationTriggers,
                writer => testData.MapScriptBuilder.GenerateRunInitializationTriggers(testData.Map, writer));
        }

        [FlakyTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionRunInitializationTriggers(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.RunInitializationTriggers);
            var actual = testData.MapScriptBuilder.ShouldGenerateRunInitializationTriggers(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataRunInitializationTriggers()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.RunInitializationTriggers))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}