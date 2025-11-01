// ------------------------------------------------------------------------------
// <copyright file="CreateNeutralUnitsTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataCreateNeutralUnits), DynamicDataSourceType.Method)]
        public void TestBodyCreateNeutralUnits(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["CreateNeutralUnits"];
            var actual = testData.MapScriptBuilder.CreateNeutralUnits(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateNeutralUnits(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("CreateNeutralUnits");
            var actual = testData.MapScriptBuilder.CreateNeutralUnitsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateNeutralUnits()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("CreateNeutralUnits"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}