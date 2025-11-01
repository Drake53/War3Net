// ------------------------------------------------------------------------------
// <copyright file="BuffObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class BuffObjectDataTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.BuffObjectData)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<BuffObjectData>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.BuffObjectData)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<BuffObjectData>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.BuffObjectData)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<BuffObjectData>.RunJsonRWTest(filePath, true);
        }
    }
}