// ------------------------------------------------------------------------------
// <copyright file="CSharpScriptCompiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CSharpLua;
using CSharpLua.LuaAst;

using War3Net.Build.Providers;

namespace War3Net.Build.Script
{
    internal sealed class CSharpScriptCompiler : ScriptCompiler
    {
        private readonly LuaSyntaxGenerator.SettingInfo _rendererOptions;

        public CSharpScriptCompiler(ScriptCompilerOptions options, LuaSyntaxGenerator.SettingInfo rendererOptions)
            : base(options)
        {
            // options.BuilderOptions.InitializationFunctions.Add("InitCSharp");

            _rendererOptions = rendererOptions;
        }

        [Obsolete]
        public override ScriptBuilder GetScriptBuilder()
        {
            return new LuaScriptBuilder();
        }

        public override void BuildMainAndConfig(out string mainFunctionFilePath, out string configFunctionFilePath)
        {
            var mainFunctionBuilder = new LuaMainFunctionBuilder(Options.MapInfo);
            mainFunctionBuilder.EnableCSharp = true;
            mainFunctionFilePath = Path.Combine(Options.OutputDirectory, "main.lua");
            RenderFunctionSyntaxToFile(mainFunctionBuilder.Build(), mainFunctionFilePath);

            var configFunctionBuilder = new LuaConfigFunctionBuilder(Options.MapInfo);
            configFunctionBuilder.LobbyMusic = Options.LobbyMusic;
            configFunctionFilePath = Path.Combine(Options.OutputDirectory, "config.lua");
            RenderFunctionSyntaxToFile(configFunctionBuilder.Build(), configFunctionFilePath);
        }

        // Additional source files (usually main.lua and config.lua) are assumed to be .lua source files, not .cs source files.
        public override bool Compile(IEnumerable<ContentReference> references, out string scriptFilePath, params string[] additionalSourceFiles)
        {
            scriptFilePath = Path.Combine(Options.OutputDirectory, "war3map.lua");

            if (Options.Obfuscate)
            {
                // Can either obfuscate the input C# source or the output lua source, depends on availability of existing libraries.
                throw new NotImplementedException();
            }

            // Options.Debug;
            // Options.Optimize;
            // Options.Obfuscate;

            // new arguments, not tested
            var exportEnums = true;
            var preventDebug = true;
            // ---

            var compiler = new Compiler(Options.SourceDirectory, scriptFilePath, null, null, null, false, null, exportEnums ? string.Empty : null)
            {
                IsExportMetadata = false,
                IsModule = false,
                IsInlineSimpleProperty = false,
                IsPreventDebugObject = preventDebug,
                IsOutputSingleFile = true,
            };

            try
            {
                compiler.Compile(references);
            }
            catch (CompilationErrorException e)
            {
                // Console.WriteLine(e.Message);
                return false;
            }

            using (var fileStream = File.OpenWrite(scriptFilePath))
            {
                fileStream.Seek(0, SeekOrigin.End);
                foreach (var additionalSourceFile in additionalSourceFiles)
                {
                    using (var writer = new StreamWriter(fileStream, new UTF8Encoding(false, true), 1024, true))
                    {
                        writer.Write(File.ReadAllText(additionalSourceFile));
                        writer.WriteLine();
                    }
                }
            }

            return true;
        }

        [Obsolete("These .dll's are now automatically added through PackageReference.EnumerateLibraries()", true)]
        private static IEnumerable<string> DiscoverWar3ApiPackageLibs(bool referenceBlizzardLib = true)
        {
            // TODO: use 'dotnet nuget locals global-packages --list' to find global package locations, instead of hardcoded default path
            // TODO: don't hardcode 1.31.1 and netstandard2.0 values in path

            var globalPackageDirectory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), @".nuget\packages");

            yield return Path.Combine(globalPackageDirectory, @"war3api.common\1.31.1\lib\netstandard2.0\War3Api.Common.dll");

            if (referenceBlizzardLib)
            {
                yield return Path.Combine(globalPackageDirectory, @"war3api.blizzard\1.31.1\lib\netstandard2.0\War3Api.Blizzard.dll");
            }
        }

        private void RenderFunctionSyntaxToFile(LuaVariableListDeclarationSyntax function, string path)
        {
            using (var fileStream = FileProvider.OpenNewWrite(path))
            {
                using (var writer = new StreamWriter(fileStream, new UTF8Encoding(false, true)))
                {
                    var renderer = new LuaRenderer(_rendererOptions, writer);

                    var compilationUnitSyntax = new LuaCompilationUnitSyntax();
                    compilationUnitSyntax.AddStatement(function);
                    renderer.RenderCompilationUnit(compilationUnitSyntax);
                }
            }
        }
    }
}