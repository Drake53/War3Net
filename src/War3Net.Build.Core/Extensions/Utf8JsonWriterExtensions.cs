// ------------------------------------------------------------------------------
// <copyright file="Utf8JsonWriterExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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

using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace War3Net.Build.Extensions
{
    internal static class Utf8JsonWriterExtensions
    {
        public static void WriteObject<TValue>(this Utf8JsonWriter writer, string propertyName, TValue value, JsonSerializerOptions? options = null)
        {
            writer.WritePropertyName(propertyName);
            JsonSerializer.Serialize(writer, value, options);
        }

        public static void Write(this Utf8JsonWriter writer, Color color)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Color.R), color.R);
            writer.WriteNumber(nameof(Color.G), color.G);
            writer.WriteNumber(nameof(Color.B), color.B);
            writer.WriteNumber(nameof(Color.A), color.A);

            writer.WriteEndObject();
        }

        public static void Write(this Utf8JsonWriter writer, string propertyName, Color color)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(color);
        }

        public static void Write(this Utf8JsonWriter writer, Point point)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Point.X), point.X);
            writer.WriteNumber(nameof(Point.Y), point.Y);

            writer.WriteEndObject();
        }

        public static void Write(this Utf8JsonWriter writer, string propertyName, Point point)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(point);
        }

        public static void Write(this Utf8JsonWriter writer, Vector2 vector)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Vector2.X), vector.X);
            writer.WriteNumber(nameof(Vector2.Y), vector.Y);

            writer.WriteEndObject();
        }

        public static void Write(this Utf8JsonWriter writer, string propertyName, Vector2 vector)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(vector);
        }

        public static void Write(this Utf8JsonWriter writer, Vector3 vector)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Vector3.X), vector.X);
            writer.WriteNumber(nameof(Vector3.Y), vector.Y);
            writer.WriteNumber(nameof(Vector3.Z), vector.Z);

            writer.WriteEndObject();
        }

        public static void Write(this Utf8JsonWriter writer, string propertyName, Vector3 vector)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(vector);
        }

        public static void Write(this Utf8JsonWriter writer, MapSounds mapSounds, JsonSerializerOptions options) => mapSounds.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, Sound sound, JsonSerializerOptions options, MapSoundsFormatVersion formatVersion) => sound.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, Bitmask32 bitmask, JsonSerializerOptions options) => bitmask.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, string propertyName, Bitmask32 bitmask, JsonSerializerOptions options)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(bitmask, options);
        }

        public static void Write(this Utf8JsonWriter writer, Quadrilateral quadrilateral, JsonSerializerOptions options) => quadrilateral.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, string propertyName, Quadrilateral quadrilateral, JsonSerializerOptions options)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(quadrilateral, options);
        }

        public static void Write(this Utf8JsonWriter writer, RandomItemSet randomItemSet, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => randomItemSet.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomItemSet randomItemSet, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomItemSet.WriteTo(writer, options, formatVersion, subVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, RandomItemSetItem randomItemSetItem, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => randomItemSetItem.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomItemSetItem randomItemSetItem, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomItemSetItem.WriteTo(writer, options, formatVersion, subVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, RectangleMargins rectangleMargins, JsonSerializerOptions options) => rectangleMargins.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, string propertyName, RectangleMargins? rectangleMargins, JsonSerializerOptions options)
        {
            writer.WritePropertyName(propertyName);

            if (rectangleMargins is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.Write(rectangleMargins, options);
            }
        }

        public static void Write(this Utf8JsonWriter writer, GameConfiguration gameConfiguration, JsonSerializerOptions options) => gameConfiguration.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, GameConfigurationPlayerInfo gameConfigurationPlayerInfo, JsonSerializerOptions options, GameConfigurationFormatVersion formatVersion) => gameConfigurationPlayerInfo.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, MapCameras mapCameras, JsonSerializerOptions options) => mapCameras.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, Camera camera, JsonSerializerOptions options, MapCamerasFormatVersion formatVersion, bool useNewFormat) => camera.WriteTo(writer, options, formatVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, MapEnvironment mapEnvironment, JsonSerializerOptions options) => mapEnvironment.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, TerrainTile terrainTile, JsonSerializerOptions options, MapEnvironmentFormatVersion formatVersion) => terrainTile.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, MapPathingMap mapPathingMap, JsonSerializerOptions options) => mapPathingMap.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, MapPreviewIcons mapPreviewIcons, JsonSerializerOptions options) => mapPreviewIcons.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, PreviewIcon previewIcon, JsonSerializerOptions options, MapPreviewIconsFormatVersion formatVersion) => previewIcon.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, MapRegions mapRegions, JsonSerializerOptions options) => mapRegions.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, Region region, JsonSerializerOptions options, MapRegionsFormatVersion formatVersion) => region.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, MapShadowMap mapShadowMap, JsonSerializerOptions options) => mapShadowMap.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, ImportedFiles importedFiles, JsonSerializerOptions options) => importedFiles.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, ImportedFile importedFile, JsonSerializerOptions options, ImportedFilesFormatVersion formatVersion) => importedFile.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, CampaignInfo campaignInfo, JsonSerializerOptions options) => campaignInfo.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, CampaignMapButton campaignMapButton, JsonSerializerOptions options, CampaignInfoFormatVersion formatVersion) => campaignMapButton.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, CampaignMap campaignMap, JsonSerializerOptions options, CampaignInfoFormatVersion formatVersion) => campaignMap.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, MapInfo mapInfo, JsonSerializerOptions options) => mapInfo.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, PlayerData playerData, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => playerData.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, ForceData forceData, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => forceData.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, UpgradeData upgradeData, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => upgradeData.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, TechData techData, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => techData.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomUnitTable randomUnitTable, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => randomUnitTable.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomItemTable randomItemTable, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => randomItemTable.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, RandomUnitSet randomUnitSet, JsonSerializerOptions options, MapInfoFormatVersion formatVersion) => randomUnitSet.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, ObjectData objectData, JsonSerializerOptions options) => objectData.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, AbilityObjectData abilityObjectData, JsonSerializerOptions options) => abilityObjectData.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, BuffObjectData buffObjectData, JsonSerializerOptions options) => buffObjectData.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, DestructableObjectData destructableObjectData, JsonSerializerOptions options) => destructableObjectData.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, DoodadObjectData doodadObjectData, JsonSerializerOptions options) => doodadObjectData.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, ItemObjectData itemObjectData, JsonSerializerOptions options) => itemObjectData.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, UnitObjectData unitObjectData, JsonSerializerOptions options) => unitObjectData.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, UpgradeObjectData upgradeObjectData, JsonSerializerOptions options) => upgradeObjectData.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, LevelObjectModification levelObjectModification, JsonSerializerOptions options, ObjectDataFormatVersion formatVersion) => levelObjectModification.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, LevelObjectDataModification levelObjectDataModification, JsonSerializerOptions options, ObjectDataFormatVersion formatVersion) => levelObjectDataModification.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, SimpleObjectModification simpleObjectModification, JsonSerializerOptions options, ObjectDataFormatVersion formatVersion) => simpleObjectModification.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, SimpleObjectDataModification simpleObjectDataModification, JsonSerializerOptions options, ObjectDataFormatVersion formatVersion) => simpleObjectDataModification.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, VariationObjectModification variationObjectModification, JsonSerializerOptions options, ObjectDataFormatVersion formatVersion) => variationObjectModification.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, VariationObjectDataModification variationObjectDataModification, JsonSerializerOptions options, ObjectDataFormatVersion formatVersion) => variationObjectDataModification.WriteTo(writer, options, formatVersion);

        public static void Write(this Utf8JsonWriter writer, MapCustomTextTriggers mapCustomTextTriggers, JsonSerializerOptions options) => mapCustomTextTriggers.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, CustomTextTrigger customTextTrigger, JsonSerializerOptions options, MapCustomTextTriggersFormatVersion formatVersion, MapCustomTextTriggersSubVersion? subVersion) => customTextTrigger.WriteTo(writer, options, formatVersion, subVersion);

        public static void Write(this Utf8JsonWriter writer, MapTriggers mapTriggers, JsonSerializerOptions options) => mapTriggers.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, TriggerItem triggerItem, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => triggerItem.WriteTo(writer, options, formatVersion, subVersion);

        public static void Write(this Utf8JsonWriter writer, VariableDefinition variableDefinition, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => variableDefinition.WriteTo(writer, options, formatVersion, subVersion);

        public static void Write(this Utf8JsonWriter writer, TriggerFunction triggerFunction, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => triggerFunction.WriteTo(writer, options, formatVersion, subVersion);

        public static void Write(this Utf8JsonWriter writer, TriggerFunctionParameter triggerFunctionParameter, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion) => triggerFunctionParameter.WriteTo(writer, options, formatVersion, subVersion);

        public static void Write(this Utf8JsonWriter writer, MapDoodads mapDoodads, JsonSerializerOptions options) => mapDoodads.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, DoodadData mapDoodadData, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => mapDoodadData.WriteTo(writer, options, formatVersion, subVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, SpecialDoodadData mapSpecialDoodadData, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, SpecialDoodadVersion specialDoodadVersion) => mapSpecialDoodadData.WriteTo(writer, options, formatVersion, subVersion, specialDoodadVersion);

        public static void Write(this Utf8JsonWriter writer, MapUnits mapUnits, JsonSerializerOptions options) => mapUnits.WriteTo(writer, options);

        public static void Write(this Utf8JsonWriter writer, UnitData mapUnitData, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => mapUnitData.WriteTo(writer, options, formatVersion, subVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, InventoryItemData inventoryItemData, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => inventoryItemData.WriteTo(writer, options, formatVersion, subVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, ModifiedAbilityData modifiedAbilityData, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => modifiedAbilityData.WriteTo(writer, options, formatVersion, subVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, RandomUnitData randomUnitData, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomUnitData.WriteTo(writer, options, formatVersion, subVersion, useNewFormat);

        public static void Write(this Utf8JsonWriter writer, RandomUnitTableUnit randomUnitTableUnit, JsonSerializerOptions options, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => randomUnitTableUnit.WriteTo(writer, options, formatVersion, subVersion, useNewFormat);
    }
}