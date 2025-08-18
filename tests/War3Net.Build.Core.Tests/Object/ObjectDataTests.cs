// ------------------------------------------------------------------------------
// <copyright file="ObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class ObjectDataTests
    {
        [TestMethod]
        public void TestCreateNewObjectData()
        {
            var objectData = new ObjectData(ObjectDataFormatVersion.v2)
            {
                UnitData = new UnitObjectData(ObjectDataFormatVersion.v2),
                ItemData = new ItemObjectData(ObjectDataFormatVersion.v2),
                DestructableData = new DestructableObjectData(ObjectDataFormatVersion.v2),
                DoodadData = new DoodadObjectData(ObjectDataFormatVersion.v2),
                AbilityData = new AbilityObjectData(ObjectDataFormatVersion.v2),
                BuffData = new BuffObjectData(ObjectDataFormatVersion.v2),
                UpgradeData = new UpgradeObjectData(ObjectDataFormatVersion.v2),
            };

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream);
            writer.Write(objectData);

            memoryStream.Position = 0;

            using var reader = new BinaryReader(memoryStream);
            reader.ReadObjectData();
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.ObjectData)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<ObjectData>.RunBinaryRWTest(filePath);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.ObjectData)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<ObjectData>.RunJsonRWTest(filePath, false);
        }

        [TestMethod]
        [DynamicTestData(TestDataFileType.ObjectData)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<ObjectData>.RunJsonRWTest(filePath, true);
        }
    }
}