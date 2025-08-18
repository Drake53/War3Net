// ------------------------------------------------------------------------------
// <copyright file="MapDoodadsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Widget;

namespace War3Net.Build.Core.Tests.Widget
{
    [TestClass]
    public class MapDoodadsTests
    {
        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapDoodadsFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapDoodads>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapDoodadsFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapDoodads>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapDoodadsFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapDoodads>.RunJsonRWTest(filePath, true);
        }
    }
}