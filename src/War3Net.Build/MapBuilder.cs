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
using System.Text;

using CSharpLua;

using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Renderer;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public class MapBuilder
    {
        private string _outputMapName;
        private ushort _blockSize;
        private bool _generateListfile;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapBuilder"/> class.
        /// </summary>
        public MapBuilder()
        {
            _outputMapName = "TestMap.w3x";
            _blockSize = 3;
            _generateListfile = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapBuilder"/> class.
        /// </summary>
        /// <param name="mapName"></param>
        public MapBuilder(string mapName)
        {
            _outputMapName = mapName;
            _blockSize = 3;
            _generateListfile = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapBuilder"/> class.
        /// </summary>
        /// <param name="mapName"></param>
        /// <param name="blockSize"></param>
        public MapBuilder(string mapName, ushort blockSize)
        {
            _outputMapName = mapName;
            _blockSize = blockSize;
            _generateListfile = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapBuilder"/> class.
        /// </summary>
        /// <param name="mapName"></param>
        /// <param name="blockSize"></param>
        /// <param name="generateListfile"></param>
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

        public bool Build(ScriptCompilerOptions compilerOptions, params string[] assetsDirectories)
        {
            Directory.CreateDirectory(compilerOptions.OutputDirectory);

            var files = new Dictionary<(string fileName, MpqLocale locale), Stream>();

            // Generate mapInfo file
            if (compilerOptions.MapInfo != null)
            {
                var path = Path.Combine(compilerOptions.OutputDirectory, MapInfo.FileName);
                using (var fileStream = File.Create(path))
                {
                    compilerOptions.MapInfo.SerializeTo(fileStream);
                }

                files.Add((new FileInfo(path).Name, MpqLocale.Neutral), File.OpenRead(path));
            }
            else
            {
                // TODO: set MapInfo by parsing war3map.w3i from assetsDirectories, because it's needed for compilation.
                // compilerOptions.MapInfo = ...;
                throw new NotImplementedException();
            }

            /*var references = new[] { new FolderReference(compilerOptions.SourceDirectory) };
            var references = new List<ContentReference>();
            RecursiveAddReferences(new ProjectReference(compilerOptions.SourceDirectory));

            void RecursiveAddReferences(ContentReference contentReference)
            {
                if (references.Contains(contentReference))
                {
                    return;
                }

                references.Add(contentReference);
                foreach (var reference in contentReference.GetReferences(false))
                {
                    if (reference is ProjectReference)
                    {
                        RecursiveAddReferences(reference);
                    }

                    if (reference is PackageReference packageReference)
                    {
                        // TODO: replace with better condition
                        if (packageReference.Name.StartsWith("War3Api") || packageReference.Name.StartsWith("War3Lib") || packageReference.Name == "War3Net.CodeAnalysis.Common")
                        {
                            RecursiveAddReferences(reference);
                        }
                    }
                }
            }*/

            // Generate script file
            if (compilerOptions.SourceDirectory != null)
            {
                if (Compile(compilerOptions, out var path))
                {
                    files.Add((new FileInfo(path).Name, MpqLocale.Neutral), File.OpenRead(path));
                }
                else
                {
                    return false;
                }
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

                /*var listfilePath = Path.Combine(compilerOptions.OutputDirectory, ListFile.Key);
                using (var listfileStream = File.Create(listfilePath))
                {
                    using (var streamWriter = new StreamWriter(listfileStream, new UTF8Encoding(false)))
                    {
                        foreach (var file in files)
                        {
                            streamWriter.WriteLine(file.Key.fileName);
                        }
                    }
                }

                if (files.ContainsKey((ListFile.Key, MpqLocale.Neutral)))
                {
                    files[(ListFile.Key, MpqLocale.Neutral)].Dispose();
                    files.Remove((ListFile.Key, MpqLocale.Neutral));
                }

                files.Add((ListFile.Key, MpqLocale.Neutral), File.OpenRead(listfilePath));*/
            }

            // Generate mpq files
            var mpqFiles = new List<MpqFile>(files.Count);
            foreach (var file in files)
            {
                var fileflags = compilerOptions.FileFlags.TryGetValue(file.Key.fileName, out var flags) ? flags : compilerOptions.DefaultFileFlags;
                if (fileflags.HasFlag(MpqFileFlags.Exists))
                {
                    // mpqFiles.Add(new MpqKnownFile(file.Key.fileName, file.Value, fileflags, file.Key.locale));
                    var mpqFile = MpqFile.New(file.Value, file.Key.fileName);
                    mpqFile.TargetFlags = fileflags;
                    mpqFile.Locale = file.Key.locale;
                    mpqFiles.Add(mpqFile);

                }
                else
                {
                    file.Value.Dispose();
                }
            }

            // Generate .mpq archive file
            var outputMap = Path.Combine(compilerOptions.OutputDirectory, _outputMapName);
            MpqArchive.Create(File.Create(outputMap), mpqFiles, blockSize: _blockSize).Dispose();

            return true;
        }

        /*public bool Compile(ScriptCompilerOptions options, out string scriptFilePath)
        {
            var compiler = ScriptCompiler.GetUnknownLanguageCompiler(options);
            if (compiler is null)
            {
                scriptFilePath = null;
                return false;
            }

            var scriptBuilder = compiler.GetScriptBuilder();
            scriptFilePath = Path.Combine(options.OutputDirectory, $"war3map{scriptBuilder.Extension}");

            var mainFunctionFile = Path.Combine(options.OutputDirectory, $"main{scriptBuilder.Extension}");
            scriptBuilder.BuildMainFunction(mainFunctionFile, options.BuilderOptions);

            var configFunctionFile = Path.Combine(options.OutputDirectory, $"config{scriptBuilder.Extension}");
            scriptBuilder.BuildConfigFunction(configFunctionFile, options.BuilderOptions);

            return compiler.Compile(mainFunctionFile, configFunctionFile);
        }*/

        public bool Compile(ScriptCompilerOptions options, out string scriptFilePath)
        {
            var compiler = GetCompiler(options);
            compiler.BuildMainAndConfig(out var mainFunctionFilePath, out var configFunctionFilePath);
            return compiler.Compile(out scriptFilePath, mainFunctionFilePath, configFunctionFilePath);
        }

        private ScriptCompiler GetCompiler(ScriptCompilerOptions options)
        {
            switch (options.MapInfo.ScriptLanguage)
            {
                case ScriptLanguage.Jass:
                    return new JassScriptCompiler(options, JassRendererOptions.Default);

                case ScriptLanguage.Lua:
                    // TODO: distinguish C# and lua
                    var rendererOptions = new CSharpLua.LuaSyntaxGenerator.SettingInfo();
                    rendererOptions.Indent = 4;

                    return new CSharpScriptCompiler(options, rendererOptions);

                default:
                    throw new Exception();
            }
        }
    }
}