// ------------------------------------------------------------------------------
// <copyright file="InitCustomTriggersTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataInitCustomTriggers), DynamicDataSourceType.Method)]
        public void TestBodyInitCustomTriggers(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["InitCustomTriggers"];
            var actual = testData.MapScriptBuilder.InitCustomTriggers(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitCustomTriggers(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("InitCustomTriggers");
            var actual = testData.MapScriptBuilder.InitCustomTriggersCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitCustomTriggers()
        {
            foreach (var testData in _testData)
            {
                if (!testData.IsMeleeWithoutTrigger && testData.DeclaredFunctions.ContainsKey("InitCustomTriggers"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}