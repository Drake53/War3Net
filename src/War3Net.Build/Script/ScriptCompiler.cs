// ------------------------------------------------------------------------------
// <copyright file="ScriptCompiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using CSharpLua;

namespace War3Net.Build.Script
{
    internal abstract class ScriptCompiler
    {
        private readonly ScriptCompilerOptions _options;

        protected ScriptCompiler(ScriptCompilerOptions options)
        {
            _options = options;
        }

        protected ScriptCompilerOptions Options => _options;

        [Obsolete]
        public abstract ScriptBuilder GetScriptBuilder();

        public abstract void BuildMainAndConfig(out string mainFunctionFilePath, out string configFunctionFilePath);

        public abstract bool Compile(IEnumerable<ContentReference> references, out string scriptFilePath, params string[] additionalSourceFiles);

        [Obsolete("Script language should be obtained from MapInfo.ScriptLanguage property.", true)]
        public static ScriptCompiler GetUnknownLanguageCompiler(ScriptCompilerOptions options)
        {
            var sourceDirectory = options.SourceDirectory;

            var countJassFiles = 0;
            var countLuaFiles = 0;
            var countCSharpFiles = 0;

            var sourceDirectoryPathLength = sourceDirectory.Length + (sourceDirectory.EndsWith("\\") ? 0 : 1);
            foreach (var file in System.IO.Directory.EnumerateFiles(sourceDirectory, "*", System.IO.SearchOption.AllDirectories))
            {
                var relativePath = file.Substring(sourceDirectoryPathLength);

                if (relativePath.StartsWith(@"bin\") || relativePath.StartsWith(@"obj\"))
                {
                    continue;
                }

                switch (new System.IO.FileInfo(file).Extension.ToLower())
                {
                    case ".j": countJassFiles++; break;
                    case ".lua": countLuaFiles++; break;
                    case ".cs": countCSharpFiles++; break;

                    default: break;
                }
            }

            var countScriptLanguages =
                Math.Min(countJassFiles, 1) +
                Math.Min(countLuaFiles, 1) +
                Math.Min(countCSharpFiles, 1);

            if (countScriptLanguages == 1)
            {
                if (countJassFiles > 0) { return new JassScriptCompiler(options, null); }
                if (countCSharpFiles > 0) { return new CSharpScriptCompiler(options, null); }
                if (countLuaFiles > 0) { return new LuaScriptCompiler(options); }
            }

            return null;
        }
    }
}