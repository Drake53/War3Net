// ------------------------------------------------------------------------------
// <copyright file="LegacyMapBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;

using CSharpLua;

using Microsoft.CodeAnalysis;

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    [Obsolete]
    public sealed class LegacyMapBuilder
    {
        private const string DefaultMapName = "TestMap.w3x";
        private const ushort DefaultBlockSize = 3;
        private const bool DefaultGenerateListFile = true;

        private string _outputMapName;
        private ushort _blockSize;
        private bool _generateListFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyMapBuilder"/> class.
        /// </summary>
        public LegacyMapBuilder()
            : this(DefaultMapName, DefaultBlockSize, DefaultGenerateListFile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyMapBuilder"/> class.
        /// </summary>
        public LegacyMapBuilder(string mapName)
            : this(mapName, DefaultBlockSize, DefaultGenerateListFile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyMapBuilder"/> class.
        /// </summary>
        public LegacyMapBuilder(string mapName, ushort blockSize)
            : this(mapName, blockSize, DefaultGenerateListFile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyMapBuilder"/> class.
        /// </summary>
        public LegacyMapBuilder(string mapName, ushort blockSize, bool generateListFile)
        {
            _outputMapName = mapName;
            _blockSize = blockSize;
            _generateListFile = generateListFile;
        }

        [Obsolete("This event is no longer supported.", true)]
        public event EventHandler<ArchiveBuildingEventArgs>? OnArchiveBuilding;

        public string OutputMapName
        {
            get => _outputMapName;
            set => _outputMapName = value;
        }

        public ushort BlockSize
        {
            get => _blockSize;
            set => _blockSize = value;
        }

        public bool GenerateListFile
        {
            get => _generateListFile;
            set => _generateListFile = value;
        }

        public BuildResult Build(ScriptCompilerOptions compilerOptions, params string[] assetsDirectories)
        {
            if (compilerOptions is null)
            {
                throw new ArgumentNullException(nameof(compilerOptions));
            }

            var map = new Map(compilerOptions.MapInfo!, compilerOptions.MapEnvironment!)
            {
                Sounds = compilerOptions.MapSounds,
                PreviewIcons = compilerOptions.MapIcons,
                Regions = compilerOptions.MapRegions,
                AbilityObjectData = compilerOptions.MapAbilityData,
                BuffObjectData = compilerOptions.MapBuffData,
                DestructableObjectData = compilerOptions.MapDestructableData,
                DoodadObjectData = compilerOptions.MapDoodadData,
                ItemObjectData = compilerOptions.MapItemData,
                UnitObjectData = compilerOptions.MapUnitData,
                UpgradeObjectData = compilerOptions.MapUpgradeData,
                Doodads = compilerOptions.MapDoodads,
                Units = compilerOptions.MapUnits,
                CustomTextTriggers = compilerOptions.MapCustomTextTriggers,
                Triggers = compilerOptions.MapTriggers,
                TriggerStrings = compilerOptions.MapTriggerStrings,
                Cameras = compilerOptions.MapCameras,
                PathingMap = compilerOptions.MapPathingMap,
                ShadowMap = compilerOptions.MapShadowMap,
            };

            var builder = new MapBuilder(map);
            foreach (var assetDirectory in assetsDirectories)
            {
                builder.AddFiles(assetDirectory, "*", SearchOption.AllDirectories);
            }

            if (map.Info.ScriptLanguage == ScriptLanguage.Lua)
            {
                var csc = compilerOptions.Debug ? "-define:DEBUG" : null;
                var csproj = Directory.EnumerateFiles(compilerOptions.SourceDirectory, "*.csproj", SearchOption.TopDirectoryOnly).Single();
                var compiler = compilerOptions.DecompilePackageLibs && compilerOptions.DecompilePackages is null && compilerOptions.ExcludeDecompilePackages is null
                    ? new Compiler(csproj, compilerOptions.OutputDirectory, string.Empty, null, csc, false, null, string.Empty)
                    : new Compiler(csproj, compilerOptions.OutputDirectory, string.Empty, null, compilerOptions.DecompilePackages, compilerOptions.ExcludeDecompilePackages, csc, false, null, string.Empty);

                compiler.IsExportMetadata = false;
                compiler.IsModule = false;
                compiler.IsInlineSimpleProperty = false;
                compiler.IsPreventDebugObject = true;
                compiler.IsCommentsDisabled = compilerOptions.Optimize;

                var compileResult = string.IsNullOrEmpty(compilerOptions.CommonJPath) || string.IsNullOrEmpty(compilerOptions.BlizzardJPath)
                    ? map.CompileScript(compiler, compilerOptions.Libraries)
                    : map.CompileScript(compiler, compilerOptions.Libraries, compilerOptions.CommonJPath, compilerOptions.BlizzardJPath);

                if (!compileResult.Success)
                {
                    return new BuildResult(false, compileResult, Array.Empty<Diagnostic>());
                }
            }
            else
            {
                map.CompileScript();
            }

            // OnArchiveBuilding?.Invoke(this, new ArchiveBuildingEventArgs(mpqFiles));

            var archiveCreateOptions = new MpqArchiveCreateOptions
            {
                ListFileCreateMode = _generateListFile ? MpqFileCreateMode.Overwrite : MpqFileCreateMode.Prune,
                AttributesCreateMode = MpqFileCreateMode.Prune,
                BlockSize = _blockSize,
            };

            builder.Build(Path.Combine(compilerOptions.OutputDirectory, _outputMapName), archiveCreateOptions);

            return new BuildResult(true, null, Array.Empty<Diagnostic>());
        }
    }
}