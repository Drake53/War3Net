using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataInitCustomTriggers), DynamicDataSourceType.Method)]
        public void TestBodyInitCustomTriggers(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.InitCustomTriggers,
                writer => testData.MapScriptBuilder.GenerateInitCustomTriggers(testData.Map, writer));
        }

        [FlakyTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitCustomTriggers(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitCustomTriggers);
            var actual = testData.MapScriptBuilder.ShouldGenerateInitCustomTriggers(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitCustomTriggers()
        {
            foreach (var testData in _testData)
            {
                if (!testData.IsMeleeWithoutTrigger && testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitCustomTriggers))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}