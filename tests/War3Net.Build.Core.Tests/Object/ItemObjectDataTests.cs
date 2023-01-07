// ------------------------------------------------------------------------------
// <copyright file="ItemObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class ItemObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetItemObjectDataFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<ItemObjectData>.RunBinaryRWTest(filePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetItemObjectDataFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<ItemObjectData>.RunJsonRWTest(filePath, false);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetItemObjectDataFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<ItemObjectData>.RunJsonRWTest(filePath, true);
        }
    }
}