// ------------------------------------------------------------------------------
// <copyright file="CreateAllUnitsTests.cs" company="Drake53">
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
        [DataTestMethod]
        [DynamicData(nameof(GetTestDataCreateAllUnits), DynamicDataSourceType.Method)]
        public void TestBodyCreateAllUnits(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["CreateAllUnits"];
            var actual = testData.MapScriptBuilder.CreateAllUnits(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateAllUnits(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("CreateAllUnits");
            var actual = testData.MapScriptBuilder.CreateAllUnitsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateAllUnits()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("CreateAllUnits"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}