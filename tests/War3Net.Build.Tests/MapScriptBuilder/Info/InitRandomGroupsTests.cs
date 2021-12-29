// ------------------------------------------------------------------------------
// <copyright file="InitRandomGroupsTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataInitRandomGroups), DynamicDataSourceType.Method)]
        public void TestBodyInitRandomGroups(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["InitRandomGroups"];
            var actual = testData.MapScriptBuilder.InitRandomGroups(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitRandomGroups(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("InitRandomGroups");
            var actual = testData.MapScriptBuilder.InitRandomGroupsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitRandomGroups()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey("InitRandomGroups"))
                {
                    yield return testData;
                }
            }
        }
    }
}