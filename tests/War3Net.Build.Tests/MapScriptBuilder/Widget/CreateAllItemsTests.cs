// ------------------------------------------------------------------------------
// <copyright file="CreateAllItemsTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataCreateAllItems), DynamicDataSourceType.Method)]
        public void TestBodyCreateAllItems(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["CreateAllItems"];
            var actual = testData.MapScriptBuilder.CreateAllItems(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateAllItems(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("CreateAllItems");
            var actual = testData.MapScriptBuilder.CreateAllItemsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateAllItems()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("CreateAllItems"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}