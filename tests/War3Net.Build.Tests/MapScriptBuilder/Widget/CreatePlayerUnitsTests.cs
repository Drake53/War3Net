using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataCreatePlayerUnits), DynamicDataSourceType.Method)]
        public void TestBodyCreatePlayerUnits(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.CreatePlayerUnits,
                writer => testData.MapScriptBuilder.GenerateCreatePlayerUnits(testData.Map, writer));
        }

        [FlakyTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreatePlayerUnits(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreatePlayerUnits);
            var actual = testData.MapScriptBuilder.ShouldGenerateCreatePlayerUnits(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreatePlayerUnits()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreatePlayerUnits))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}