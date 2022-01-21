// ------------------------------------------------------------------------------
// <copyright file="InitCustomPlayerSlotsTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataInitCustomPlayerSlots), DynamicDataSourceType.Method)]
        public void TestBodyInitCustomPlayerSlots(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["InitCustomPlayerSlots"];
            var actual = testData.MapScriptBuilder.InitCustomPlayerSlots(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitCustomPlayerSlots(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("InitCustomPlayerSlots");
            var actual = testData.MapScriptBuilder.InitCustomPlayerSlotsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitCustomPlayerSlots()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("InitCustomPlayerSlots"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}