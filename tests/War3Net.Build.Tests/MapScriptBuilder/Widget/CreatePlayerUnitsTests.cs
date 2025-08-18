// ------------------------------------------------------------------------------
// <copyright file="CreatePlayerUnitsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTestDataCreatePlayerUnits), DynamicDataSourceType.Method)]
        public void TestBodyCreatePlayerUnits(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["CreatePlayerUnits"];
            var actual = testData.MapScriptBuilder.CreatePlayerUnits(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreatePlayerUnits(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("CreatePlayerUnits");
            var actual = testData.MapScriptBuilder.CreatePlayerUnitsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreatePlayerUnits()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("CreatePlayerUnits"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}