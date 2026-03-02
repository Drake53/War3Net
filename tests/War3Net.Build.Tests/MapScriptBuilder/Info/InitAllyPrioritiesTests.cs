namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataInitAllyPriorities), DynamicDataSourceType.Method)]
        public void TestBodyInitAllyPriorities(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.InitAllyPriorities,
                writer => testData.MapScriptBuilder.GenerateInitAllyPriorities(testData.Map, writer));
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitAllyPriorities(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitAllyPriorities);
            var actual = testData.MapScriptBuilder.ShouldGenerateInitAllyPriorities(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitAllyPriorities()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitAllyPriorities))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}