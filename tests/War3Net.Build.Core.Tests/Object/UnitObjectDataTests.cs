// ------------------------------------------------------------------------------
// <copyright file="UnitObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class UnitObjectDataTests
    {
        [TestMethod]
        [DynamicTestData(TestDataFileType.UnitObjectData)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<UnitObjectData>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.UnitObjectData)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<UnitObjectData>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.UnitObjectData)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<UnitObjectData>.RunJsonRWTest(filePath, true);
        }
    }
}