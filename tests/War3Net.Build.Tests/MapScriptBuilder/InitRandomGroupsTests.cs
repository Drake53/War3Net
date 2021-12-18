// ------------------------------------------------------------------------------
// <copyright file="InitRandomGroupsTests.cs" company="Drake53">
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
        public void TestConditionInitRandomGroups(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("InitRandomGroups");
            var actual = testData.MapScriptBuilder.InitRandomGroupsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }
    }
}