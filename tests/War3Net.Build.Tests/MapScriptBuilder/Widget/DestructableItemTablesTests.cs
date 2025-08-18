// ------------------------------------------------------------------------------
// <copyright file="DestructableItemTablesTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionDestructableItemTables(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("DestructableItemTables");
            var actual = testData.MapScriptBuilder.DestructableItemTablesCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }
    }
}