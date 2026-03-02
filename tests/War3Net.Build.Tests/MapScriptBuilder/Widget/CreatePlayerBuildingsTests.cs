namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTestDataCreatePlayerBuildings), DynamicDataSourceType.Method)]
        public void TestBodyCreatePlayerBuildings(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.CreatePlayerBuildings,
                writer => testData.MapScriptBuilder.GenerateCreatePlayerBuildings(testData.Map, writer));
        }

        [FlakyTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreatePlayerBuildings(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreatePlayerBuildings);
            var actual = testData.MapScriptBuilder.ShouldGenerateCreatePlayerBuildings(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreatePlayerBuildings()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreatePlayerBuildings))
                {
                    yield return testData;
                }
            }
        }
    }
}