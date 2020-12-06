// ------------------------------------------------------------------------------
// <copyright file="Map.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public sealed class Map
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="mapEnvironment"></param>
        public Map(MapInfo mapInfo, MapEnvironment mapEnvironment)
        {
            Info = mapInfo;
            Environment = mapEnvironment;
        }

        internal Map(string path)
        {
            using var mapArchive = MpqArchive.Open(path);

            using var infoStream = MpqFile.OpenRead(mapArchive, MapInfo.FileName);
            using var infoReader = new BinaryReader(infoStream);
            Info = infoReader.ReadMapInfo();

            using var environmentStream = MpqFile.OpenRead(mapArchive, MapEnvironment.FileName);
            using var environmentReader = new BinaryReader(environmentStream);
            Environment = environmentReader.ReadMapEnvironment();

            if (MpqFile.Exists(mapArchive, MapSounds.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapSounds.FileName);
                using var reader = new BinaryReader(fileStream);
                Sounds = reader.ReadMapSounds();
            }

            if (MpqFile.Exists(mapArchive, MapCameras.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapCameras.FileName);
                using var reader = new BinaryReader(fileStream);
                Cameras = reader.ReadMapCameras();
            }

            if (MpqFile.Exists(mapArchive, MapPreviewIcons.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapPreviewIcons.FileName);
                using var reader = new BinaryReader(fileStream);
                PreviewIcons = reader.ReadMapPreviewIcons();
            }

            if (MpqFile.Exists(mapArchive, MapRegions.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapRegions.FileName);
                using var reader = new BinaryReader(fileStream);
                Regions = reader.ReadMapRegions();
            }

            if (MpqFile.Exists(mapArchive, PathingMap.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, PathingMap.FileName);
                using var reader = new BinaryReader(fileStream);
                PathingMap = reader.ReadPathingMap();
            }

            if (MpqFile.Exists(mapArchive, ShadowMap.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, ShadowMap.FileName);
                using var reader = new BinaryReader(fileStream);
                ShadowMap = reader.ReadShadowMap();
            }

            if (MpqFile.Exists(mapArchive, MapAbilityObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapAbilityObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                AbilityObjectData = reader.ReadAbilityObjectData(false);
            }

            if (MpqFile.Exists(mapArchive, MapBuffObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapBuffObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                BuffObjectData = reader.ReadBuffObjectData(false);
            }

            if (MpqFile.Exists(mapArchive, MapDestructableObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapDestructableObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                DestructableObjectData = reader.ReadDestructableObjectData(false);
            }

            if (MpqFile.Exists(mapArchive, MapDoodadObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapDoodadObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                DoodadObjectData = reader.ReadDoodadObjectData(false);
            }

            if (MpqFile.Exists(mapArchive, MapItemObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapItemObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                ItemObjectData = reader.ReadItemObjectData(false);
            }

            if (MpqFile.Exists(mapArchive, MapUnitObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapUnitObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                UnitObjectData = reader.ReadUnitObjectData(false);
            }

            if (MpqFile.Exists(mapArchive, MapUpgradeObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapUpgradeObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                UpgradeObjectData = reader.ReadUpgradeObjectData(false);
            }

            if (MpqFile.Exists(mapArchive, MapCustomTextTriggers.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapCustomTextTriggers.FileName);
                using var reader = new BinaryReader(fileStream);
                CustomTextTriggers = reader.ReadMapCustomTextTriggers(Encoding.UTF8);
            }

            if (MpqFile.Exists(mapArchive, MapTriggers.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapTriggers.FileName);
                using var reader = new BinaryReader(fileStream);
                Triggers = reader.ReadMapTriggers(TriggerData.Default);
            }

            if (MpqFile.Exists(mapArchive, MapTriggerStrings.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapTriggerStrings.FileName);
                using var reader = new StreamReader(fileStream);
                TriggerStrings = reader.ReadTriggerStrings(false);
            }

            if (MpqFile.Exists(mapArchive, MapDoodads.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapDoodads.FileName);
                using var reader = new BinaryReader(fileStream);
                Doodads = reader.ReadMapDoodads();
            }

            if (MpqFile.Exists(mapArchive, MapUnits.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapUnits.FileName);
                using var reader = new BinaryReader(fileStream);
                Units = reader.ReadMapUnits();
            }
        }

        public MapSounds? Sounds { get; set; }

        public MapCameras? Cameras { get; set; }

        public MapEnvironment Environment { get; set; }

        public MapPreviewIcons? PreviewIcons { get; set; }

        public MapRegions? Regions { get; set; }

        public PathingMap? PathingMap { get; set; }

        public ShadowMap? ShadowMap { get; set; }

        public MapInfo Info { get; set; }

        public AbilityObjectData? AbilityObjectData { get; set; }

        public BuffObjectData? BuffObjectData { get; set; }

        public DestructableObjectData? DestructableObjectData { get; set; }

        public DoodadObjectData? DoodadObjectData { get; set; }

        public ItemObjectData? ItemObjectData { get; set; }

        public UnitObjectData? UnitObjectData { get; set; }

        public UpgradeObjectData? UpgradeObjectData { get; set; }

        public MapCustomTextTriggers? CustomTextTriggers { get; set; }

        public MapTriggers? Triggers { get; set; }

        public TriggerStrings? TriggerStrings { get; set; }

        public MapDoodads? Doodads { get; set; }

        public MapUnits? Units { get; set; }

        public static Map Open(string path) => new Map(path);
    }
}