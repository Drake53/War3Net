// ------------------------------------------------------------------------------
// <copyright file="MapTriggersTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Script;

namespace War3Net.Build.Core.Tests.Script
{
    [TestClass]
    public class MapTriggersTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapTriggersFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<MapTriggers>.RunBinaryRWTest(filePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapTriggersFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<MapTriggers>.RunJsonRWTest(filePath, false);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetMapTriggersFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<MapTriggers>.RunJsonRWTest(filePath, true);
        }
    }
}