// ------------------------------------------------------------------------------
// <copyright file="CreateAllDestructablesTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataCreateAllDestructables), DynamicDataSourceType.Method)]
        public void TestBodyCreateAllDestructables(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["CreateAllDestructables"];
            var actual = testData.MapScriptBuilder.CreateAllDestructables(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateAllDestructables(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("CreateAllDestructables");
            var actual = testData.MapScriptBuilder.CreateAllDestructablesCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateAllDestructables()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("CreateAllDestructables"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}