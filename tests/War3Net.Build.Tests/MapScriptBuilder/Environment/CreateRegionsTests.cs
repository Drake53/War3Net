// ------------------------------------------------------------------------------
// <copyright file="CreateRegionsTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataCreateRegions), DynamicDataSourceType.Method)]
        public void TestBodyCreateRegions(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["CreateRegions"];
            var actual = testData.MapScriptBuilder.CreateRegions(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateRegions(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("CreateRegions");
            var actual = testData.MapScriptBuilder.CreateRegionsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateRegions()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("CreateRegions"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}