// ------------------------------------------------------------------------------
// <copyright file="MapBuilder.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Providers;
using War3Net.Build.Script;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public class MapBuilder
    {
        private string _outputMapName;
        private ushort _blockSize;
        private bool _generateListfile;

        public MapBuilder()
        {
            _outputMapName = "TestMap.w3x";
            _blockSize = 3;
            _generateListfile = true;
        }

        public MapBuilder(string mapName)
        {
            _outputMapName = mapName;
            _blockSize = 3;
            _generateListfile = true;
        }

        public MapBuilder(string mapName, ushort blockSize)
        {
            _outputMapName = mapName;
            _blockSize = blockSize;
            _generateListfile = true;
        }

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
            var files = new Dictionary<string, Stream>();

            // Generate script file
            if (compilerOptions != null)
            {
                if (Compile(compilerOptions, out var path))
                {
                    files.Add(new FileInfo(path).Name, File.OpenRead(path));
                }
                else
                {
                    return false;
                }
            }

            // Load assets
            foreach (var assetsDirectory in assetsDirectories)
            {
                foreach (var (key, value) in FileProvider.EnumerateFiles(assetsDirectory))
                {
                    if (!files.ContainsKey(key))
                    {
                        files.Add(key, value);
                    }
                    else
                    {
                        value.Dispose();
                    }
                }
            }

            // Generate (listfile)
            if (_generateListfile)
            {
                var listfilePath = Path.Combine(compilerOptions.OutputDirectory, ListFile.Key);
                using (var listfileStream = File.Create(listfilePath))
                {
                    using (var streamWriter = new StreamWriter(listfileStream))
                    {
                        foreach (var file in files)
                        {
                            foreach (var path in file.Key)
                            {
                                streamWriter.WriteLine(path);
                            }
                        }
                    }
                }

                if (files.ContainsKey(ListFile.Key))
                {
                    files[ListFile.Key].Dispose();
                    files.Remove(ListFile.Key);
                }

                files.Add(ListFile.Key, File.OpenRead(listfilePath));
            }

            // Generate mpq files
            var mpqFiles = new List<MpqFile>(files.Count);
            foreach (var file in files)
            {
                // TODO: allow compression/encryption/etc
                mpqFiles.Add(new MpqFile(file.Value, file.Key, MpqFileFlags.Exists, _blockSize));
            }

            // Generate .mpq file
            var outputMap = Path.Combine(compilerOptions.OutputDirectory, _outputMapName);
            MpqArchive.Create(File.Create(outputMap), mpqFiles, blockSize: _blockSize).Dispose();

            return true;
        }

        public bool Compile(ScriptCompilerOptions options, out string scriptFilePath)
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
        }
    }
}