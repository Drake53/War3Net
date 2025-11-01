// ------------------------------------------------------------------------------
// <copyright file="InitSoundsTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataInitSounds), DynamicDataSourceType.Method)]
        public void TestBodyInitSounds(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["InitSounds"];
            var actual = testData.MapScriptBuilder.InitSounds(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [FlakyTestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionInitSounds(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("InitSounds");
            var actual = testData.MapScriptBuilder.InitSoundsCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataInitSounds()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("InitSounds"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}