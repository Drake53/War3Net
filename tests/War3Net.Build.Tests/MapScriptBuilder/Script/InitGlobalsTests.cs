// ------------------------------------------------------------------------------
// <copyright file="InitGlobalsTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataInitGlobals), DynamicDataSourceType.Method)]
        public void TestBodyInitGlobals(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["InitGlobals"];
            var actual = testData.MapScriptBuilder.InitGlobals(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitGlobals(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("InitGlobals");
            var actual = testData.MapScriptBuilder.InitGlobalsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitGlobals()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("InitGlobals"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}