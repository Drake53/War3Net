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

using Microsoft.CodeAnalysis;

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Renderer;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public class MapBuilder
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

            void AddDiagnostic(Diagnostic diagnostic)
            {
                diagnostics.Add(diagnostic);
                if (diagnostic.Severity == DiagnosticSeverity.Error)
                {
                    haveErrorDiagnostic = true;
                }
            }

            void TrySetMapFile<TMapFile>(string fileName, Func<Stream, bool, TMapFile> parser, Action<TMapFile, Stream, bool> serializer, DiagnosticDescriptor descriptor, Func<TMapFile> defaultObjectGenerator = null)
            {
                if (!(compilerOptions.GetMapFile(fileName) is TMapFile mapFile))
                {
                    var fileStream = FindFile(fileName);
                    if (fileStream is null)
                    {
                        AddDiagnostic(Diagnostic.Create(descriptor, null));

                        defaultObjectGenerator ??= (Func<TMapFile>)(() => default);
                        mapFile = defaultObjectGenerator();
                    }
                    else
                    {
                        mapFile = parser(fileStream, false);
                    }
                }

                if (compilerOptions.SetMapFile(mapFile))
                {
                    var outputPath = Path.Combine(compilerOptions.OutputDirectory, fileName);
                    using (var outputStream = File.Create(outputPath))
                    {
                        serializer(mapFile, outputStream, false);
                    }

                    files.Add((fileName, MpqLocale.Neutral), File.OpenRead(outputPath));
                }
            }

            TrySetMapFile(MapInfo.FileName,        MapInfo.Parse,        MapInfo.Serialize,        DiagnosticProvider.MissingMapInfo);
            if (haveErrorDiagnostic)
            {
                return new BuildResult(!haveErrorDiagnostic, null, diagnostics);
            }

            TrySetMapFile(MapEnvironment.FileName, MapEnvironment.Parse, MapEnvironment.Serialize, DiagnosticProvider.MissingMapEnvironment, () => new MapEnvironment(compilerOptions.MapInfo));
            TrySetMapFile(MapDoodads.FileName,     MapDoodads.Parse,     MapDoodads.Serialize,     DiagnosticProvider.MissingMapDoodads);
            TrySetMapFile(MapUnits.FileName,       MapUnits.Parse,       MapUnits.Serialize,       DiagnosticProvider.MissingMapUnits);
            TrySetMapFile(MapRegions.FileName,     MapRegions.Parse,     MapRegions.Serialize,     DiagnosticProvider.MissingMapRegions);
            TrySetMapFile(MapSounds.FileName,      MapSounds.Parse,      MapSounds.Serialize,      DiagnosticProvider.MissingMapSounds);

            // Generate script file
            if (compilerOptions.SourceDirectory != null)
            {
                if (!Directory.Exists(compilerOptions.SourceDirectory))
                {
                    AddDiagnostic(Diagnostic.Create(DiagnosticProvider.MissingSourceDirectory, null, compilerOptions.SourceDirectory));
                    return new BuildResult(!haveErrorDiagnostic, null, diagnostics);
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

            void EnumerateFiles(string directory)
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

            Stream FindFile(string searchedFile)
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

            // Load assets
            foreach (var assetsDirectory in assetsDirectories)
            {
                if (string.IsNullOrWhiteSpace(assetsDirectory))
                {
                    continue;
                }

                EnumerateFiles(assetsDirectory);
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
                AddDiagnostic(Diagnostic.Create(DiagnosticProvider.MissingFileWithCustomMpqFlags, null, expectedFile, compilerOptions.FileFlags[expectedFile]));
            }

            foreach (var pair in haveNeutralLocale)
            {
                if (!pair.Value)
                {
                    AddDiagnostic(Diagnostic.Create(DiagnosticProvider.MissingFileNeutralLocale, null, pair.Key));
                }
            }

            // Generate .mpq archive file
            var outputMap = Path.Combine(compilerOptions.OutputDirectory, _outputMapName);
            MpqArchive.Create(File.Create(outputMap), mpqFiles, blockSize: _blockSize).Dispose();

            // TODO: pass compileResult argument
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
                    throw new Exception();
            }
        }
    }
}