// ------------------------------------------------------------------------------
// <copyright file="JassScriptCompiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;

using War3Net.CodeAnalysis.Jass;

namespace War3Net.Build.Script
{
    public sealed class JassScriptCompiler : ScriptCompiler
    {
        private readonly string _jasshelperPath;
        private readonly string _commonPath;
        private readonly string _blizzardPath;

        public JassScriptCompiler(ScriptCompilerOptions options)
            : base(options)
        {
            // todo: retrieve these vals from somewhere
            var x86 = true;
            var ptr = false;

            _jasshelperPath = Path.Combine(new FileInfo(Providers.WarcraftPathProvider.GetExePath(x86, ptr)).DirectoryName, "JassHelper", "jasshelper.exe");

            var jasshelperDocuments = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                ptr ? "Warcraft III Public Test" : "Warcraft III",
                "Jasshelper");
            _commonPath = Path.Combine(jasshelperDocuments, "common.j");
            _blizzardPath = Path.Combine(jasshelperDocuments, "Blizzard.j");
        }

        public override ScriptBuilder GetScriptBuilder()
        {
            return new JassScriptBuilder();
        }

        public override bool Compile(params string[] additionalSourceFiles)
        {
            var inputScript = Path.Combine(Options.OutputDirectory, "files.j");
            using (var inputScriptStream = Providers.FileProvider.OpenNewWrite(inputScript))
            {
                using (var streamWriter = new StreamWriter(inputScriptStream))
                {
                    foreach (var file in Directory.EnumerateFiles(Options.SourceDirectory, "*.j", SearchOption.AllDirectories))
                    {
                        streamWriter.WriteLine($"//! import \"{file}\"");
                    }

                    foreach (var file in additionalSourceFiles)
                    {
                        streamWriter.WriteLine($"//! import \"{file}\"");
                    }
                }
            }

            var outputScript = "war3map.j";
            var jasshelperOutputScript = Path.Combine(Options.OutputDirectory, Options.Obfuscate ? "war3map.original.j" : outputScript);
            var jasshelperOptions = Options.Debug ? "--debug" : Options.Optimize ? string.Empty : "--nooptimize";
            var jasshelper = Process.Start(_jasshelperPath, $"{jasshelperOptions} --scriptonly \"{_commonPath}\" \"{_blizzardPath}\" \"{inputScript}\" \"{jasshelperOutputScript}\"");
            jasshelper.WaitForExit();

            var success = jasshelper.ExitCode == 0;
            if (success && Options.Obfuscate)
            {
                JassObfuscator.Obfuscate(jasshelperOutputScript, Path.Combine(Options.OutputDirectory, outputScript), _commonPath, _blizzardPath);
            }

            return success;
        }
    }
}