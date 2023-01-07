// ------------------------------------------------------------------------------
// <copyright file="MapExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Import;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.Common.Providers;
using War3Net.IO.Mpq;

namespace War3Net.Build.Extensions
{
    public static class MapExtensions
    {
        private static readonly Encoding _defaultEncoding = UTF8EncodingProvider.StrictUTF8;

        public static MpqFile? GetSoundsFile(this Map map, Encoding? encoding = null)
        {
            if (map.Sounds is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.Sounds);
            writer.Flush();

            return MpqFile.New(memoryStream, MapSounds.FileName);
        }

        public static MpqFile? GetCamerasFile(this Map map, Encoding? encoding = null)
        {
            if (map.Cameras is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.Cameras);
            writer.Flush();

            return MpqFile.New(memoryStream, MapCameras.FileName);
        }

        public static MpqFile? GetEnvironmentFile(this Map map, Encoding? encoding = null)
        {
            if (map.Environment is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.Environment);
            writer.Flush();

            return MpqFile.New(memoryStream, MapEnvironment.FileName);
        }

        public static MpqFile? GetPathingMapFile(this Map map, Encoding? encoding = null)
        {
            if (map.PathingMap is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.PathingMap);
            writer.Flush();

            return MpqFile.New(memoryStream, MapPathingMap.FileName);
        }

        public static MpqFile? GetPreviewIconsFile(this Map map, Encoding? encoding = null)
        {
            if (map.PreviewIcons is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.PreviewIcons);
            writer.Flush();

            return MpqFile.New(memoryStream, MapPreviewIcons.FileName);
        }

        public static MpqFile? GetRegionsFile(this Map map, Encoding? encoding = null)
        {
            if (map.Regions is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.Regions);
            writer.Flush();

            return MpqFile.New(memoryStream, MapRegions.FileName);
        }

        public static MpqFile? GetShadowMapFile(this Map map, Encoding? encoding = null)
        {
            if (map.ShadowMap is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.ShadowMap);
            writer.Flush();

            return MpqFile.New(memoryStream, MapShadowMap.FileName);
        }

        public static MpqFile? GetImportedFilesFile(this Map map, Encoding? encoding = null)
        {
            if (map.ImportedFiles is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.ImportedFiles);
            writer.Flush();

            return MpqFile.New(memoryStream, ImportedFiles.MapFileName);
        }

        public static MpqFile? GetInfoFile(this Map map, Encoding? encoding = null)
        {
            if (map.Info is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.Info);
            writer.Flush();

            return MpqFile.New(memoryStream, MapInfo.FileName);
        }

        public static MpqFile? GetAbilityObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.AbilityObjectData is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.AbilityObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, AbilityObjectData.MapFileName);
        }

        public static MpqFile? GetBuffObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.BuffObjectData is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.BuffObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, BuffObjectData.MapFileName);
        }

        public static MpqFile? GetDestructableObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.DestructableObjectData is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.DestructableObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, DestructableObjectData.MapFileName);
        }

        public static MpqFile? GetDoodadObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.DoodadObjectData is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.DoodadObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, DoodadObjectData.MapFileName);
        }

        public static MpqFile? GetItemObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.ItemObjectData is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.ItemObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, ItemObjectData.MapFileName);
        }

        public static MpqFile? GetUnitObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.UnitObjectData is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.UnitObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, UnitObjectData.MapFileName);
        }

        public static MpqFile? GetUpgradeObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.UpgradeObjectData is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.UpgradeObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, UpgradeObjectData.MapFileName);
        }

        public static MpqFile? GetCustomTextTriggersFile(this Map map, Encoding? encoding = null)
        {
            if (map.CustomTextTriggers is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.CustomTextTriggers, encoding ?? _defaultEncoding);
            writer.Flush();

            return MpqFile.New(memoryStream, MapCustomTextTriggers.FileName);
        }

        public static MpqFile? GetScriptFile(this Map map, Encoding? encoding = null)
        {
            if (map.Info is null || string.IsNullOrEmpty(map.Script))
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, encoding ?? _defaultEncoding, leaveOpen: true);

            writer.Write(map.Script);
            writer.Flush();

            return MpqFile.New(memoryStream, map.Info.ScriptLanguage == ScriptLanguage.Lua ? LuaMapScript.FileName : JassMapScript.FileName);
        }

        public static MpqFile? GetTriggersFile(this Map map, Encoding? encoding = null)
        {
            if (map.Triggers is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.Triggers);
            writer.Flush();

            return MpqFile.New(memoryStream, MapTriggers.FileName);
        }

        public static MpqFile? GetTriggerStringsFile(this Map map, Encoding? encoding = null)
        {
            if (map.TriggerStrings is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, encoding ?? _defaultEncoding, leaveOpen: true);

            writer.WriteTriggerStrings(map.TriggerStrings);
            writer.Flush();

            return MpqFile.New(memoryStream, TriggerStrings.MapFileName);
        }

        public static MpqFile? GetDoodadsFile(this Map map, Encoding? encoding = null)
        {
            if (map.Doodads is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.Doodads);
            writer.Flush();

            return MpqFile.New(memoryStream, MapDoodads.FileName);
        }

        public static MpqFile? GetUnitsFile(this Map map, Encoding? encoding = null)
        {
            if (map.Units is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.Units);
            writer.Flush();

            return MpqFile.New(memoryStream, MapUnits.FileName);
        }

        public static void SetSoundsFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.Sounds = reader.ReadMapSounds();
        }

        public static void SetCamerasFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.Cameras = reader.ReadMapCameras();
        }

        public static void SetEnvironmentFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.Environment = reader.ReadMapEnvironment();
        }

        public static void SetPathingMapFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.PathingMap = reader.ReadMapPathingMap();
        }

        public static void SetPreviewIconsFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.PreviewIcons = reader.ReadMapPreviewIcons();
        }

        public static void SetRegionsFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.Regions = reader.ReadMapRegions();
        }

        public static void SetShadowMapFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.ShadowMap = reader.ReadMapShadowMap();
        }

        public static void SetImportedFilesFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.ImportedFiles = reader.ReadImportedFiles();
        }

        public static void SetInfoFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.Info = reader.ReadMapInfo();
        }

        public static void SetAbilityObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.AbilityObjectData = reader.ReadAbilityObjectData();
        }

        public static void SetBuffObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.BuffObjectData = reader.ReadBuffObjectData();
        }

        public static void SetDestructableObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.DestructableObjectData = reader.ReadDestructableObjectData();
        }

        public static void SetDoodadObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.DoodadObjectData = reader.ReadDoodadObjectData();
        }

        public static void SetItemObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.ItemObjectData = reader.ReadItemObjectData();
        }

        public static void SetUnitObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.UnitObjectData = reader.ReadUnitObjectData();
        }

        public static void SetUpgradeObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.UpgradeObjectData = reader.ReadUpgradeObjectData();
        }

        public static void SetCustomTextTriggersFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.CustomTextTriggers = reader.ReadMapCustomTextTriggers(encoding ?? _defaultEncoding);
        }

        public static void SetScriptFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new StreamReader(stream, encoding ?? _defaultEncoding, leaveOpen: leaveOpen);
            map.Script = reader.ReadToEnd();
        }

        public static void SetTriggersFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.Triggers = reader.ReadMapTriggers();
        }

        public static void SetTriggerStringsFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new StreamReader(stream, encoding ?? _defaultEncoding, leaveOpen: leaveOpen);
            map.TriggerStrings = reader.ReadTriggerStrings();
        }

        public static void SetDoodadsFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.Doodads = reader.ReadMapDoodads();
        }

        public static void SetUnitsFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.Units = reader.ReadMapUnits();
        }

        /// <returns>
        /// <see langword="true"/> if the file was recognized as a war3map file.
        /// Note that if the file already exists and <paramref name="overwriteFile"/> is <see langword="false"/>,
        /// this method will still return <see langword="true"/> even though nothing changed.
        /// If the file is a script file (war3map.j or war3map.lua), returns <see langword="false"/> if <see cref="Map.Info"/> is null,
        /// or if <see cref="MapInfo.ScriptLanguage"/> does not match the script file's language.
        /// </returns>
        public static bool SetFile(this Map map, string fileName, bool overwriteFile, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            switch (fileName.ToLowerInvariant())
            {
#pragma warning disable IDE0011, SA1503
                case MapSounds.FileName: if (map.Sounds is null || overwriteFile) map.SetSoundsFile(stream, encoding, leaveOpen); break;
                case MapCameras.FileName: if (map.Cameras is null || overwriteFile) map.SetCamerasFile(stream, encoding, leaveOpen); break;
                case MapEnvironment.FileName: if (map.Environment is null || overwriteFile) map.SetEnvironmentFile(stream, encoding, leaveOpen); break;
                case MapPathingMap.FileName: if (map.PathingMap is null || overwriteFile) map.SetPathingMapFile(stream, encoding, leaveOpen); break;
                case MapPreviewIcons.FileName: if (map.PreviewIcons is null || overwriteFile) map.SetPreviewIconsFile(stream, encoding, leaveOpen); break;
                case MapRegions.FileName: if (map.Regions is null || overwriteFile) map.SetRegionsFile(stream, encoding, leaveOpen); break;
                case MapShadowMap.FileName: if (map.ShadowMap is null || overwriteFile) map.SetShadowMapFile(stream, encoding, leaveOpen); break;
                case ImportedFiles.MapFileName: if (map.ImportedFiles is null || overwriteFile) map.SetImportedFilesFile(stream, encoding, leaveOpen); break;
                case MapInfo.FileName: if (map.Info is null || overwriteFile) map.SetInfoFile(stream, encoding, leaveOpen); break;
                case AbilityObjectData.MapFileName: if (map.AbilityObjectData is null || overwriteFile) map.SetAbilityObjectDataFile(stream, encoding, leaveOpen); break;
                case BuffObjectData.MapFileName: if (map.BuffObjectData is null || overwriteFile) map.SetBuffObjectDataFile(stream, encoding, leaveOpen); break;
                case DestructableObjectData.MapFileName: if (map.DestructableObjectData is null || overwriteFile) map.SetDestructableObjectDataFile(stream, encoding, leaveOpen); break;
                case DoodadObjectData.MapFileName: if (map.DoodadObjectData is null || overwriteFile) map.SetDoodadObjectDataFile(stream, encoding, leaveOpen); break;
                case ItemObjectData.MapFileName: if (map.ItemObjectData is null || overwriteFile) map.SetItemObjectDataFile(stream, encoding, leaveOpen); break;
                case UnitObjectData.MapFileName: if (map.UnitObjectData is null || overwriteFile) map.SetUnitObjectDataFile(stream, encoding, leaveOpen); break;
                case UpgradeObjectData.MapFileName: if (map.UpgradeObjectData is null || overwriteFile) map.SetUpgradeObjectDataFile(stream, encoding, leaveOpen); break;
                case MapCustomTextTriggers.FileName: if (map.CustomTextTriggers is null || overwriteFile) map.SetCustomTextTriggersFile(stream, encoding, leaveOpen); break;
                case MapTriggers.FileName: if (map.Triggers is null || overwriteFile) map.SetTriggersFile(stream, encoding, leaveOpen); break;
                case TriggerStrings.MapFileName: if (map.TriggerStrings is null || overwriteFile) map.SetTriggerStringsFile(stream, encoding, leaveOpen); break;
                case MapDoodads.FileName: if (map.Doodads is null || overwriteFile) map.SetDoodadsFile(stream, encoding, leaveOpen); break;
                case /*MapUnits.FileName*/ "war3mapunits.doo": if (map.Units is null || overwriteFile) map.SetUnitsFile(stream, encoding, leaveOpen); break;

                case JassMapScript.FileName:
                case JassMapScript.FullName:
                    if (map.Info is null || map.Info.ScriptLanguage != ScriptLanguage.Jass) return false;
                    if (map.Script is null || overwriteFile) map.SetScriptFile(stream, encoding, leaveOpen); break;

                case LuaMapScript.FileName:
                case LuaMapScript.FullName:
                    if (map.Info is null || map.Info.ScriptLanguage != ScriptLanguage.Lua) return false;
                    if (map.Script is null || overwriteFile) map.SetScriptFile(stream, encoding, leaveOpen); break;
#pragma warning restore IDE0011, SA1503

                default: return false;
            }

            return true;
        }

        public static void LocalizeInfo(this Map map)
        {
            var info = map.Info;
            var strings = map.TriggerStrings;
            if (info is null || strings is null)
            {
                return;
            }

            if (strings.TryGetValue(info.MapName, out var mapName))
            {
                info.MapName = mapName;
            }

            if (strings.TryGetValue(info.MapAuthor, out var mapAuthor))
            {
                info.MapAuthor = mapAuthor;
            }

            if (strings.TryGetValue(info.MapDescription, out var mapDescription))
            {
                info.MapDescription = mapDescription;
            }

            if (strings.TryGetValue(info.RecommendedPlayers, out var recommendedPlayers))
            {
                info.RecommendedPlayers = recommendedPlayers;
            }

            foreach (var player in info.Players)
            {
                if (strings.TryGetValue(player.Name, out var name))
                {
                    player.Name = name;
                }
            }

            foreach (var force in info.Forces)
            {
                if (strings.TryGetValue(force.Name, out var name))
                {
                    force.Name = name;
                }
            }
        }
    }
}