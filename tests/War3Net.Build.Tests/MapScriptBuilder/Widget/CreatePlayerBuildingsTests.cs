// ------------------------------------------------------------------------------
// <copyright file="CreatePlayerBuildingsTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataCreatePlayerBuildings), DynamicDataSourceType.Method)]
        public void TestBodyCreatePlayerBuildings(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["CreatePlayerBuildings"];
            var actual = testData.MapScriptBuilder.CreatePlayerBuildings(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreatePlayerBuildings(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("CreatePlayerBuildings");
            var actual = testData.MapScriptBuilder.CreatePlayerBuildingsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreatePlayerBuildings()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey("CreatePlayerBuildings"))
                {
                    yield return testData;
                }
            }
        }
    }
}