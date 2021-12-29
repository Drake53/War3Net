// ------------------------------------------------------------------------------
// <copyright file="RunInitializationTriggersTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataRunInitializationTriggers), DynamicDataSourceType.Method)]
        public void TestBodyRunInitializationTriggers(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["RunInitializationTriggers"];
            var actual = testData.MapScriptBuilder.RunInitializationTriggers(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionRunInitializationTriggers(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("RunInitializationTriggers");
            var actual = testData.MapScriptBuilder.RunInitializationTriggersCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataRunInitializationTriggers()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey("RunInitializationTriggers"))
                {
                    yield return testData;
                }
            }
        }
    }
}