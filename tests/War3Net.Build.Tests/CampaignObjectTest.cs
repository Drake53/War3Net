// ------------------------------------------------------------------------------
// <copyright file="CampaignObjectTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;
using War3Net.Build.Providers;
using War3Net.Common.Testing;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class CampaignObjectTest
    {
        [TestMethod]
        public void TestCreateNewObjectData()
        {
            var objectData = new CampaignObjectData(
                new CampaignUnitObjectData(Array.Empty<ObjectModification>()),
                new CampaignItemObjectData(Array.Empty<ObjectModification>()),
                new CampaignDestructableObjectData(Array.Empty<ObjectModification>()),
                new CampaignDoodadObjectData(Array.Empty<ObjectModification>()),
                new CampaignAbilityObjectData(Array.Empty<ObjectModification>()),
                new CampaignBuffObjectData(Array.Empty<ObjectModification>()),
                new CampaignUpgradeObjectData(Array.Empty<ObjectModification>()));

            using var memoryStream = new MemoryStream();
            objectData.SerializeTo(memoryStream, true);

            memoryStream.Position = 0;
            CampaignObjectData.Parse(memoryStream);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetCampaignUnitObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignUnitObjectData(string campaignUnitObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignUnitObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignUnitObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetCampaignItemObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignItemObjectData(string campaignItemObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignItemObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignItemObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetCampaignDestructableObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignDestructableObjectData(string campaignDestructableObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignDestructableObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignDestructableObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetCampaignDoodadObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignDoodadObjectData(string campaignDoodadObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignDoodadObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignDoodadObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetCampaignAbilityObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignAbilityObjectData(string campaignAbilityObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignAbilityObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignAbilityObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetCampaignBuffObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignBuffObjectData(string campaignBuffObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignBuffObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignBuffObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetCampaignUpgradeObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignUpgradeObjectData(string campaignUpgradeObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignUpgradeObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignUpgradeObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetCampaignAllObjectData), DynamicDataSourceType.Method)]
        public void TestParseCampaignObjectData(string campaignObjectDataFilePath)
        {
            using var original = FileProvider.GetFile(campaignObjectDataFilePath);
            using var recreated = new MemoryStream();

            CampaignObjectData.Parse(original, true).SerializeTo(recreated, true);
            StreamAssert.AreEqual(original, recreated, true, true);
        }

        private static IEnumerable<object[]> GetCampaignUnitObjectData()
        {
            return GetCampaignObjectData("Unit", CampaignUnitObjectData.FileName);
        }

        private static IEnumerable<object[]> GetCampaignItemObjectData()
        {
            return GetCampaignObjectData("Item", CampaignItemObjectData.FileName);
        }

        private static IEnumerable<object[]> GetCampaignDestructableObjectData()
        {
            return GetCampaignObjectData("Destructable", CampaignDestructableObjectData.FileName);
        }

        private static IEnumerable<object[]> GetCampaignDoodadObjectData()
        {
            return GetCampaignObjectData("Doodad", CampaignDoodadObjectData.FileName);
        }

        private static IEnumerable<object[]> GetCampaignAbilityObjectData()
        {
            return GetCampaignObjectData("Ability", CampaignAbilityObjectData.FileName);
        }

        private static IEnumerable<object[]> GetCampaignBuffObjectData()
        {
            return GetCampaignObjectData("Buff", CampaignBuffObjectData.FileName);
        }

        private static IEnumerable<object[]> GetCampaignUpgradeObjectData()
        {
            return GetCampaignObjectData("Upgrade", CampaignUpgradeObjectData.FileName);
        }

        private static IEnumerable<object[]> GetCampaignAllObjectData()
        {
            return GetCampaignObjectData("All", CampaignObjectData.FileName);
        }

        private static IEnumerable<object[]> GetCampaignObjectData(string directory, string fileName)
        {
            return TestDataProvider.GetDynamicData(
                fileName.GetSearchPattern(),
                SearchOption.AllDirectories,
                Path.Combine("Object", directory))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                fileName,
                SearchOption.TopDirectoryOnly,
                "Campaigns"));
        }
    }
}