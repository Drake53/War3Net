// ------------------------------------------------------------------------------
// <copyright file="Map.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.Build.Import;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public sealed class Map
    {
        private static readonly Encoding _defaultEncoding = Encoding.UTF8;

        private readonly string? _mapName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="mapEnvironment"></param>
        [Obsolete]
        public Map(MapInfo? mapInfo, MapEnvironment? mapEnvironment)
        {
            Info = mapInfo;
            Environment = mapEnvironment;
        }

        private Map(string? mapName, string mapFolder, MapFiles mapFiles)
        {
            _mapName = mapName;

            if (mapFiles.HasFlag(MapFiles.Info) && File.Exists(Path.Combine(mapFolder, MapInfo.FileName)))
            {
                using var infoStream = File.OpenRead(Path.Combine(mapFolder, MapInfo.FileName));
                using var infoReader = new BinaryReader(infoStream);
                Info = infoReader.ReadMapInfo();

                if (mapFiles.HasFlag(MapFiles.Script))
                {
                    var scriptFileName = Info.ScriptLanguage switch
                    {
                        ScriptLanguage.Jass => JassMapScript.FileName,
                        ScriptLanguage.Lua => LuaMapScript.FileName,

                        _ => throw new InvalidEnumArgumentException(nameof(Info.ScriptLanguage), (int)Info.ScriptLanguage, typeof(ScriptLanguage)),
                    };

                    if (File.Exists(Path.Combine(mapFolder, scriptFileName)))
                    {
                        Script = File.ReadAllText(Path.Combine(mapFolder, scriptFileName));
                    }
                    else if (File.Exists(Path.Combine(mapFolder, "scripts", scriptFileName)))
                    {
                        Script = File.ReadAllText(Path.Combine(mapFolder, "scripts", scriptFileName));
                    }
                }
            }

            if (mapFiles.HasFlag(MapFiles.Sounds) && File.Exists(Path.Combine(mapFolder, MapSounds.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapSounds.FileName));
                using var reader = new BinaryReader(fileStream);
                Sounds = reader.ReadMapSounds();
            }

            if (mapFiles.HasFlag(MapFiles.Cameras) && File.Exists(Path.Combine(mapFolder, MapCameras.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapCameras.FileName));
                using var reader = new BinaryReader(fileStream);
                Cameras = reader.ReadMapCameras();
            }

            if (mapFiles.HasFlag(MapFiles.Environment) && File.Exists(Path.Combine(mapFolder, MapEnvironment.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapEnvironment.FileName));
                using var reader = new BinaryReader(fileStream);
                Environment = reader.ReadMapEnvironment();
            }

            if (mapFiles.HasFlag(MapFiles.PathingMap) && File.Exists(Path.Combine(mapFolder, MapPathingMap.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapPathingMap.FileName));
                using var reader = new BinaryReader(fileStream);
                PathingMap = reader.ReadMapPathingMap();
            }

            if (mapFiles.HasFlag(MapFiles.PreviewIcons) && File.Exists(Path.Combine(mapFolder, MapPreviewIcons.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapPreviewIcons.FileName));
                using var reader = new BinaryReader(fileStream);
                PreviewIcons = reader.ReadMapPreviewIcons();
            }

            if (mapFiles.HasFlag(MapFiles.Regions) && File.Exists(Path.Combine(mapFolder, MapRegions.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapRegions.FileName));
                using var reader = new BinaryReader(fileStream);
                Regions = reader.ReadMapRegions();
            }

            if (mapFiles.HasFlag(MapFiles.ShadowMap) && File.Exists(Path.Combine(mapFolder, MapShadowMap.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapShadowMap.FileName));
                using var reader = new BinaryReader(fileStream);
                ShadowMap = reader.ReadMapShadowMap();
            }

            if (mapFiles.HasFlag(MapFiles.ImportedFiles) && File.Exists(Path.Combine(mapFolder, MapImportedFiles.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapImportedFiles.FileName));
                using var reader = new BinaryReader(fileStream);
                ImportedFiles = reader.ReadMapImportedFiles();
            }

            if (mapFiles.HasFlag(MapFiles.AbilityObjectData) && File.Exists(Path.Combine(mapFolder, MapAbilityObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapAbilityObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                AbilityObjectData = reader.ReadMapAbilityObjectData();
            }

            if (mapFiles.HasFlag(MapFiles.BuffObjectData) && File.Exists(Path.Combine(mapFolder, MapBuffObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapBuffObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                BuffObjectData = reader.ReadMapBuffObjectData();
            }

            if (mapFiles.HasFlag(MapFiles.DestructableObjectData) && File.Exists(Path.Combine(mapFolder, MapDestructableObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapDestructableObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                DestructableObjectData = reader.ReadMapDestructableObjectData();
            }

            if (mapFiles.HasFlag(MapFiles.DoodadObjectData) && File.Exists(Path.Combine(mapFolder, MapDoodadObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapDoodadObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                DoodadObjectData = reader.ReadMapDoodadObjectData();
            }

            if (mapFiles.HasFlag(MapFiles.ItemObjectData) && File.Exists(Path.Combine(mapFolder, MapItemObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapItemObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                ItemObjectData = reader.ReadMapItemObjectData();
            }

            if (mapFiles.HasFlag(MapFiles.UnitObjectData) && File.Exists(Path.Combine(mapFolder, MapUnitObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapUnitObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                UnitObjectData = reader.ReadMapUnitObjectData();
            }

            if (mapFiles.HasFlag(MapFiles.UpgradeObjectData) && File.Exists(Path.Combine(mapFolder, MapUpgradeObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapUpgradeObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                UpgradeObjectData = reader.ReadMapUpgradeObjectData();
            }

            if (mapFiles.HasFlag(MapFiles.CustomTextTriggers) && File.Exists(Path.Combine(mapFolder, MapCustomTextTriggers.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapCustomTextTriggers.FileName));
                using var reader = new BinaryReader(fileStream);
                CustomTextTriggers = reader.ReadMapCustomTextTriggers(_defaultEncoding);
            }

            if (mapFiles.HasFlag(MapFiles.Triggers) && File.Exists(Path.Combine(mapFolder, MapTriggers.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapTriggers.FileName));
                using var reader = new BinaryReader(fileStream);
                Triggers = reader.ReadMapTriggers(TriggerData.Default);
            }

            if (mapFiles.HasFlag(MapFiles.TriggerStrings) && File.Exists(Path.Combine(mapFolder, MapTriggerStrings.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapTriggerStrings.FileName));
                using var reader = new StreamReader(fileStream);
                TriggerStrings = reader.ReadMapTriggerStrings();
            }

            if (mapFiles.HasFlag(MapFiles.Doodads) && File.Exists(Path.Combine(mapFolder, MapDoodads.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapDoodads.FileName));
                using var reader = new BinaryReader(fileStream);
                Doodads = reader.ReadMapDoodads();
            }

            if (mapFiles.HasFlag(MapFiles.Units) && File.Exists(Path.Combine(mapFolder, MapUnits.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(mapFolder, MapUnits.FileName));
                using var reader = new BinaryReader(fileStream);
                Units = reader.ReadMapUnits();
            }
        }

        private Map(string? mapName, MpqArchive mapArchive, MapFiles mapFiles)
        {
            _mapName = mapName;

            if (mapFiles.HasFlag(MapFiles.Info) && MpqFile.Exists(mapArchive, MapInfo.FileName))
            {
                using var infoStream = MpqFile.OpenRead(mapArchive, MapInfo.FileName);
                using var infoReader = new BinaryReader(infoStream);
                Info = infoReader.ReadMapInfo();

                if (mapFiles.HasFlag(MapFiles.Script))
                {
                    string scriptFileName, scriptFullName;
                    switch (Info.ScriptLanguage)
                    {
                        case ScriptLanguage.Jass:
                            scriptFileName = JassMapScript.FileName;
                            scriptFullName = JassMapScript.FullName;
                            break;

                        case ScriptLanguage.Lua:
                            scriptFileName = LuaMapScript.FileName;
                            scriptFullName = LuaMapScript.FullName;
                            break;

                        default: throw new InvalidEnumArgumentException(nameof(Info.ScriptLanguage), (int)Info.ScriptLanguage, typeof(ScriptLanguage));
                    }

                    if (MpqFile.Exists(mapArchive, scriptFileName))
                    {
                        using var fileStream = MpqFile.OpenRead(mapArchive, scriptFileName);
                        using var reader = new StreamReader(fileStream);
                        Script = reader.ReadToEnd();
                    }
                    else if (MpqFile.Exists(mapArchive, scriptFullName))
                    {
                        using var fileStream = MpqFile.OpenRead(mapArchive, scriptFullName);
                        using var reader = new StreamReader(fileStream);
                        Script = reader.ReadToEnd();
                    }
                }
            }

            if (mapFiles.HasFlag(MapFiles.Sounds) && MpqFile.Exists(mapArchive, MapSounds.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapSounds.FileName);
                using var reader = new BinaryReader(fileStream);
                Sounds = reader.ReadMapSounds();
            }

            if (mapFiles.HasFlag(MapFiles.Cameras) && MpqFile.Exists(mapArchive, MapCameras.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapCameras.FileName);
                using var reader = new BinaryReader(fileStream);
                Cameras = reader.ReadMapCameras();
            }

            if (mapFiles.HasFlag(MapFiles.Environment) && MpqFile.Exists(mapArchive, MapEnvironment.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapEnvironment.FileName);
                using var reader = new BinaryReader(fileStream);
                Environment = reader.ReadMapEnvironment();
            }

            if (mapFiles.HasFlag(MapFiles.PathingMap) && MpqFile.Exists(mapArchive, MapPathingMap.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapPathingMap.FileName);
                using var reader = new BinaryReader(fileStream);
                PathingMap = reader.ReadMapPathingMap();
            }

            if (mapFiles.HasFlag(MapFiles.PreviewIcons) && MpqFile.Exists(mapArchive, MapPreviewIcons.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapPreviewIcons.FileName);
                using var reader = new BinaryReader(fileStream);
                PreviewIcons = reader.ReadMapPreviewIcons();
            }

            if (mapFiles.HasFlag(MapFiles.Regions) && MpqFile.Exists(mapArchive, MapRegions.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapRegions.FileName);
                using var reader = new BinaryReader(fileStream);
                Regions = reader.ReadMapRegions();
            }

            if (mapFiles.HasFlag(MapFiles.ShadowMap) && MpqFile.Exists(mapArchive, MapShadowMap.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapShadowMap.FileName);
                using var reader = new BinaryReader(fileStream);
                ShadowMap = reader.ReadMapShadowMap();
            }

            if (mapFiles.HasFlag(MapFiles.ImportedFiles) && MpqFile.Exists(mapArchive, MapImportedFiles.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapImportedFiles.FileName);
                using var reader = new BinaryReader(fileStream);
                ImportedFiles = reader.ReadMapImportedFiles();
            }

            if (mapFiles.HasFlag(MapFiles.AbilityObjectData) && MpqFile.Exists(mapArchive, MapAbilityObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapAbilityObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                AbilityObjectData = reader.ReadAbilityObjectData(false);
            }

            if (mapFiles.HasFlag(MapFiles.BuffObjectData) && MpqFile.Exists(mapArchive, MapBuffObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapBuffObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                BuffObjectData = reader.ReadBuffObjectData(false);
            }

            if (mapFiles.HasFlag(MapFiles.DestructableObjectData) && MpqFile.Exists(mapArchive, MapDestructableObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapDestructableObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                DestructableObjectData = reader.ReadDestructableObjectData(false);
            }

            if (mapFiles.HasFlag(MapFiles.DoodadObjectData) && MpqFile.Exists(mapArchive, MapDoodadObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapDoodadObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                DoodadObjectData = reader.ReadDoodadObjectData(false);
            }

            if (mapFiles.HasFlag(MapFiles.ItemObjectData) && MpqFile.Exists(mapArchive, MapItemObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapItemObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                ItemObjectData = reader.ReadItemObjectData(false);
            }

            if (mapFiles.HasFlag(MapFiles.UnitObjectData) && MpqFile.Exists(mapArchive, MapUnitObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapUnitObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                UnitObjectData = reader.ReadUnitObjectData(false);
            }

            if (mapFiles.HasFlag(MapFiles.UpgradeObjectData) && MpqFile.Exists(mapArchive, MapUpgradeObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapUpgradeObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                UpgradeObjectData = reader.ReadUpgradeObjectData(false);
            }

            if (mapFiles.HasFlag(MapFiles.CustomTextTriggers) && MpqFile.Exists(mapArchive, MapCustomTextTriggers.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapCustomTextTriggers.FileName);
                using var reader = new BinaryReader(fileStream);
                CustomTextTriggers = reader.ReadMapCustomTextTriggers(_defaultEncoding);
            }

            if (mapFiles.HasFlag(MapFiles.Triggers) && MpqFile.Exists(mapArchive, MapTriggers.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapTriggers.FileName);
                using var reader = new BinaryReader(fileStream);
                Triggers = reader.ReadMapTriggers(TriggerData.Default);
            }

            if (mapFiles.HasFlag(MapFiles.TriggerStrings) && MpqFile.Exists(mapArchive, MapTriggerStrings.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapTriggerStrings.FileName);
                using var reader = new StreamReader(fileStream);
                TriggerStrings = reader.ReadTriggerStrings(false);
            }

            if (mapFiles.HasFlag(MapFiles.Doodads) && MpqFile.Exists(mapArchive, MapDoodads.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapDoodads.FileName);
                using var reader = new BinaryReader(fileStream);
                Doodads = reader.ReadMapDoodads();
            }

            if (mapFiles.HasFlag(MapFiles.Units) && MpqFile.Exists(mapArchive, MapUnits.FileName))
            {
                using var fileStream = MpqFile.OpenRead(mapArchive, MapUnits.FileName);
                using var reader = new BinaryReader(fileStream);
                Units = reader.ReadMapUnits();
            }
        }

        public MapSounds? Sounds { get; set; }

        public MapCameras? Cameras { get; set; }

        public MapEnvironment? Environment { get; set; }

        public MapPathingMap? PathingMap { get; set; }

        public MapPreviewIcons? PreviewIcons { get; set; }

        public MapRegions? Regions { get; set; }

        public MapShadowMap? ShadowMap { get; set; }

        public MapImportedFiles? ImportedFiles { get; set; }

        public MapInfo? Info { get; set; }

        public AbilityObjectData? AbilityObjectData { get; set; }

        public BuffObjectData? BuffObjectData { get; set; }

        public DestructableObjectData? DestructableObjectData { get; set; }

        public DoodadObjectData? DoodadObjectData { get; set; }

        public ItemObjectData? ItemObjectData { get; set; }

        public UnitObjectData? UnitObjectData { get; set; }

        public UpgradeObjectData? UpgradeObjectData { get; set; }

        public MapCustomTextTriggers? CustomTextTriggers { get; set; }

        public string? Script { get; set; }

        public MapTriggers? Triggers { get; set; }

        public TriggerStrings? TriggerStrings { get; set; }

        public MapDoodads? Doodads { get; set; }

        public MapUnits? Units { get; set; }

        /// <summary>
        /// Opens the map from the specified file or folder path.
        /// </summary>
        public static Map Open(string path, MapFiles mapFiles = MapFiles.All)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                using var mapArchive = MpqArchive.Open(path);
                return new Map(fileInfo.Name, mapArchive, mapFiles);
            }
            else
            {
                var directoryInfo = new DirectoryInfo(path);
                if (directoryInfo.Exists)
                {
                    return new Map(directoryInfo.Name, path, mapFiles);
                }
                else
                {
                    throw new ArgumentException("Could not find a file or folder at the specified path.", nameof(path));
                }
            }
        }

        public static Map Open(Stream stream, MapFiles mapFiles = MapFiles.All)
        {
            using var mapArchive = MpqArchive.Open(stream);
            return new Map(null, mapArchive, mapFiles);
        }

        public static Map Open(MpqArchive archive, MapFiles mapFiles = MapFiles.All)
        {
            return new Map(null, archive, mapFiles);
        }

        public static bool TryOpen(string path, [NotNullWhen(true)] out Map? map, MapFiles mapFiles = MapFiles.All)
        {
            try
            {
                map = Open(path, mapFiles);
                return true;
            }
            catch
            {
                map = null;
                return false;
            }
        }

        public static bool TryOpen(Stream stream, [NotNullWhen(true)] out Map? map, MapFiles mapFiles = MapFiles.All)
        {
            try
            {
                map = Open(stream, mapFiles);
                return true;
            }
            catch
            {
                map = null;
                return false;
            }
        }

        public static bool TryOpen(MpqArchive archive, [NotNullWhen(true)] out Map? map, MapFiles mapFiles = MapFiles.All)
        {
            try
            {
                map = Open(archive, mapFiles);
                return true;
            }
            catch
            {
                map = null;
                return false;
            }
        }

        public override string? ToString()
        {
            if (!string.IsNullOrEmpty(_mapName))
            {
                return _mapName;
            }

            var mapName = Info?.MapName.Localize(TriggerStrings);
            if (!string.IsNullOrEmpty(mapName))
            {
                return mapName;
            }

            return base.ToString();
        }
    }
}