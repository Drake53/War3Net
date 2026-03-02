using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTestDataInitRandomGroups), DynamicDataSourceType.Method)]
        public void TestBodyInitRandomGroups(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.InitRandomGroups,
                writer => testData.MapScriptBuilder.GenerateInitRandomGroups(testData.Map, writer));
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitRandomGroups(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitRandomGroups);
            var actual = testData.MapScriptBuilder.ShouldGenerateInitRandomGroups(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitRandomGroups()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.InitRandomGroups))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}