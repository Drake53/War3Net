// ------------------------------------------------------------------------------
// <copyright file="TestDataFileParams.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Concurrent;

using War3Net.Build.Audio;
using War3Net.Build.Configuration;
using War3Net.Build.Environment;
using War3Net.Build.Import;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;

namespace War3Net.Build.Core.Tests
{
    public class TestDataFileParams
    {
        private static readonly ConcurrentDictionary<TestDataFileType, TestDataFileParams> _cache = new();

        private TestDataFileParams(
            string fileExtension,
            string folderName,
            params string[] knownFileNames)
        {
            SearchPattern = $"*{fileExtension}";
            FolderName = folderName;
            KnownFileNames = knownFileNames;
        }

        public static TestDataFileParams Get(TestDataFileType testDataFileType)
        {
            return _cache.GetOrAdd(testDataFileType, Create);
        }

        public string SearchPattern { get; }

        public string FolderName { get; }

        public string[] KnownFileNames { get; }

        private static TestDataFileParams Create(TestDataFileType testDataFileType)
        {
            return testDataFileType switch
            {
                TestDataFileType.MapSounds => new TestDataFileParams(MapSounds.FileExtension, "Audio", MapSounds.FileName),
                TestDataFileType.GameConfiguration => new TestDataFileParams(GameConfiguration.FileExtension, "Configuration"),
                TestDataFileType.MapCameras => new TestDataFileParams(MapCameras.FileExtension, "Camera", MapCameras.FileName),
                TestDataFileType.MapEnvironment => new TestDataFileParams(MapEnvironment.FileExtension, "Environment", MapEnvironment.FileName),
                TestDataFileType.MapPathingMap => new TestDataFileParams(MapPathingMap.FileExtension, "Pathing", MapPathingMap.FileName),
                TestDataFileType.MapPreviewIcons => new TestDataFileParams(MapPreviewIcons.FileExtension, "Icons", MapPreviewIcons.FileName),
                TestDataFileType.MapRegions => new TestDataFileParams(MapRegions.FileExtension, "Region", MapRegions.FileName),
                TestDataFileType.MapShadowMap => new TestDataFileParams(MapShadowMap.FileExtension, "Shadow", MapShadowMap.FileName),
                TestDataFileType.ImportedFiles => new TestDataFileParams(ImportedFiles.FileExtension, "Import", ImportedFiles.CampaignFileName, ImportedFiles.MapFileName),
                TestDataFileType.CampaignInfo => new TestDataFileParams(CampaignInfo.FileExtension, "Info/Campaign", CampaignInfo.FileName),
                TestDataFileType.MapInfo => new TestDataFileParams(MapInfo.FileExtension, "Info", MapInfo.FileName),
                TestDataFileType.AbilityObjectData => new TestDataFileParams(AbilityObjectData.FileExtension, "Object/Ability", AbilityObjectData.CampaignFileName, AbilityObjectData.CampaignSkinFileName, AbilityObjectData.MapFileName, AbilityObjectData.MapSkinFileName),
                TestDataFileType.BuffObjectData => new TestDataFileParams(BuffObjectData.FileExtension, "Object/Buff", BuffObjectData.CampaignFileName, BuffObjectData.CampaignSkinFileName, BuffObjectData.MapFileName, BuffObjectData.MapSkinFileName),
                TestDataFileType.DestructableObjectData => new TestDataFileParams(DestructableObjectData.FileExtension, "Object/Destructable", DestructableObjectData.CampaignFileName, DestructableObjectData.CampaignSkinFileName, DestructableObjectData.MapFileName, DestructableObjectData.MapSkinFileName),
                TestDataFileType.DoodadObjectData => new TestDataFileParams(DoodadObjectData.FileExtension, "Object/Doodad", DoodadObjectData.CampaignFileName, DoodadObjectData.CampaignSkinFileName, DoodadObjectData.MapFileName, DoodadObjectData.MapSkinFileName),
                TestDataFileType.ItemObjectData => new TestDataFileParams(ItemObjectData.FileExtension, "Object/Item", ItemObjectData.CampaignFileName, ItemObjectData.CampaignSkinFileName, ItemObjectData.MapFileName, ItemObjectData.MapSkinFileName),
                TestDataFileType.UnitObjectData => new TestDataFileParams(UnitObjectData.FileExtension, "Object/Unit", UnitObjectData.CampaignFileName, UnitObjectData.CampaignSkinFileName, UnitObjectData.MapFileName, UnitObjectData.MapSkinFileName),
                TestDataFileType.UpgradeObjectData => new TestDataFileParams(UpgradeObjectData.FileExtension, "Object/Upgrade", UpgradeObjectData.CampaignFileName, UpgradeObjectData.CampaignSkinFileName, UpgradeObjectData.MapFileName, UpgradeObjectData.MapSkinFileName),
                TestDataFileType.ObjectData => new TestDataFileParams(ObjectData.FileExtension, "Object/All"),
                TestDataFileType.MapCustomTextTriggers => new TestDataFileParams(MapCustomTextTriggers.FileExtension, "Triggers", MapCustomTextTriggers.FileName),
                TestDataFileType.MapTriggers => new TestDataFileParams(MapTriggers.FileExtension, "Triggers", MapTriggers.FileName),
                TestDataFileType.TriggerStrings => new TestDataFileParams(TriggerStrings.FileExtension, "Triggers", TriggerStrings.CampaignFileName, TriggerStrings.MapFileName),
                TestDataFileType.MapDoodads => new TestDataFileParams(MapDoodads.FileExtension, "Widget/Doodads", MapDoodads.FileName),
                TestDataFileType.MapUnits => new TestDataFileParams(MapUnits.FileExtension, "Widget/Units", MapUnits.FileName),
            };
        }
    }
}