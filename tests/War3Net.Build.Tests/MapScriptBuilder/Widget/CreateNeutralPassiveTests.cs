// ------------------------------------------------------------------------------
// <copyright file="CreateNeutralPassiveTests.cs" company="Drake53">
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
        [TestMethod]
        [DynamicData(nameof(GetTestDataCreateNeutralPassive), DynamicDataSourceType.Method)]
        public void TestBodyCreateNeutralPassive(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["CreateNeutralPassive"];
            var actual = testData.MapScriptBuilder.CreateNeutralPassive(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateNeutralPassive(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("CreateNeutralPassive");
            var actual = testData.MapScriptBuilder.CreateNeutralPassiveCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateNeutralPassive()
        {
            foreach (var testData in GetUnobfuscatedTestData())
            {
                if (((MapScriptBuilderTestData)testData[0]).DeclaredFunctions.ContainsKey("CreateNeutralPassive"))
                {
                    yield return testData;
                }
            }
        }
    }
}