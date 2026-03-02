using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataCreateAllItems), DynamicDataSourceType.Method)]
        public void TestBodyCreateAllItems(MapScriptBuilderTestData testData)
        {
            AssertFunctionGeneratedCorrectly(
                testData,
                MapScriptBuilder.GeneratedFunctionName.CreateAllItems,
                writer => testData.MapScriptBuilder.GenerateCreateAllItems(testData.Map, writer));
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateAllItems(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreateAllItems);
            var actual = testData.MapScriptBuilder.ShouldGenerateCreateAllItems(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateAllItems()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey(MapScriptBuilder.GeneratedFunctionName.CreateAllItems))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}