namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTestDataCreateCameras), DynamicDataSourceType.Method)]
        public void TestBodyCreateCameras(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.CreateCameras,
                writer => testData.MapScriptBuilder.GenerateCreateCameras(testData.Map, writer));
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateCameras(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreateCameras);
            var actual = testData.MapScriptBuilder.ShouldGenerateCreateCameras(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateCameras()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreateCameras))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}