// ------------------------------------------------------------------------------
// <copyright file="MapBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;

using War3Net.Build.Audio;
using War3Net.Build.Common;
using War3Net.Build.Diagnostics;
using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Providers;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Renderer;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public sealed class MapBuilder
    {
        private const string DefaultMapName = "TestMap.w3x";
        private const ushort DefaultBlockSize = 3;
        private const bool DefaultGenerateListFile = true;

        private string _outputMapName;
        private ushort _blockSize;
        private bool _generateListfile;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapBuilder"/> class.
        /// </summary>
        public MapBuilder()
            : this(DefaultMapName, DefaultBlockSize, DefaultGenerateListFile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapBuilder"/> class.
        /// </summary>
        public MapBuilder(string mapName)
            : this(mapName, DefaultBlockSize, DefaultGenerateListFile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapBuilder"/> class.
        /// </summary>
        public MapBuilder(string mapName, ushort blockSize)
            : this(mapName, blockSize, DefaultGenerateListFile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapBuilder"/> class.
        /// </summary>
        public MapBuilder(string mapName, ushort blockSize, bool generateListfile)
        {
            _outputMapName = mapName;
            _blockSize = blockSize;
            _generateListfile = generateListfile;
        }

        public event EventHandler<ArchiveBuildingEventArgs> OnArchiveBuilding;

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

        public bool GenerateListfile
        {
            get => _generateListfile;
            set => _generateListfile = value;
        }

        public BuildResult Build(ScriptCompilerOptions compilerOptions, params string[] assetsDirectories)
        {
            if (compilerOptions is null)
            {
                throw new ArgumentNullException(nameof(compilerOptions));
            }

            Directory.CreateDirectory(compilerOptions.OutputDirectory);

            var files = new Dictionary<(string fileName, MpqLocale locale), Stream>();
            var diagnostics = new List<Diagnostic>();
            var haveErrorDiagnostic = false;

            bool TrySetMapFile<TMapFile>(MapFileHandler<TMapFile> handler, Func<TMapFile> defaultObjectGenerator = null)
            {
                var fileName = handler.FileName;
                if (!(compilerOptions.GetMapFile(fileName) is TMapFile mapFile))
                {
                    mapFile = default;

                    var fileStream = FindFile(fileName, assetsDirectories);
                    if (fileStream is null)
                    {
                        var required = handler.IsRequired;
                        var requiredString = required ? "required" : "optional";
                        if (required && defaultObjectGenerator is null)
                        {
                            AddDiagnostic(Diagnostic.Create(DiagnosticProvider.MissingMapFile, null, fileName, requiredString), diagnostics, ref haveErrorDiagnostic);
                        }
                        else
                        {
                            var severity = required ? DiagnosticSeverity.Warning : DiagnosticSeverity.Info;
                            AddDiagnostic(Diagnostic.Create(DiagnosticProvider.MissingMapFile, null, severity, null, null, fileName, requiredString), diagnostics, ref haveErrorDiagnostic);
                            if (required || !compilerOptions.Optimize)
                            {
                                mapFile = (defaultObjectGenerator ?? handler.GetDefault)();
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            mapFile = handler.Parse(fileStream, false);
                        }
                        catch (TargetInvocationException e)
                        {
                            var innerException = e.InnerException;
                            var diagnosticDescriptor = innerException is InvalidDataException ? DiagnosticProvider.InvalidMapFile : DiagnosticProvider.GenericMapFileError;

                            AddDiagnostic(Diagnostic.Create(diagnosticDescriptor, null, fileName, innerException.Message), diagnostics, ref haveErrorDiagnostic);
                        }
                    }
                }

                if (compilerOptions.SetMapFile(mapFile))
                {
                    var outputPath = Path.Combine(compilerOptions.OutputDirectory, fileName);
                    try
                    {
                        var outputStream = File.Create(outputPath);
                        handler.Serialize(mapFile, outputStream, false);

                        files.Add((fileName, MpqLocale.Neutral), File.OpenRead(outputPath));
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        // todo: generate diagnostic
                        throw;
                    }
                    catch (IOException e)
                    {
                        // todo: generate diagnostic
                        throw;
                    }
                }

                return !haveErrorDiagnostic;
            }

            if (!compilerOptions.TargetPatch.HasValue)
            {
                AddDiagnostic(Diagnostic.Create(CompatibilityDiagnostics.TargetPatchNotSet, null), diagnostics, ref haveErrorDiagnostic);
            }

            // Set map files
            if (!TrySetMapFile(new MapFileHandler<MapInfo>()) ||
                !TrySetMapFile(new MapFileHandler<MapEnvironment>(), () => new MapEnvironment(compilerOptions.MapInfo)) ||
                !TrySetMapFile(new MapFileHandler<MapDoodads>()) ||
                !TrySetMapFile(new MapFileHandler<MapUnits>()) ||
                !TrySetMapFile(new MapFileHandler<MapRegions>()) ||
                !TrySetMapFile(new MapFileHandler<MapSounds>()) ||
                !TrySetMapFile(new MapFileHandler<MapPreviewIcons>(), () => new MapPreviewIcons(compilerOptions.MapInfo, compilerOptions.MapEnvironment, compilerOptions.MapUnits)) ||
                !TrySetMapFile(new MapFileHandler<MapUnitObjectData>()) ||
                !TrySetMapFile(new MapFileHandler<MapItemObjectData>()) ||
                !TrySetMapFile(new MapFileHandler<MapDestructableObjectData>()) ||
                !TrySetMapFile(new MapFileHandler<MapDoodadObjectData>()) ||
                !TrySetMapFile(new MapFileHandler<MapAbilityObjectData>()) ||
                !TrySetMapFile(new MapFileHandler<MapBuffObjectData>()) ||
                !TrySetMapFile(new MapFileHandler<MapUpgradeObjectData>()))
            {
                return GenerateResult(diagnostics, haveErrorDiagnostic);
            }

            // Generate script file
            if (compilerOptions.SourceDirectory != null)
            {
                if (!Directory.Exists(compilerOptions.SourceDirectory))
                {
                    AddDiagnostic(Diagnostic.Create(DiagnosticProvider.MissingSourceDirectory, null, compilerOptions.SourceDirectory), diagnostics, ref haveErrorDiagnostic);
                    return GenerateResult(diagnostics, haveErrorDiagnostic);
                }

                var compileResult = Compile(compilerOptions, out var path);
                if (compileResult.Success)
                {
                    files.Add((new FileInfo(path).Name, MpqLocale.Neutral), File.OpenRead(path));
                }
                else
                {
                    return new BuildResult(!haveErrorDiagnostic, compileResult, diagnostics);
                }
            }
            else if (compilerOptions.ForceCompile)
            {
                CompileSimple(compilerOptions, out var path);
                files.Add((new FileInfo(path).Name, MpqLocale.Neutral), File.OpenRead(path));
            }

            // Load assets
            foreach (var assetsDirectory in assetsDirectories)
            {
                if (string.IsNullOrWhiteSpace(assetsDirectory))
                {
                    continue;
                }

                EnumerateFiles(assetsDirectory, files);
            }

            // Load assets from projects
            /*foreach (var contentReference in references
                .Where(reference => reference is ProjectReference)
                .Select(reference => reference.Folder))
            {
                // TODO: use ProjectReference._project.Files? (since this would pre-enumerate over the files instead of returning a folder, will still need to deal with locale folders)
                // EnumerateFiles(contentReference);
            }

            // Load assets from packages
            foreach (var contentReference in references
                .Where(reference => reference is PackageReference)
                .Select(reference => Path.Combine(reference.Folder, ContentReference.ContentFolder)))
            {
                EnumerateFiles(contentReference);
            }*/

            // Generate (listfile)
            var generateListfile = compilerOptions.FileFlags.TryGetValue(ListFile.Key, out var listfileFlags)
                ? listfileFlags.HasFlag(MpqFileFlags.Exists)
                : _generateListfile; // compilerOptions.DefaultFileFlags.HasFlag(MpqFileFlags.Exists);

            if (generateListfile)
            {
                using var listFile = new ListFile(files.Select(file => file.Key.fileName));
                listFile.Finish(true);

                if (files.ContainsKey((ListFile.Key, MpqLocale.Neutral)))
                {
                    files[(ListFile.Key, MpqLocale.Neutral)].Dispose();
                    files.Remove((ListFile.Key, MpqLocale.Neutral));
                }

                files.Add((ListFile.Key, MpqLocale.Neutral), listFile.BaseStream);

                using var fileStream = File.Create(Path.Combine(compilerOptions.OutputDirectory, ListFile.Key));
                listFile.BaseStream.CopyTo(fileStream);
            }

            // Create warning diagnostics if applicable.
            var expectedFiles = new HashSet<string>(compilerOptions.FileFlags.Where(pair => pair.Value.HasFlag(MpqFileFlags.Exists)).Select(pair => pair.Key));
            var haveNeutralLocale = new Dictionary<string, bool>();

            // Generate mpq files
            var mpqFiles = new List<MpqFile>(files.Count);
            foreach (var file in files)
            {
                var fileName = file.Key.fileName;
                var locale = file.Key.locale;
                var isNeutralLocale = locale == MpqLocale.Neutral;

                expectedFiles.Remove(fileName);
                if (haveNeutralLocale.TryGetValue(fileName, out var yes))
                {
                    haveNeutralLocale[fileName] = yes || isNeutralLocale;
                }
                else
                {
                    haveNeutralLocale.Add(fileName, isNeutralLocale);
                }

                var fileflags = compilerOptions.FileFlags.TryGetValue(fileName, out var flags) ? flags : compilerOptions.DefaultFileFlags;
                if (fileflags.HasFlag(MpqFileFlags.Exists))
                {
                    var mpqFile = MpqFile.New(file.Value, fileName);
                    mpqFile.TargetFlags = fileflags;
                    mpqFile.Locale = locale;
                    mpqFiles.Add(mpqFile);
                }
                else
                {
                    file.Value.Dispose();
                }
            }

            // Generate warnings
            foreach (var expectedFile in expectedFiles)
            {
                AddDiagnostic(Diagnostic.Create(DiagnosticProvider.MissingFileWithCustomMpqFlags, null, expectedFile, compilerOptions.FileFlags[expectedFile]), diagnostics, ref haveErrorDiagnostic);
            }

            foreach (var pair in haveNeutralLocale)
            {
                if (!pair.Value)
                {
                    AddDiagnostic(Diagnostic.Create(DiagnosticProvider.MissingFileNeutralLocale, null, pair.Key), diagnostics, ref haveErrorDiagnostic);
                }
            }

            // Generate .mpq archive file
            OnArchiveBuilding?.Invoke(this, new ArchiveBuildingEventArgs(mpqFiles));

            var outputMap = Path.Combine(compilerOptions.OutputDirectory, _outputMapName);
            MpqArchive.Create(File.Create(outputMap), mpqFiles, blockSize: _blockSize).Dispose();

            // TODO: pass compileResult argument
            return GenerateResult(diagnostics, haveErrorDiagnostic);
        }

        private static Stream FindFile(string searchedFile, string[] assetsDirectories)
        {
            foreach (var assetsDirectory in assetsDirectories)
            {
                if (string.IsNullOrWhiteSpace(assetsDirectory))
                {
                    continue;
                }

                foreach (var (fileName, _, stream) in FileProvider.EnumerateFiles(assetsDirectory))
                {
                    if (fileName == searchedFile)
                    {
                        return stream;
                    }
                }
            }

            return null;
        }

        private static void EnumerateFiles(string directory, Dictionary<(string fileName, MpqLocale locale), Stream> files)
        {
            foreach (var (fileName, locale, stream) in FileProvider.EnumerateFiles(directory))
            {
                if (files.ContainsKey((fileName, locale)))
                {
                    stream.Dispose();
                }
                else
                {
                    files.Add((fileName, locale), stream);
                }
            }
        }

        private static void AddDiagnostic(Diagnostic diagnostic, List<Diagnostic> diagnostics, ref bool haveErrorDiagnostic)
        {
            diagnostics.Add(diagnostic);
            if (diagnostic.Severity == DiagnosticSeverity.Error)
            {
                haveErrorDiagnostic = true;
            }
        }

        private static BuildResult GenerateResult(List<Diagnostic> diagnostics, bool haveErrorDiagnostic)
        {
            return new BuildResult(!haveErrorDiagnostic, null, diagnostics);
        }

        public CompileResult Compile(ScriptCompilerOptions options, out string scriptFilePath)
        {
            var compiler = GetCompiler(options);
            compiler.BuildAutogeneratedCode(out var globalsFilePath, out var mainFunctionFilePath, out var configFunctionFilePath);
            return compiler.Compile(out scriptFilePath, globalsFilePath, mainFunctionFilePath, configFunctionFilePath);
        }

        public void CompileSimple(ScriptCompilerOptions options, out string scriptFilePath)
        {
            var compiler = GetCompiler(options);
            compiler.BuildAutogeneratedCode(out var globalsFilePath, out var mainFunctionFilePath, out var configFunctionFilePath);
            compiler.CompileSimple(out scriptFilePath, globalsFilePath, mainFunctionFilePath, configFunctionFilePath);
        }

        private ScriptCompiler GetCompiler(ScriptCompilerOptions options)
        {
            switch (options.MapInfo.ScriptLanguage)
            {
                case ScriptLanguage.Jass:
                    return new JassScriptCompiler(options, JassRendererOptions.Default);

                case ScriptLanguage.Lua:
                    // TODO: distinguish C# and lua (possibly by creating subclasses of ScriptCompilerOptions)
                    var rendererOptions = new CSharpLua.LuaSyntaxGenerator.SettingInfo();
                    rendererOptions.Indent = 4;

                    return new CSharpScriptCompiler(options, rendererOptions);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}