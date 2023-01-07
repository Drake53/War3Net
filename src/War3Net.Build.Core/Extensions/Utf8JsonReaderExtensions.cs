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

        public static ImportedFiles ReadImportedFiles(this ref Utf8JsonReader reader) => new ImportedFiles(ref reader);

        public static CampaignInfo ReadCampaignInfo(this ref Utf8JsonReader reader) => new CampaignInfo(ref reader);

        public static MapInfo ReadMapInfo(this ref Utf8JsonReader reader) => new MapInfo(ref reader);

        public static AbilityObjectData ReadAbilityObjectData(this ref Utf8JsonReader reader) => new AbilityObjectData(ref reader);

        public static BuffObjectData ReadBuffObjectData(this ref Utf8JsonReader reader) => new BuffObjectData(ref reader);

        public static DestructableObjectData ReadDestructableObjectData(this ref Utf8JsonReader reader) => new DestructableObjectData(ref reader);

        public static DoodadObjectData ReadDoodadObjectData(this ref Utf8JsonReader reader) => new DoodadObjectData(ref reader);

        public static ItemObjectData ReadItemObjectData(this ref Utf8JsonReader reader) => new ItemObjectData(ref reader);

        public static UnitObjectData ReadUnitObjectData(this ref Utf8JsonReader reader) => new UnitObjectData(ref reader);

        public static UpgradeObjectData ReadUpgradeObjectData(this ref Utf8JsonReader reader) => new UpgradeObjectData(ref reader);

        public static ObjectData ReadObjectData(this ref Utf8JsonReader reader) => new ObjectData(ref reader);

        public static MapCustomTextTriggers ReadMapCustomTextTriggers(this ref Utf8JsonReader reader) => new MapCustomTextTriggers(ref reader);

        public static MapTriggers ReadMapTriggers(this ref Utf8JsonReader reader) => new MapTriggers(ref reader);

        public static MapDoodads ReadMapDoodads(this ref Utf8JsonReader reader) => new MapDoodads(ref reader);

        public static MapUnits ReadMapUnits(this ref Utf8JsonReader reader) => new MapUnits(ref reader);
    }
}