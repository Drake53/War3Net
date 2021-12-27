// ------------------------------------------------------------------------------
// <copyright file="ObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.IO.Mpq;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class ObjectDataTests
    {
        [TestMethod]
        public void TestCreateNewObjectDataCampaign()
        {
            var objectData = new ObjectData(ObjectDataFormatVersion.Normal)
            {
                UnitData = new CampaignUnitObjectData(ObjectDataFormatVersion.Normal),
                ItemData = new CampaignItemObjectData(ObjectDataFormatVersion.Normal),
                DestructableData = new CampaignDestructableObjectData(ObjectDataFormatVersion.Normal),
                DoodadData = new CampaignDoodadObjectData(ObjectDataFormatVersion.Normal),
                AbilityData = new CampaignAbilityObjectData(ObjectDataFormatVersion.Normal),
                BuffData = new CampaignBuffObjectData(ObjectDataFormatVersion.Normal),
                UpgradeData = new CampaignUpgradeObjectData(ObjectDataFormatVersion.Normal),
            };

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream);
            writer.Write(objectData);

            memoryStream.Position = 0;

            using var reader = new BinaryReader(memoryStream);
            reader.ReadObjectData(true);
        }

        [TestMethod]
        public void TestCreateNewObjectDataMap()
        {
            var objectData = new ObjectData(ObjectDataFormatVersion.Normal)
            {
                UnitData = new MapUnitObjectData(ObjectDataFormatVersion.Normal),
                ItemData = new MapItemObjectData(ObjectDataFormatVersion.Normal),
                DestructableData = new MapDestructableObjectData(ObjectDataFormatVersion.Normal),
                DoodadData = new MapDoodadObjectData(ObjectDataFormatVersion.Normal),
                AbilityData = new MapAbilityObjectData(ObjectDataFormatVersion.Normal),
                BuffData = new MapBuffObjectData(ObjectDataFormatVersion.Normal),
                UpgradeData = new MapUpgradeObjectData(ObjectDataFormatVersion.Normal),
            };

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream);
            writer.Write(objectData);

            memoryStream.Position = 0;

            using var reader = new BinaryReader(memoryStream);
            reader.ReadObjectData(false);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignObjectData(string objectDataFilePath)
        {
            TestParseObjectDataInternal(objectDataFilePath, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetObjectData), DynamicDataSourceType.Method)]
        public void TestParseMapObjectData(string objectDataFilePath)
        {
            TestParseObjectDataInternal(objectDataFilePath, false);
        }

        private void TestParseObjectDataInternal(string objectDataFilePath, bool fromCampaign)
        {
            using var original = MpqFile.OpenRead(objectDataFilePath);
            using var recreated = new MemoryStream();

            using var objectDataReader = new BinaryReader(original);
            var objectData = objectDataReader.ReadObjectData(fromCampaign);

            using var objectDataWriter = new BinaryWriter(recreated);
            objectDataWriter.Write(objectData);

            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetObjectData()
        {
            return TestDataProvider.GetDynamicData(
                "*.w3o",
                SearchOption.AllDirectories,
                Path.Combine("Object", "All"));
        }
    }
}