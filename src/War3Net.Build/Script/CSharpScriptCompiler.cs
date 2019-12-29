// ------------------------------------------------------------------------------
// <copyright file="CSharpScriptCompiler.cs" company="Drake53">
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

        public override void BuildMainAndConfig(out string mainFunctionFilePath, out string configFunctionFilePath)
        {
            var functionBuilderData = new FunctionBuilderData(Options.MapInfo, Options.MapUnits, Options.LobbyMusic, Options.SourceDirectory != null);
            var functionBuilder = new LuaFunctionBuilder(functionBuilderData);

            mainFunctionFilePath = Path.Combine(Options.OutputDirectory, "main.lua");
            RenderFunctionSyntaxToFile(functionBuilder.BuildMainFunction(), mainFunctionFilePath);

            configFunctionFilePath = Path.Combine(Options.OutputDirectory, "config.lua");
            RenderFunctionSyntaxToFile(functionBuilder.BuildConfigFunction(), configFunctionFilePath);
        }

        // Additional source files (usually main.lua and config.lua) are assumed to be .lua source files, not .cs source files.
        public override bool Compile(out string scriptFilePath, params string[] additionalSourceFiles)
        {
            scriptFilePath = Path.Combine(Options.OutputDirectory, "war3map.lua");

            if (Options.Obfuscate)
            {
                // Can either obfuscate the input C# source or the output lua source, depends on availability of existing libraries.
                throw new NotImplementedException();
            }

            // Options.Obfuscate;

            // new arguments, not tested
            var exportEnums = true;
            var preventDebug = true;
            // ---

            // var csc = Options.Debug ? "-debug:full -define:DEBUG" : null;
            var csc = Options.Debug ? "-define:DEBUG" : null;

            var csproj = Directory.EnumerateFiles(Options.SourceDirectory, "*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
            var input = csproj ?? Options.SourceDirectory;

            var lib = string.Empty;
            if (csproj is null)
            {
                var packageLibs =
                    PackageHelper.GetLibs("War3Api.Common", "*").Concat(
                    PackageHelper.GetLibs("War3Api.Blizzard", "*").Concat(
                    PackageHelper.GetLibs("War3Net.CodeAnalysis.Common", "*")));
                lib = packageLibs.Aggregate((accum, next) => $"{accum};{next}");
            }

            // var compiler = new Compiler(Options.SourceDirectory, scriptFilePath, null, null, null, false, null, exportEnums ? string.Empty : null)
            var compiler = new Compiler(input, Options.OutputDirectory, lib, null, csc, false, null, exportEnums ? string.Empty : null)
            {
                IsExportMetadata = false,
                IsModule = false,
                IsInlineSimpleProperty = false,
                IsPreventDebugObject = preventDebug,
                IsCommentsDisabled = Options.Optimize,
            };

            try
            {
                compiler.CompileSingleFile("war3map", Options.Libraries);
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

        public override void CompileSimple(out string scriptFilePath, params string[] additionalSourceFiles)
        {
            scriptFilePath = Path.Combine(Options.OutputDirectory, "war3map.lua");
            using (var fileStream = File.Create(scriptFilePath))
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