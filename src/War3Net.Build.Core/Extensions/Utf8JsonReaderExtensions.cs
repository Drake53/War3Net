// ------------------------------------------------------------------------------
// <copyright file="Utf8JsonReaderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Import;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;

namespace War3Net.Build.Extensions
{
    internal static class Utf8JsonReaderExtensions
    {
        public static MapSounds ReadMapSounds(this ref Utf8JsonReader reader) => new MapSounds(ref reader);

        public static MapCameras ReadMapCameras(this ref Utf8JsonReader reader) => new MapCameras(ref reader);

        public static MapEnvironment ReadMapEnvironment(this ref Utf8JsonReader reader) => new MapEnvironment(ref reader);

        public static MapPathingMap ReadMapPathingMap(this ref Utf8JsonReader reader) => new MapPathingMap(ref reader);

        public static MapPreviewIcons ReadMapPreviewIcons(this ref Utf8JsonReader reader) => new MapPreviewIcons(ref reader);

        public static MapRegions ReadMapRegions(this ref Utf8JsonReader reader) => new MapRegions(ref reader);

        public static MapShadowMap ReadMapShadowMap(this ref Utf8JsonReader reader) => new MapShadowMap(ref reader);

        public static CampaignImportedFiles ReadCampaignImportedFiles(this ref Utf8JsonReader reader) => new CampaignImportedFiles(ref reader);

        public static MapImportedFiles ReadMapImportedFiles(this ref Utf8JsonReader reader) => new MapImportedFiles(ref reader);

        public static CampaignInfo ReadCampaignInfo(this ref Utf8JsonReader reader) => new CampaignInfo(ref reader);

        public static MapInfo ReadMapInfo(this ref Utf8JsonReader reader) => new MapInfo(ref reader);

        public static CampaignAbilityObjectData ReadCampaignAbilityObjectData(this ref Utf8JsonReader reader) => new CampaignAbilityObjectData(ref reader);

        public static CampaignBuffObjectData ReadCampaignBuffObjectData(this ref Utf8JsonReader reader) => new CampaignBuffObjectData(ref reader);

        public static CampaignDestructableObjectData ReadCampaignDestructableObjectData(this ref Utf8JsonReader reader) => new CampaignDestructableObjectData(ref reader);

        public static CampaignDoodadObjectData ReadCampaignDoodadObjectData(this ref Utf8JsonReader reader) => new CampaignDoodadObjectData(ref reader);

        public static CampaignItemObjectData ReadCampaignItemObjectData(this ref Utf8JsonReader reader) => new CampaignItemObjectData(ref reader);

        public static CampaignUnitObjectData ReadCampaignUnitObjectData(this ref Utf8JsonReader reader) => new CampaignUnitObjectData(ref reader);

        public static CampaignUpgradeObjectData ReadCampaignUpgradeObjectData(this ref Utf8JsonReader reader) => new CampaignUpgradeObjectData(ref reader);

        public static MapAbilityObjectData ReadMapAbilityObjectData(this ref Utf8JsonReader reader) => new MapAbilityObjectData(ref reader);

        public static MapBuffObjectData ReadMapBuffObjectData(this ref Utf8JsonReader reader) => new MapBuffObjectData(ref reader);

        public static MapDestructableObjectData ReadMapDestructableObjectData(this ref Utf8JsonReader reader) => new MapDestructableObjectData(ref reader);

        public static MapDoodadObjectData ReadMapDoodadObjectData(this ref Utf8JsonReader reader) => new MapDoodadObjectData(ref reader);

        public static MapItemObjectData ReadMapItemObjectData(this ref Utf8JsonReader reader) => new MapItemObjectData(ref reader);

        public static MapUnitObjectData ReadMapUnitObjectData(this ref Utf8JsonReader reader) => new MapUnitObjectData(ref reader);

        public static MapUpgradeObjectData ReadMapUpgradeObjectData(this ref Utf8JsonReader reader) => new MapUpgradeObjectData(ref reader);

        public static ObjectData ReadObjectData(this ref Utf8JsonReader reader, bool fromCampaign) => new ObjectData(ref reader, fromCampaign);

        public static MapCustomTextTriggers ReadMapCustomTextTriggers(this ref Utf8JsonReader reader) => new MapCustomTextTriggers(ref reader);

        public static MapTriggers ReadMapTriggers(this ref Utf8JsonReader reader) => new MapTriggers(ref reader);

        public static MapDoodads ReadMapDoodads(this ref Utf8JsonReader reader) => new MapDoodads(ref reader);

        public static MapUnits ReadMapUnits(this ref Utf8JsonReader reader) => new MapUnits(ref reader);
    }
}