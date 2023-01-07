// ------------------------------------------------------------------------------
// <copyright file="JsonElementExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Numerics;
using System.Text.Json;

using War3Net.Build.Audio;
using War3Net.Build.Common;
using War3Net.Build.Configuration;
using War3Net.Build.Environment;
using War3Net.Build.Import;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.Common.Extensions;

using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace War3Net.Build.Extensions
{
    internal static class JsonElementExtensions
    {
        public static Color GetColor(this JsonElement jsonElement)
        {
            return Color.FromArgb(
                jsonElement.GetInt32(nameof(Color.A)),
                jsonElement.GetInt32(nameof(Color.R)),
                jsonElement.GetInt32(nameof(Color.G)),
                jsonElement.GetInt32(nameof(Color.B)));
        }

        public static Point GetPoint(this JsonElement jsonElement)
        {
            return new Point(
                jsonElement.GetInt32(nameof(Point.X)),
                jsonElement.GetInt32(nameof(Point.Y)));
        }

        public static Vector2 GetVector2(this JsonElement jsonElement)
        {
            return new Vector2(
                jsonElement.GetSingle(nameof(Vector2.X)),
                jsonElement.GetSingle(nameof(Vector2.Y)));
        }

        public static Vector3 GetVector3(this JsonElement jsonElement)
        {
            return new Vector3(
                jsonElement.GetSingle(nameof(Vector3.X)),
                jsonElement.GetSingle(nameof(Vector3.Y)),
                jsonElement.GetSingle(nameof(Vector3.Z)));
        }

        public static Version? GetVersion(this JsonElement jsonElement)
        {
            var versionString = jsonElement.GetString();
            return versionString is null ? null : new Version(versionString);
        }

        public static MapSounds GetMapSounds(this JsonElement jsonElement) => new MapSounds(jsonElement);

        public static Sound GetSound(this JsonElement jsonElement, MapSoundsFormatVersion formatVersion) => new Sound(jsonElement, formatVersion);

        public static Bitmask32 GetBitmask32(this JsonElement jsonElement) => new Bitmask32(jsonElement);

        public static Quadrilateral GetQuadrilateral(this JsonElement jsonElement) => new Quadrilateral(jsonElement);

        public static RandomItemSet GetRandomItemSet(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new RandomItemSet(jsonElement, formatVersion);

        public static RandomItemSet GetRandomItemSet(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new RandomItemSet(jsonElement, formatVersion, subVersion, useNewFormat);

        public static RandomItemSetItem GetRandomItemSetItem(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new RandomItemSetItem(jsonElement, formatVersion);

        public static RandomItemSetItem GetRandomItemSetItem(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new RandomItemSetItem(jsonElement, formatVersion, subVersion, useNewFormat);

        public static RectangleMargins GetRectangleMargins(this JsonElement jsonElement) => new RectangleMargins(jsonElement);

        public static GameConfiguration GetGameConfiguration(this JsonElement jsonElement) => new GameConfiguration(jsonElement);

        public static GameConfigurationPlayerInfo GetGameConfigurationPlayerInfo(this JsonElement jsonElement, GameConfigurationFormatVersion formatVersion) => new GameConfigurationPlayerInfo(jsonElement, formatVersion);

        public static MapCameras GetMapCameras(this JsonElement jsonElement) => new MapCameras(jsonElement);

        public static Camera GetCamera(this JsonElement jsonElement, MapCamerasFormatVersion formatVersion, bool useNewFormat) => new Camera(jsonElement, formatVersion, useNewFormat);

        public static MapEnvironment GetMapEnvironment(this JsonElement jsonElement) => new MapEnvironment(jsonElement);

        public static TerrainTile GetTerrainTile(this JsonElement jsonElement, MapEnvironmentFormatVersion formatVersion) => new TerrainTile(jsonElement, formatVersion);

        public static MapPathingMap GetMapPathingMap(this JsonElement jsonElement) => new MapPathingMap(jsonElement);

        public static MapPreviewIcons GetMapPreviewIcons(this JsonElement jsonElement) => new MapPreviewIcons(jsonElement);

        public static PreviewIcon GetPreviewIcon(this JsonElement jsonElement, MapPreviewIconsFormatVersion formatVersion) => new PreviewIcon(jsonElement, formatVersion);

        public static MapRegions GetMapRegions(this JsonElement jsonElement) => new MapRegions(jsonElement);

        public static Region GetRegion(this JsonElement jsonElement, MapRegionsFormatVersion formatVersion) => new Region(jsonElement, formatVersion);

        public static MapShadowMap GetMapShadowMap(this JsonElement jsonElement) => new MapShadowMap(jsonElement);

        public static ImportedFiles GetImportedFiles(this JsonElement jsonElement, bool fromCampaign) => fromCampaign ? jsonElement.GetCampaignImportedFiles() : jsonElement.GetMapImportedFiles();

        public static CampaignImportedFiles GetCampaignImportedFiles(this JsonElement jsonElement) => new CampaignImportedFiles(jsonElement);

        public static MapImportedFiles GetMapImportedFiles(this JsonElement jsonElement) => new MapImportedFiles(jsonElement);

        public static ImportedFile GetImportedFile(this JsonElement jsonElement, ImportedFilesFormatVersion formatVersion) => new ImportedFile(jsonElement, formatVersion);

        public static CampaignInfo GetCampaignInfo(this JsonElement jsonElement) => new CampaignInfo(jsonElement);

        public static CampaignMapButton GetCampaignMapButton(this JsonElement jsonElement, CampaignInfoFormatVersion formatVersion) => new CampaignMapButton(jsonElement, formatVersion);

        public static CampaignMap GetCampaignMap(this JsonElement jsonElement, CampaignInfoFormatVersion formatVersion) => new CampaignMap(jsonElement, formatVersion);

        public static MapInfo GetMapInfo(this JsonElement jsonElement) => new MapInfo(jsonElement);

        public static PlayerData GetPlayerData(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new PlayerData(jsonElement, formatVersion);

        public static ForceData GetForceData(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new ForceData(jsonElement, formatVersion);

        public static UpgradeData GetUpgradeData(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new UpgradeData(jsonElement, formatVersion);

        public static TechData GetTechData(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new TechData(jsonElement, formatVersion);

        public static RandomUnitTable GetRandomUnitTable(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new RandomUnitTable(jsonElement, formatVersion);

        public static RandomItemTable GetRandomItemTable(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new RandomItemTable(jsonElement, formatVersion);

        public static RandomUnitSet GetRandomUnitSet(this JsonElement jsonElement, MapInfoFormatVersion formatVersion, int setSize) => new RandomUnitSet(jsonElement, formatVersion, setSize);

        public static ObjectData GetObjectData(this JsonElement jsonElement, bool fromCampaign) => new ObjectData(jsonElement, fromCampaign);

        public static AbilityObjectData GetAbilityObjectData(this JsonElement jsonElement, bool fromCampaign) => fromCampaign ? jsonElement.GetCampaignAbilityObjectData() : jsonElement.GetMapAbilityObjectData();

        public static BuffObjectData GetBuffObjectData(this JsonElement jsonElement, bool fromCampaign) => fromCampaign ? jsonElement.GetCampaignBuffObjectData() : jsonElement.GetMapBuffObjectData();

        public static DestructableObjectData GetDestructableObjectData(this JsonElement jsonElement, bool fromCampaign) => fromCampaign ? jsonElement.GetCampaignDestructableObjectData() : jsonElement.GetMapDestructableObjectData();

        public static DoodadObjectData GetDoodadObjectData(this JsonElement jsonElement, bool fromCampaign) => fromCampaign ? jsonElement.GetCampaignDoodadObjectData() : jsonElement.GetMapDoodadObjectData();

        public static ItemObjectData GetItemObjectData(this JsonElement jsonElement, bool fromCampaign) => fromCampaign ? jsonElement.GetCampaignItemObjectData() : jsonElement.GetMapItemObjectData();

        public static UnitObjectData GetUnitObjectData(this JsonElement jsonElement, bool fromCampaign) => fromCampaign ? jsonElement.GetCampaignUnitObjectData() : jsonElement.GetMapUnitObjectData();

        public static UpgradeObjectData GetUpgradeObjectData(this JsonElement jsonElement, bool fromCampaign) => fromCampaign ? jsonElement.GetCampaignUpgradeObjectData() : jsonElement.GetMapUpgradeObjectData();

        public static CampaignAbilityObjectData GetCampaignAbilityObjectData(this JsonElement jsonElement) => new CampaignAbilityObjectData(jsonElement);

        public static CampaignBuffObjectData GetCampaignBuffObjectData(this JsonElement jsonElement) => new CampaignBuffObjectData(jsonElement);

        public static CampaignDestructableObjectData GetCampaignDestructableObjectData(this JsonElement jsonElement) => new CampaignDestructableObjectData(jsonElement);

        public static CampaignDoodadObjectData GetCampaignDoodadObjectData(this JsonElement jsonElement) => new CampaignDoodadObjectData(jsonElement);

        public static CampaignItemObjectData GetCampaignItemObjectData(this JsonElement jsonElement) => new CampaignItemObjectData(jsonElement);

        public static CampaignUnitObjectData GetCampaignUnitObjectData(this JsonElement jsonElement) => new CampaignUnitObjectData(jsonElement);

        public static CampaignUpgradeObjectData GetCampaignUpgradeObjectData(this JsonElement jsonElement) => new CampaignUpgradeObjectData(jsonElement);

        public static MapAbilityObjectData GetMapAbilityObjectData(this JsonElement jsonElement) => new MapAbilityObjectData(jsonElement);

        public static MapBuffObjectData GetMapBuffObjectData(this JsonElement jsonElement) => new MapBuffObjectData(jsonElement);

        public static MapDestructableObjectData GetMapDestructableObjectData(this JsonElement jsonElement) => new MapDestructableObjectData(jsonElement);

        public static MapDoodadObjectData GetMapDoodadObjectData(this JsonElement jsonElement) => new MapDoodadObjectData(jsonElement);

        public static MapItemObjectData GetMapItemObjectData(this JsonElement jsonElement) => new MapItemObjectData(jsonElement);

        public static MapUnitObjectData GetMapUnitObjectData(this JsonElement jsonElement) => new MapUnitObjectData(jsonElement);

        public static MapUpgradeObjectData GetMapUpgradeObjectData(this JsonElement jsonElement) => new MapUpgradeObjectData(jsonElement);

        public static LevelObjectModification GetLevelObjectModification(this JsonElement jsonElement, ObjectDataFormatVersion formatVersion) => new LevelObjectModification(jsonElement, formatVersion);

        public static LevelObjectDataModification GetLevelObjectDataModification(this JsonElement jsonElement, ObjectDataFormatVersion formatVersion) => new LevelObjectDataModification(jsonElement, formatVersion);

        public static SimpleObjectModification GetSimpleObjectModification(this JsonElement jsonElement, ObjectDataFormatVersion formatVersion) => new SimpleObjectModification(jsonElement, formatVersion);

        public static SimpleObjectDataModification GetSimpleObjectDataModification(this JsonElement jsonElement, ObjectDataFormatVersion formatVersion) => new SimpleObjectDataModification(jsonElement, formatVersion);

        public static VariationObjectModification GetVariationObjectModification(this JsonElement jsonElement, ObjectDataFormatVersion formatVersion) => new VariationObjectModification(jsonElement, formatVersion);

        public static VariationObjectDataModification GetVariationObjectDataModification(this JsonElement jsonElement, ObjectDataFormatVersion formatVersion) => new VariationObjectDataModification(jsonElement, formatVersion);

        public static MapCustomTextTriggers GetMapCustomTextTriggers(this JsonElement jsonElement) => new MapCustomTextTriggers(jsonElement);

        public static CustomTextTrigger GetCustomTextTrigger(this JsonElement jsonElement, MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion) => new CustomTextTrigger(jsonElement, formatVersion, subVersion);

        public static MapTriggers GetMapTriggers(this JsonElement jsonElement) => new MapTriggers(jsonElement);

        public static DeletedTriggerItem GetDeletedTriggerItem(this JsonElement jsonElement, TriggerItemType triggerItemType, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => new DeletedTriggerItem(jsonElement, triggerItemType, formatVersion, subVersion);

        public static TriggerCategoryDefinition GetTriggerCategoryDefinition(this JsonElement jsonElement, TriggerItemType triggerItemType, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => new TriggerCategoryDefinition(jsonElement, triggerItemType, formatVersion, subVersion);

        public static TriggerDefinition GetTriggerDefinition(this JsonElement jsonElement, TriggerItemType triggerItemType, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => new TriggerDefinition(jsonElement, triggerItemType, formatVersion, subVersion);

        public static TriggerVariableDefinition GetTriggerVariableDefinition(this JsonElement jsonElement, TriggerItemType triggerItemType, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => new TriggerVariableDefinition(jsonElement, triggerItemType, formatVersion, subVersion);

        public static VariableDefinition GetVariableDefinition(this JsonElement jsonElement, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => new VariableDefinition(jsonElement, formatVersion, subVersion);

        public static TriggerFunction GetTriggerFunction(this JsonElement jsonElement, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion, bool isChildFunction) => new TriggerFunction(jsonElement, formatVersion, subVersion, isChildFunction);

        public static TriggerFunctionParameter GetTriggerFunctionParameter(this JsonElement jsonElement, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => new TriggerFunctionParameter(jsonElement, formatVersion, subVersion);

        public static MapDoodads GetMapDoodads(this JsonElement jsonElement) => new MapDoodads(jsonElement);

        public static DoodadData GetMapDoodadData(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new DoodadData(jsonElement, formatVersion, subVersion, useNewFormat);

        public static SpecialDoodadData GetMapSpecialDoodadData(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion) => new SpecialDoodadData(jsonElement, formatVersion, subVersion, specialDoodadVersion);

        public static MapUnits GetMapUnits(this JsonElement jsonElement) => new MapUnits(jsonElement);

        public static UnitData GetMapUnitData(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new UnitData(jsonElement, formatVersion, subVersion, useNewFormat);

        public static InventoryItemData GetInventoryItemData(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new InventoryItemData(jsonElement, formatVersion, subVersion, useNewFormat);

        public static ModifiedAbilityData GetModifiedAbilityData(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new ModifiedAbilityData(jsonElement, formatVersion, subVersion, useNewFormat);

        public static RandomUnitAny GetRandomUnitNeutral(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new RandomUnitAny(jsonElement, formatVersion, subVersion, useNewFormat);

        public static RandomUnitGlobalTable GetRandomUnitGlobalTable(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new RandomUnitGlobalTable(jsonElement, formatVersion, subVersion, useNewFormat);

        public static RandomUnitCustomTable GetRandomUnitCustomTable(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new RandomUnitCustomTable(jsonElement, formatVersion, subVersion, useNewFormat);

        public static RandomUnitTableUnit GetRandomUnitTableUnit(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new RandomUnitTableUnit(jsonElement, formatVersion, subVersion, useNewFormat);

        public static Color GetColor(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetColor();

        public static Point GetPoint(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetPoint();

        public static Vector2 GetVector2(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetVector2();

        public static Vector3 GetVector3(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetVector3();

        public static Version? GetVersion(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetVersion();

        public static Bitmask32 GetBitmask32(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetBitmask32();

        public static Quadrilateral GetQuadrilateral(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetQuadrilateral();

        public static RectangleMargins GetRectangleMargins(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetRectangleMargins();
    }
}