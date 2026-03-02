using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [FlakyTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionDestructableItemTables(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("DestructableItemTables");
            var actual = testData.MapScriptBuilder.ShouldGenerateDestructableItemTables(testData.Map);

            Assert.AreEqual(expected, actual);
        }
    }
}