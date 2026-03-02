using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataInitGlobals), DynamicDataSourceType.Method)]
        public void TestBodyInitGlobals(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.InitGlobals,
                writer => testData.MapScriptBuilder.GenerateInitGlobals(testData.Map, writer));
        }

        [FlakyTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitGlobals(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitGlobals);
            var actual = testData.MapScriptBuilder.ShouldGenerateInitGlobals(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitGlobals()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitGlobals))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}