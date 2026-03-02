namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataCreateNeutralPassiveBuildings), DynamicDataSourceType.Method)]
        public void TestBodyCreateNeutralPassiveBuildings(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.CreateNeutralPassiveBuildings,
                writer => testData.MapScriptBuilder.GenerateCreateNeutralPassiveBuildings(testData.Map, writer));
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateNeutralPassiveBuildings(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreateNeutralPassiveBuildings);
            var actual = testData.MapScriptBuilder.ShouldGenerateCreateNeutralPassiveBuildings(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateNeutralPassiveBuildings()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreateNeutralPassiveBuildings))
                {
                    yield return testData;
                }
            }
        }
    }
}