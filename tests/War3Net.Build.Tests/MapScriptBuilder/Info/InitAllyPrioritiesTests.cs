// ------------------------------------------------------------------------------
// <copyright file="InitAllyPrioritiesTests.cs" company="Drake53">
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
        [FlakyTestMethod]
        [DynamicData(nameof(GetTestDataInitAllyPriorities), DynamicDataSourceType.Method)]
        public void TestBodyInitAllyPriorities(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["InitAllyPriorities"];
            var actual = testData.MapScriptBuilder.InitAllyPriorities(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitAllyPriorities(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("InitAllyPriorities");
            var actual = testData.MapScriptBuilder.InitAllyPrioritiesCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitAllyPriorities()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("InitAllyPriorities"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}