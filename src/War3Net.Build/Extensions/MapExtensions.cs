// ------------------------------------------------------------------------------
// <copyright file="MapExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CSharpLua;

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Transpilers;
using War3Net.IO.Mpq;

namespace War3Net.Build.Extensions
{
    public static class MapExtensions
    {
        private static readonly Encoding _defaultEncoding = new UTF8Encoding(false, true);

        public static MpqFile? GetSoundsFile(this Map map, Encoding? encoding = null)
        {
            if (map.Sounds is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
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

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.Cameras);
            writer.Flush();

            return MpqFile.New(memoryStream, MapCameras.FileName);
        }

        public static MpqFile GetEnvironmentFile(this Map map, Encoding? encoding = null)
        {
            using var memoryStream = new MemoryStream();
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

            using var memoryStream = new MemoryStream();
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

            using var memoryStream = new MemoryStream();
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

            using var memoryStream = new MemoryStream();
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

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.ShadowMap);
            writer.Flush();

            return MpqFile.New(memoryStream, MapShadowMap.FileName);
        }

        public static MpqFile GetInfoFile(this Map map, Encoding? encoding = null)
        {
            using var memoryStream = new MemoryStream();
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

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.AbilityObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, MapAbilityObjectData.FileName);
        }

        public static MpqFile? GetBuffObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.BuffObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.BuffObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, MapBuffObjectData.FileName);
        }

        public static MpqFile? GetDestructableObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.DestructableObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.DestructableObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, MapDestructableObjectData.FileName);
        }

        public static MpqFile? GetDoodadObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.DoodadObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.DoodadObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, MapDoodadObjectData.FileName);
        }

        public static MpqFile? GetItemObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.ItemObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.ItemObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, MapItemObjectData.FileName);
        }

        public static MpqFile? GetUnitObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.UnitObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.UnitObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, MapUnitObjectData.FileName);
        }

        public static MpqFile? GetUpgradeObjectDataFile(this Map map, Encoding? encoding = null)
        {
            if (map.UpgradeObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.UpgradeObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, MapUpgradeObjectData.FileName);
        }

        public static MpqFile? GetCustomTextTriggersFile(this Map map, Encoding? encoding = null)
        {
            if (map.CustomTextTriggers is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(map.CustomTextTriggers, encoding ?? _defaultEncoding);
            writer.Flush();

            return MpqFile.New(memoryStream, MapCustomTextTriggers.FileName);
        }

        public static MpqFile GetScriptFile(this Map map, Encoding? encoding = null)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, encoding ?? _defaultEncoding, leaveOpen: true);

            writer.Write(map.Script);
            writer.Flush();

            return MpqFile.New(memoryStream, map.Info.ScriptLanguage == ScriptLanguage.Lua ? "war3map.lua" : "war3map.j");
        }

        public static MpqFile? GetTriggersFile(this Map map, Encoding? encoding = null)
        {
            if (map.Triggers is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
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

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, encoding ?? _defaultEncoding, leaveOpen: true);

            writer.WriteTriggerStrings(map.TriggerStrings);
            writer.Flush();

            return MpqFile.New(memoryStream, MapTriggerStrings.FileName);
        }

        public static MpqFile? GetDoodadsFile(this Map map, Encoding? encoding = null)
        {
            if (map.Doodads is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
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

            using var memoryStream = new MemoryStream();
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

        public static void SetInfoFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.Info = reader.ReadMapInfo();
        }

        public static void SetAbilityObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.AbilityObjectData = reader.ReadMapAbilityObjectData();
        }

        public static void SetBuffObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.BuffObjectData = reader.ReadMapBuffObjectData();
        }

        public static void SetDestructableObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.DestructableObjectData = reader.ReadMapDestructableObjectData();
        }

        public static void SetDoodadObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.DoodadObjectData = reader.ReadMapDoodadObjectData();
        }

        public static void SetItemObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.ItemObjectData = reader.ReadMapItemObjectData();
        }

        public static void SetUnitObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.UnitObjectData = reader.ReadMapUnitObjectData();
        }

        public static void SetUpgradeObjectDataFile(this Map map, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            map.UpgradeObjectData = reader.ReadMapUpgradeObjectData();
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
            map.TriggerStrings = reader.ReadMapTriggerStrings();
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

        /// <returns><see langword="true"/> if the file was recognized as a war3map file.</returns>
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
                case MapInfo.FileName: if (map.Info is null || overwriteFile) map.SetInfoFile(stream, encoding, leaveOpen); break;
                case MapAbilityObjectData.FileName: if (map.AbilityObjectData is null || overwriteFile) map.SetAbilityObjectDataFile(stream, encoding, leaveOpen); break;
                case MapBuffObjectData.FileName: if (map.BuffObjectData is null || overwriteFile) map.SetBuffObjectDataFile(stream, encoding, leaveOpen); break;
                case MapDestructableObjectData.FileName: if (map.DestructableObjectData is null || overwriteFile) map.SetDestructableObjectDataFile(stream, encoding, leaveOpen); break;
                case MapDoodadObjectData.FileName: if (map.DoodadObjectData is null || overwriteFile) map.SetDoodadObjectDataFile(stream, encoding, leaveOpen); break;
                case MapItemObjectData.FileName: if (map.ItemObjectData is null || overwriteFile) map.SetItemObjectDataFile(stream, encoding, leaveOpen); break;
                case MapUnitObjectData.FileName: if (map.UnitObjectData is null || overwriteFile) map.SetUnitObjectDataFile(stream, encoding, leaveOpen); break;
                case MapUpgradeObjectData.FileName: if (map.UpgradeObjectData is null || overwriteFile) map.SetUpgradeObjectDataFile(stream, encoding, leaveOpen); break;
                case MapCustomTextTriggers.FileName: if (map.CustomTextTriggers is null || overwriteFile) map.SetCustomTextTriggersFile(stream, encoding, leaveOpen); break;
                case MapTriggers.FileName: if (map.Triggers is null || overwriteFile) map.SetTriggersFile(stream, encoding, leaveOpen); break;
                case MapTriggerStrings.FileName: if (map.TriggerStrings is null || overwriteFile) map.SetTriggerStringsFile(stream, encoding, leaveOpen); break;
                case MapDoodads.FileName: if (map.Doodads is null || overwriteFile) map.SetDoodadsFile(stream, encoding, leaveOpen); break;
                case MapUnits.FileName: if (map.Units is null || overwriteFile) map.SetUnitsFile(stream, encoding, leaveOpen); break;

                case @"war3map.j":
                case @"scripts\war3map.j":
                    if (map.Info.ScriptLanguage == ScriptLanguage.Jass && overwriteFile) map.SetScriptFile(stream, encoding, leaveOpen); break;

                case @"war3map.lua":
                case @"scripts\war3map.lua":
                    if (map.Info.ScriptLanguage == ScriptLanguage.Lua && overwriteFile) map.SetScriptFile(stream, encoding, leaveOpen); break;
#pragma warning restore IDE0011, SA1503

                default: return false;
            }

            return true;
        }

        public static void CompileScript(this Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info.ScriptLanguage != ScriptLanguage.Jass)
            {
                throw new InvalidOperationException($"The map's script language must be set to jass in order to use the jass compiler.");
            }

            using var stream = new MemoryStream();

            var mapScriptBuilder = new MapScriptBuilder();
            mapScriptBuilder.SetDefaultOptionsForMap(map);

            var compilationUnit = mapScriptBuilder.Build(map);

            using (var writer = new StreamWriter(stream, _defaultEncoding, leaveOpen: true))
            {
                var renderer = new JassRenderer(writer);
                renderer.Render(compilationUnit);
            }

            stream.Position = 0;
            map.SetScriptFile(stream);
        }

        public static CompileResult CompileScript(this Map map, Compiler compiler, IEnumerable<string> luaSystemLibs)
        {
            var jassHelperFolder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Warcraft III", "JassHelper");
            return map.CompileScript(compiler, luaSystemLibs, Path.Combine(jassHelperFolder, "common.j"), Path.Combine(jassHelperFolder, "Blizzard.j"));
        }

        public static CompileResult CompileScript(this Map map, Compiler compiler, IEnumerable<string> luaSystemLibs, string commonJPath, string blizzardJPath)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (compiler is null)
            {
                throw new ArgumentNullException(nameof(compiler));
            }

            if (map.Info.ScriptLanguage != ScriptLanguage.Lua)
            {
                throw new InvalidOperationException($"The map's script language must be set to lua in order to use the C# compiler.");
            }

            using var stream = new MemoryStream();

            try
            {
                compiler.CompileSingleFile(stream, luaSystemLibs);
            }
            catch (CompilationErrorException e)
            {
                return new CompileResult(e.EmitResult);
            }

            var transpiler = new JassToLuaTranspiler();
            transpiler.IgnoreComments = true;
            transpiler.IgnoreEmptyDeclarations = true;
            transpiler.IgnoreEmptyStatements = true;
            transpiler.KeepFunctionsSeparated = true;

            transpiler.RegisterJassFile(JassSyntaxFactory.ParseCompilationUnit(File.ReadAllText(commonJPath)));
            transpiler.RegisterJassFile(JassSyntaxFactory.ParseCompilationUnit(File.ReadAllText(blizzardJPath)));

            var mapScriptBuilder = new MapScriptBuilder();
            mapScriptBuilder.SetDefaultOptionsForCSharpLua();

            var luaCompilationUnit = transpiler.Transpile(mapScriptBuilder.Build(map));
            using (var writer = new StreamWriter(stream, _defaultEncoding, leaveOpen: true))
            {
                var luaRenderOptions = new LuaSyntaxGenerator.SettingInfo
                {
                    Indent = 4,
                };

                var luaRenderer = new LuaRenderer(luaRenderOptions, writer);
                luaRenderer.RenderCompilationUnit(luaCompilationUnit);
            }

            stream.Position = 0;
            map.SetScriptFile(stream);

            return new CompileResult(true, null);
        }
    }
}