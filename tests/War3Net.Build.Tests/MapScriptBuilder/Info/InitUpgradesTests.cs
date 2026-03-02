using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataInitUpgrades), DynamicDataSourceType.Method)]
        public void TestBodyInitUpgrades(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.InitUpgrades,
                writer => testData.MapScriptBuilder.GenerateInitUpgrades(testData.Map, writer));
        }

        [FlakyTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitUpgrades(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitUpgrades);
            var actual = testData.MapScriptBuilder.ShouldGenerateInitUpgrades(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitUpgrades()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitUpgrades))
                {
                    yield return testData;
                }
            }
        }
    }
}