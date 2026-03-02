using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataMain), DynamicDataSourceType.Method)]
        public void TestBodyMain(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.Main,
                writer => testData.MapScriptBuilder.GenerateMain(testData.Map, writer));
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionMain(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.Main);
            var actual = testData.MapScriptBuilder.ShouldGenerateMain(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataMain()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.Main))
                {
                    yield return testData;
                }
            }
        }
    }
}