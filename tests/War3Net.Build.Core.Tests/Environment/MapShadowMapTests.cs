// ------------------------------------------------------------------------------
// <copyright file="MapShadowMapTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Environment;

namespace War3Net.Build.Core.Tests.Environment
{
    [TestClass]
    public class MapShadowMapTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.MapShadowMap)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapShadowMap>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapShadowMap)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapShadowMap>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.MapShadowMap)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapShadowMap>.RunJsonRWTest(filePath, true);
        }
    }
}