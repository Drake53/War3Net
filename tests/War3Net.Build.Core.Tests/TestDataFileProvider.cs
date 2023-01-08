// ------------------------------------------------------------------------------
// <copyright file="TestDataFileProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using War3Net.Build.Audio;
using War3Net.Build.Configuration;
using War3Net.Build.Environment;
using War3Net.Build.Import;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests
{
    public static class TestDataFileProvider
    {
        public static IEnumerable<object[]> GetMapSoundsFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapSounds.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Audio"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapSounds.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetGameConfigurationFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{GameConfiguration.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Configuration"));
        }

        public static IEnumerable<object[]> GetMapCamerasFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapCameras.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Camera"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapCameras.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetMapEnvironmentFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapEnvironment.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Environment"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapEnvironment.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetMapPathingMapFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapPathingMap.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Pathing"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapPathingMap.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetMapPreviewIconsFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapPreviewIcons.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Icons"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapPreviewIcons.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetMapRegionsFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapRegions.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Region"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapRegions.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetMapShadowMapFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapShadowMap.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Shadow"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapShadowMap.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetImportedFilesFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{ImportedFiles.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Import"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ImportedFiles.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ImportedFiles.MapFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetCampaignInfoFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{CampaignInfo.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Info", "Campaign"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                CampaignInfo.FileName,
                SearchOption.AllDirectories,
                "Campaigns"));
        }

        public static IEnumerable<object[]> GetMapInfoFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapInfo.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Info"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapInfo.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetAbilityObjectDataFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{AbilityObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Ability"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                AbilityObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                AbilityObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                AbilityObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                AbilityObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetBuffObjectDataFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{BuffObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Buff"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                BuffObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                BuffObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                BuffObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                BuffObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetDestructableObjectDataFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{DestructableObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Destructable"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DestructableObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DestructableObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DestructableObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DestructableObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetDoodadObjectDataFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{DoodadObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Doodad"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DoodadObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DoodadObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DoodadObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                DoodadObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetItemObjectDataFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{ItemObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Item"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ItemObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ItemObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ItemObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                ItemObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetUnitObjectDataFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{UnitObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Unit"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UnitObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UnitObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UnitObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UnitObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetUpgradeObjectDataFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{UpgradeObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "Upgrade"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UpgradeObjectData.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UpgradeObjectData.CampaignSkinFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UpgradeObjectData.MapFileName,
                SearchOption.AllDirectories,
                "Maps"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                UpgradeObjectData.MapSkinFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetObjectDataFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{ObjectData.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Object", "All"));
        }

        public static IEnumerable<object[]> GetMapCustomTextTriggersFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapCustomTextTriggers.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Triggers"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapCustomTextTriggers.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetMapTriggersFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapTriggers.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Triggers"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapTriggers.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetTriggerStringsFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{TriggerStrings.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Triggers"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                TriggerStrings.CampaignFileName,
                SearchOption.AllDirectories,
                "Campaigns"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                TriggerStrings.MapFileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetMapDoodadsFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapDoodads.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Widget", "Doodads"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapDoodads.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }

        public static IEnumerable<object[]> GetMapUnitsFilePaths()
        {
            return TestDataProvider.GetDynamicData(
                $"*{MapUnits.FileExtension}",
                SearchOption.AllDirectories,
                Path.Combine("Widget", "Units"))

            .Concat(TestDataProvider.GetDynamicArchiveData(
                MapUnits.FileName,
                SearchOption.AllDirectories,
                "Maps"));
        }
    }
}