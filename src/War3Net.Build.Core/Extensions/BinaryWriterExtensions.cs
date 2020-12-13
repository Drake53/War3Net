// ------------------------------------------------------------------------------
// <copyright file="BinaryWriterExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1600

using System.IO;
using System.Text;

using War3Net.Build.Audio;
using War3Net.Build.Common;
using War3Net.Build.Environment;
using War3Net.Build.Import;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;

namespace War3Net.Build.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, MapSounds mapSounds) => mapSounds.WriteTo(writer);

        public static void Write(this BinaryWriter writer, Sound sound, MapSoundsFormatVersion formatVersion) => sound.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, Bitmask32 bitmask) => bitmask.WriteTo(writer);

        public static void Write(this BinaryWriter writer, Quadrilateral quadrilateral) => quadrilateral.WriteTo(writer);

        public static void Write(this BinaryWriter writer, RandomItemSet randomItemSet, MapInfoFormatVersion formatVersion) => randomItemSet.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, RandomItemSet randomItemSet, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomItemSet.WriteTo(writer, formatVersion, subVersion, useNewFormat);

        public static void Write(this BinaryWriter writer, RandomItemSetItem randomItemSetItem, MapInfoFormatVersion formatVersion) => randomItemSetItem.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, RandomItemSetItem randomItemSetItem, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomItemSetItem.WriteTo(writer, formatVersion, subVersion, useNewFormat);

        public static void Write(this BinaryWriter writer, RectangleMargins rectangleMargins) => rectangleMargins.WriteTo(writer);

        public static void Write(this BinaryWriter writer, MapCameras mapCameras) => mapCameras.WriteTo(writer);

        public static void Write(this BinaryWriter writer, Camera camera, MapCamerasFormatVersion formatVersion, bool useNewFormat) => camera.WriteTo(writer, formatVersion, useNewFormat);

        public static void Write(this BinaryWriter writer, MapEnvironment mapEnvironment) => mapEnvironment.WriteTo(writer);

        public static void Write(this BinaryWriter writer, TerrainTile terrainTile, MapEnvironmentFormatVersion formatVersion) => terrainTile.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, MapPreviewIcons mapPreviewIcons) => mapPreviewIcons.WriteTo(writer);

        public static void Write(this BinaryWriter writer, PreviewIcon previewIcon, MapPreviewIconsFormatVersion formatVersion) => previewIcon.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, MapRegions mapRegions) => mapRegions.WriteTo(writer);

        public static void Write(this BinaryWriter writer, Region region, MapRegionsFormatVersion formatVersion) => region.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, MapPathingMap mapPathingMap) => mapPathingMap.WriteTo(writer);

        public static void Write(this BinaryWriter writer, MapShadowMap mapShadowMap) => mapShadowMap.WriteTo(writer);

        public static void Write(this BinaryWriter writer, CampaignInfo campaignInfo) => campaignInfo.WriteTo(writer);

        public static void Write(this BinaryWriter writer, CampaignMapButton campaignMapButton, CampaignInfoFormatVersion formatVersion) => campaignMapButton.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, CampaignMap campaignMap, CampaignInfoFormatVersion formatVersion) => campaignMap.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, MapInfo mapInfo) => mapInfo.WriteTo(writer);

        public static void Write(this BinaryWriter writer, PlayerData playerData, MapInfoFormatVersion formatVersion) => playerData.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, ForceData forceData, MapInfoFormatVersion formatVersion) => forceData.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, UpgradeData upgradeData, MapInfoFormatVersion formatVersion) => upgradeData.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, TechData techData, MapInfoFormatVersion formatVersion) => techData.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, RandomUnitTable randomUnitTable, MapInfoFormatVersion formatVersion) => randomUnitTable.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, RandomItemTable randomItemTable, MapInfoFormatVersion formatVersion) => randomItemTable.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, RandomUnitSet randomUnitSet, MapInfoFormatVersion formatVersion) => randomUnitSet.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, ObjectData objectData) => objectData.WriteTo(writer);

        public static void Write(this BinaryWriter writer, AbilityObjectData abilityObjectData) => abilityObjectData.WriteTo(writer);

        public static void Write(this BinaryWriter writer, BuffObjectData buffObjectData) => buffObjectData.WriteTo(writer);

        public static void Write(this BinaryWriter writer, DestructableObjectData destructableObjectData) => destructableObjectData.WriteTo(writer);

        public static void Write(this BinaryWriter writer, DoodadObjectData doodadObjectData) => doodadObjectData.WriteTo(writer);

        public static void Write(this BinaryWriter writer, ItemObjectData itemObjectData) => itemObjectData.WriteTo(writer);

        public static void Write(this BinaryWriter writer, UnitObjectData unitObjectData) => unitObjectData.WriteTo(writer);

        public static void Write(this BinaryWriter writer, UpgradeObjectData upgradeObjectData) => upgradeObjectData.WriteTo(writer);

        public static void Write(this BinaryWriter writer, LevelObjectModification levelObjectModification, ObjectDataFormatVersion formatVersion) => levelObjectModification.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, LevelObjectDataModification levelObjectDataModification, ObjectDataFormatVersion formatVersion) => levelObjectDataModification.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, SimpleObjectModification simpleObjectModification, ObjectDataFormatVersion formatVersion) => simpleObjectModification.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, SimpleObjectDataModification simpleObjectDataModification, ObjectDataFormatVersion formatVersion) => simpleObjectDataModification.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, VariationObjectModification variationObjectModification, ObjectDataFormatVersion formatVersion) => variationObjectModification.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, VariationObjectDataModification variationObjectDataModification, ObjectDataFormatVersion formatVersion) => variationObjectDataModification.WriteTo(writer, formatVersion);

        public static void Write(this BinaryWriter writer, MapCustomTextTriggers mapCustomTextTriggers, Encoding encoding) => mapCustomTextTriggers.WriteTo(writer, encoding);

        public static void Write(this BinaryWriter writer, CustomTextTrigger customTextTrigger, Encoding encoding, MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion) => customTextTrigger.WriteTo(writer, encoding, formatVersion, subVersion);

        public static void Write(this BinaryWriter writer, MapTriggers mapTriggers) => mapTriggers.WriteTo(writer);

        public static void Write(this BinaryWriter writer, TriggerItem triggerItem, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => triggerItem.WriteTo(writer, formatVersion, subVersion);

        public static void Write(this BinaryWriter writer, VariableDefinition variableDefinition, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => variableDefinition.WriteTo(writer, formatVersion, subVersion);

        public static void Write(this BinaryWriter writer, TriggerFunction triggerFunction, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => triggerFunction.WriteTo(writer, formatVersion, subVersion);

        public static void Write(this BinaryWriter writer, TriggerFunctionParameter triggerFunctionParameter, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => triggerFunctionParameter.WriteTo(writer, formatVersion, subVersion);

        public static void Write(this BinaryWriter writer, MapDoodads mapDoodads) => mapDoodads.WriteTo(writer);

        public static void Write(this BinaryWriter writer, DoodadData mapDoodadData, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => mapDoodadData.WriteTo(writer, formatVersion, subVersion, useNewFormat);

        public static void Write(this BinaryWriter writer, SpecialDoodadData mapSpecialDoodadData, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion) => mapSpecialDoodadData.WriteTo(writer, formatVersion, subVersion, specialDoodadVersion);

        public static void Write(this BinaryWriter writer, MapUnits mapUnits) => mapUnits.WriteTo(writer);

        public static void Write(this BinaryWriter writer, UnitData mapUnitData, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => mapUnitData.WriteTo(writer, formatVersion, subVersion, useNewFormat);

        public static void Write(this BinaryWriter writer, InventoryItemData inventoryItemData, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => inventoryItemData.WriteTo(writer, formatVersion, subVersion, useNewFormat);

        public static void Write(this BinaryWriter writer, ModifiedAbilityData modifiedAbilityData, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => modifiedAbilityData.WriteTo(writer, formatVersion, subVersion, useNewFormat);

        public static void Write(this BinaryWriter writer, RandomUnitData randomUnitData, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomUnitData.WriteTo(writer, formatVersion, subVersion, useNewFormat);

        public static void Write(this BinaryWriter writer, RandomUnitTableUnit randomUnitTableUnit, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomUnitTableUnit.WriteTo(writer, formatVersion, subVersion, useNewFormat);

        public static void Write(this BinaryWriter writer, ImportedFiles importedFiles) => importedFiles.WriteTo(writer);

        public static void Write(this BinaryWriter writer, ImportedFile importedFile, ImportedFilesFormatVersion formatVersion) => importedFile.WriteTo(writer, formatVersion);
    }
}