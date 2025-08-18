// ------------------------------------------------------------------------------
// <copyright file="CreateNeutralHostileTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataCreateNeutralHostile), DynamicDataSourceType.Method)]
        public void TestBodyCreateNeutralHostile(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["CreateNeutralHostile"];
            var actual = testData.MapScriptBuilder.CreateNeutralHostile(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateNeutralHostile(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("CreateNeutralHostile");
            var actual = testData.MapScriptBuilder.CreateNeutralHostileCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateNeutralHostile()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey("CreateNeutralHostile"))
                {
                    yield return testData;
                }
            }
        }
    }
}