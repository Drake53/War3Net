// ------------------------------------------------------------------------------
// <copyright file="CreateCamerasTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetTestDataCreateCameras), DynamicDataSourceType.Method)]
        public void TestBodyCreateCameras(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions["CreateCameras"];
            var actual = testData.MapScriptBuilder.CreateCameras(testData.Map);

            SyntaxAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(GetUnobfuscatedTestData), DynamicDataSourceType.Method)]
        public void TestConditionCreateCameras(MapScriptBuilderTestData testData)
        {
            var expected = testData.DeclaredFunctions.ContainsKey("CreateCameras");
            var actual = testData.MapScriptBuilder.CreateCamerasCondition(testData.Map);

            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object?[]> GetTestDataCreateCameras()
        {
            foreach (var testData in _testData)
            {
                if (testData.DeclaredFunctions.ContainsKey("CreateCameras"))
                {
                    yield return new object[] { testData };
                }
            }
        }
    }
}