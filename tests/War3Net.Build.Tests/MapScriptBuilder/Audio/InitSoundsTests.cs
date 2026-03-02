using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataInitSounds), DynamicDataSourceType.Method)]
        public void TestBodyInitSounds(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.InitSounds,
                writer => testData.MapScriptBuilder.GenerateInitSounds(testData.Map, writer));
        }

        [FlakyTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitSounds(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitSounds);
            var actual = testData.MapScriptBuilder.ShouldGenerateInitSounds(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitSounds()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitSounds))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}