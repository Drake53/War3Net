// ------------------------------------------------------------------------------
// <copyright file="Map.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private Map(string mapFolder)
        {
            using var infoStream = File.OpenRead(Path.Combine(mapFolder, MapInfo.FileName));
            using var infoReader = new BinaryReader(infoStream);
            Info = infoReader.ReadMapInfo();

            using var environmentStream = File.OpenRead(Path.Combine(mapFolder, MapEnvironment.FileName));
            using var environmentReader = new BinaryReader(environmentStream);
            Environment = environmentReader.ReadMapEnvironment();

            if (File.Exists(Path.Combine(mapFolder, MapSounds.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapSounds.FileName));
                using var reader = new BinaryReader(fileStream);
                Sounds = reader.ReadMapSounds();
            }

            if (File.Exists(Path.Combine(mapFolder, MapCameras.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapCameras.FileName));
                using var reader = new BinaryReader(fileStream);
                Cameras = reader.ReadMapCameras();
            }

            if (File.Exists(Path.Combine(mapFolder, MapPathingMap.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapPathingMap.FileName));
                using var reader = new BinaryReader(fileStream);
                PathingMap = reader.ReadMapPathingMap();
            }

            if (File.Exists(Path.Combine(mapFolder, MapPreviewIcons.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapPreviewIcons.FileName));
                using var reader = new BinaryReader(fileStream);
                PreviewIcons = reader.ReadMapPreviewIcons();
            }

            if (File.Exists(Path.Combine(mapFolder, MapRegions.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapRegions.FileName));
                using var reader = new BinaryReader(fileStream);
                Regions = reader.ReadMapRegions();
            }

            if (File.Exists(Path.Combine(mapFolder, MapShadowMap.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapShadowMap.FileName));
                using var reader = new BinaryReader(fileStream);
                ShadowMap = reader.ReadMapShadowMap();
            }

            if (File.Exists(Path.Combine(mapFolder, MapAbilityObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapAbilityObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                AbilityObjectData = reader.ReadMapAbilityObjectData();
            }

            if (File.Exists(Path.Combine(mapFolder, MapBuffObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapBuffObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                BuffObjectData = reader.ReadMapBuffObjectData();
            }

            if (File.Exists(Path.Combine(mapFolder, MapDestructableObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapDestructableObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                DestructableObjectData = reader.ReadMapDestructableObjectData();
            }

            if (File.Exists(Path.Combine(mapFolder, MapDoodadObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapDoodadObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                DoodadObjectData = reader.ReadMapDoodadObjectData();
            }

            if (File.Exists(Path.Combine(mapFolder, MapItemObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapItemObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                ItemObjectData = reader.ReadMapItemObjectData();
            }

            if (File.Exists(Path.Combine(mapFolder, MapUnitObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapUnitObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                UnitObjectData = reader.ReadMapUnitObjectData();
            }

            if (File.Exists(Path.Combine(mapFolder, MapUpgradeObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapUpgradeObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                UpgradeObjectData = reader.ReadMapUpgradeObjectData();
            }

            if (File.Exists(Path.Combine(mapFolder, MapCustomTextTriggers.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapCustomTextTriggers.FileName));
                using var reader = new BinaryReader(fileStream);
                CustomTextTriggers = reader.ReadMapCustomTextTriggers(Encoding.UTF8);
            }

            if (File.Exists(Path.Combine(mapFolder, MapTriggers.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapTriggers.FileName));
                using var reader = new BinaryReader(fileStream);
                Triggers = reader.ReadMapTriggers(TriggerData.Default);
            }

            if (File.Exists(Path.Combine(mapFolder, MapTriggerStrings.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapTriggerStrings.FileName));
                using var reader = new StreamReader(fileStream);
                TriggerStrings = reader.ReadMapTriggerStrings();
            }

            if (File.Exists(Path.Combine(mapFolder, MapDoodads.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapDoodads.FileName));
                using var reader = new BinaryReader(fileStream);
                Doodads = reader.ReadMapDoodads();
            }

            if (File.Exists(Path.Combine(mapFolder, MapUnits.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapUnits.FileName));
                using var reader = new BinaryReader(fileStream);
                Units = reader.ReadMapUnits();
            }
        }

        private Map(MpqArchive mapArchive)
        {
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

            if (MpqFile.Exists(mapArchive, MapPathingMap.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapPathingMap.FileName);
                using var reader = new BinaryReader(fileStream);
                PathingMap = reader.ReadMapPathingMap();
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

            if (MpqFile.Exists(mapArchive, MapShadowMap.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapShadowMap.FileName);
                using var reader = new BinaryReader(fileStream);
                ShadowMap = reader.ReadMapShadowMap();
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

        public MapPathingMap? PathingMap { get; set; }

        public MapPreviewIcons? PreviewIcons { get; set; }

        public MapRegions? Regions { get; set; }

        public MapShadowMap? ShadowMap { get; set; }

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

        /// <summary>
        /// Opens the map from the specified file or folder path.
        /// </summary>
        public static Map Open(string path)
        {
            if (File.Exists(path))
            {
                using var mapArchive = MpqArchive.Open(path);
                return new Map(mapArchive);
            }
            else if (Directory.Exists(path))
            {
                return new Map(path);
            }
            else
            {
                throw new ArgumentException("Could not find a file or folder at the specified path.");
            }
        }

        public static Map Open(Stream stream)
        {
            using var mapArchive = MpqArchive.Open(stream);
            return new Map(mapArchive);
        }

        public static Map Open(MpqArchive archive)
        {
            return new Map(archive);
        }
    }
}