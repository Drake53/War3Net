// ------------------------------------------------------------------------------
// <copyright file="ScriptCompilerOptions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Audio;
using War3Net.Build.Common;
using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    [Obsolete]
    public sealed class ScriptCompilerOptions
    {
        public ScriptCompilerOptions(params string[] libraries)
        {
            FileFlags = new Dictionary<string, MpqFileFlags>();
            DefaultFileFlags = MpqFileFlags.Exists;
            Libraries = new List<string>(libraries);

            SourceDirectory = string.Empty;
            OutputDirectory = string.Empty;
        }

        public ScriptCompilerOptions(IEnumerable<string> libraries)
            : this(libraries.ToArray())
        {
        }

        public string SourceDirectory { get; set; }

        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the path to clijasshelper.exe.
        /// This property must be set when compiling vJass source code.
        /// </summary>
        public string? JasshelperCliPath { get; set; }

        public string? CommonJPath { get; set; }

        public string? BlizzardJPath { get; set; }

        /// <summary>
        /// If <see langword="true"/>, script files added through assets will be overwritten, even if <see cref="SourceDirectory"/> is null.
        /// </summary>
        public bool ForceCompile { get; set; }

        /// <summary>
        /// If <see langword="true"/>, .dll files from package references are decompiled into .cs files, so that they can be transpiled to lua.
        /// Exceptions are made for System.* packages, *.Sources packages, and the Microsoft.NETCore.Platforms and NETStandard.Library packages.
        /// This setting is ignored if either <see cref="DecompilePackages"/> or <see cref="ExcludeDecompilePackages"/> is not <see langword="null"/>.
        /// </summary>
        public bool DecompilePackageLibs { get; set; }

        /// <summary>
        /// Case-insensitive list (separated by ;) of package names for which the .dll file(s) should be decompiled into .cs files, so that they can be transpiled to lua.
        /// Supports ? and * as wildcard characters.
        /// </summary>
        public string? DecompilePackages { get; set; }

        /// <summary>
        /// Case-insensitive list (separated by ;) of package names for which the .dll file(s) should NOT be decompiled into .cs files.
        /// Supports ? and * as wildcard characters.
        /// </summary>
        public string? ExcludeDecompilePackages { get; set; }

        /// <summary>
        /// <see cref="CSharpLua.Compiler.IsExportMetadata"/>.
        /// </summary>
        public bool ExportMetadata { get; set; }

        public bool Debug { get; set; }

        public bool Optimize { get; set; }

        public bool Obfuscate { get; set; }

        public Dictionary<string, MpqFileFlags> FileFlags { get; private set; }

        public MpqFileFlags DefaultFileFlags { get; set; }

        public MapInfo? MapInfo { get; set; }

        public MapEnvironment? MapEnvironment { get; set; }

        public MapDoodads? MapDoodads { get; set; }

        public MapUnits? MapUnits { get; set; }

        public MapRegions? MapRegions { get; set; }

        public MapSounds? MapSounds { get; set; }

        public MapPreviewIcons? MapIcons { get; set; }

        public UnitObjectData? MapUnitData { get; set; }

        public ItemObjectData? MapItemData { get; set; }

        public DestructableObjectData? MapDestructableData { get; set; }

        public DoodadObjectData? MapDoodadData { get; set; }

        public AbilityObjectData? MapAbilityData { get; set; }

        public BuffObjectData? MapBuffData { get; set; }

        public UpgradeObjectData? MapUpgradeData { get; set; }

        public MapCustomTextTriggers? MapCustomTextTriggers { get; set; }

        public MapTriggers? MapTriggers { get; set; }

        public TriggerStrings? MapTriggerStrings { get; set; }

        public MapCameras? MapCameras { get; set; }

        public MapPathingMap? MapPathingMap { get; set; }

        public MapShadowMap? MapShadowMap { get; set; }

        public string? LobbyMusic { get; set; }

        public GamePatch? TargetPatch { get; set; }

        internal List<string> Libraries { get; private set; }

        public object? GetMapFile(string fileName)
        {
            return fileName switch
            {
                MapInfo.FileName => MapInfo,
                MapEnvironment.FileName => MapEnvironment,
                MapDoodads.FileName => MapDoodads,
                MapUnits.FileName => MapUnits,
                MapRegions.FileName => MapRegions,
                MapSounds.FileName => MapSounds,
                MapPreviewIcons.FileName => MapIcons,

                MapUnitObjectData.FileName => MapUnitData,
                MapItemObjectData.FileName => MapItemData,
                MapDestructableObjectData.FileName => MapDestructableData,
                MapDoodadObjectData.FileName => MapDoodadData,
                MapAbilityObjectData.FileName => MapAbilityData,
                MapBuffObjectData.FileName => MapBuffData,
                MapUpgradeObjectData.FileName => MapUpgradeData,

                _ => null,
            };
        }

        public bool SetMapFile(object file)
        {
            if (file is null)
            {
                return false;
            }
            else if (file is MapInfo mapInfo)
            {
                MapInfo = mapInfo;
            }
            else if (file is MapEnvironment mapEnvironment)
            {
                MapEnvironment = mapEnvironment;
            }
            else if (file is MapDoodads mapDoodads)
            {
                MapDoodads = mapDoodads;
            }
            else if (file is MapUnits mapUnits)
            {
                MapUnits = mapUnits;
            }
            else if (file is MapRegions mapRegions)
            {
                MapRegions = mapRegions;
            }
            else if (file is MapSounds mapSounds)
            {
                MapSounds = mapSounds;
            }
            else if (file is MapPreviewIcons mapIcons)
            {
                MapIcons = mapIcons;
            }
            else if (file is MapUnitObjectData mapUnitData)
            {
                MapUnitData = mapUnitData;
            }
            else if (file is MapItemObjectData mapItemData)
            {
                MapItemData = mapItemData;
            }
            else if (file is MapDestructableObjectData mapDestructableData)
            {
                MapDestructableData = mapDestructableData;
            }
            else if (file is MapDoodadObjectData mapDoodadData)
            {
                MapDoodadData = mapDoodadData;
            }
            else if (file is MapAbilityObjectData mapAbilityData)
            {
                MapAbilityData = mapAbilityData;
            }
            else if (file is MapBuffObjectData mapBuffData)
            {
                MapBuffData = mapBuffData;
            }
            else if (file is MapUpgradeObjectData mapUpgradeData)
            {
                MapUpgradeData = mapUpgradeData;
            }
            else
            {
                throw new ArgumentException("The object is not a map file.", nameof(file));
            }

            return true;
        }
    }
}