namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataInitCustomPlayerSlots), DynamicDataSourceType.Method)]
        public void TestBodyInitCustomPlayerSlots(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.InitCustomPlayerSlots,
                writer => testData.MapScriptBuilder.GenerateInitCustomPlayerSlots(testData.Map, writer));
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitCustomPlayerSlots(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitCustomPlayerSlots);
            var actual = testData.MapScriptBuilder.ShouldGenerateInitCustomPlayerSlots(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitCustomPlayerSlots()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitCustomPlayerSlots))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}