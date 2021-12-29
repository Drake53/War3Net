// ------------------------------------------------------------------------------
// <copyright file="InitCustomTriggersTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    public partial class MapScriptBuilderTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitCustomTriggers(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("InitCustomTriggers");
            var actual = testData.MapScriptBuilder.InitCustomTriggersCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }
    }
}