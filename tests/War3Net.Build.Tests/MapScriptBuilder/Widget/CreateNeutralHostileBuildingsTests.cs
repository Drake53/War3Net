namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataCreateNeutralHostileBuildings), DynamicDataSourceType.Method)]
        public void TestBodyCreateNeutralHostileBuildings(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.CreateNeutralHostileBuildings,
                writer => testData.MapScriptBuilder.GenerateCreateNeutralHostileBuildings(testData.Map, writer));
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateNeutralHostileBuildings(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreateNeutralHostileBuildings);
            var actual = testData.MapScriptBuilder.ShouldGenerateCreateNeutralHostileBuildings(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateNeutralHostileBuildings()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreateNeutralHostileBuildings))
                {
                    yield return testData;
                }
            }
        }
    }
}