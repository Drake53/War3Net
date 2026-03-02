namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTestDataCreateNeutralUnits), DynamicDataSourceType.Method)]
        public void TestBodyCreateNeutralUnits(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.CreateNeutralUnits,
                writer => testData.MapScriptBuilder.GenerateCreateNeutralUnits(testData.Map, writer));
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateNeutralUnits(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreateNeutralUnits);
            var actual = testData.MapScriptBuilder.ShouldGenerateCreateNeutralUnits(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateNeutralUnits()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreateNeutralUnits))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}