using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataConfig), DynamicDataSourceType.Method)]
        public void TestBodyConfig(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.Config,
                writer => testData.MapScriptBuilder.GenerateConfig(testData.Map, writer));
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionConfig(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.Config);
            var actual = testData.MapScriptBuilder.ShouldGenerateConfig(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataConfig()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.Config))
                {
                    yield return testData;
                }
            }
        }
    }
}