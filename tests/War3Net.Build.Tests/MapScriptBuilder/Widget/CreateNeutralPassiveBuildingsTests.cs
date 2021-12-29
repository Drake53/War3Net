// ------------------------------------------------------------------------------
// <copyright file="CreateNeutralPassiveBuildingsTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataCreateNeutralPassiveBuildings), DynamicDataSourceType.Method)]
        public void TestBodyCreateNeutralPassiveBuildings(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["CreateNeutralPassiveBuildings"];
            var actual = testData.MapScriptBuilder.CreateNeutralPassiveBuildings(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateNeutralPassiveBuildings(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("CreateNeutralPassiveBuildings");
            var actual = testData.MapScriptBuilder.CreateNeutralPassiveBuildingsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateNeutralPassiveBuildings()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey("CreateNeutralPassiveBuildings"))
                {
                    yield return testData;
                }
            }
        }
    }
}