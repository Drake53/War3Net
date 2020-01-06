﻿// ------------------------------------------------------------------------------
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
            _rendererOptions = rendererOptions;
        }

        public override void BuildAutogeneratedCode(out string globalsFilePath, out string mainFunctionFilePath, out string configFunctionFilePath)
        {
            var functionBuilderData = new FunctionBuilderData(Options.MapInfo, Options.MapDoodads, Options.MapUnits, Options.MapRegions, Options.LobbyMusic, Options.SourceDirectory != null);
            var functionBuilder = new LuaFunctionBuilder(functionBuilderData);

            globalsFilePath = Path.Combine(Options.OutputDirectory, "globals.lua");
            var globals = new LuaVariableListDeclarationSyntax[Options.MapInfo.RandomUnitTableCount];
            for (var i = 0; i < globals.Length; i++)
            {
                globals[i] = new LuaVariableListDeclarationSyntax();
                globals[i].Variables.Add(new LuaVariableDeclaratorSyntax(
                    $"gg_rg_{Options.MapInfo.GetUnitTable(i).Index.ToString("D3")}",
                    functionBuilder.GenerateInvocationExpression(
                        "__jarray",
                        functionBuilder.GenerateIntegerLiteralExpression(0))));
            }

            RenderToFile(globalsFilePath, globals);

            mainFunctionFilePath = Path.Combine(Options.OutputDirectory, "main.lua");
            RenderToFile(mainFunctionFilePath, functionBuilder.BuildMainFunction());

            configFunctionFilePath = Path.Combine(Options.OutputDirectory, "config.lua");
            RenderToFile(configFunctionFilePath, functionBuilder.BuildConfigFunction());
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
                using (var writer = new StreamWriter(fileStream, new UTF8Encoding(false, true), 1024, true))
                {
                    foreach (var additionalSourceFile in additionalSourceFiles)
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
            using (var fileStream = FileProvider.OpenNewWrite(scriptFilePath))
            {
                using (var writer = new StreamWriter(fileStream, new UTF8Encoding(false, true), 1024, true))
                {
                    foreach (var additionalSourceFile in additionalSourceFiles)
                    {
                        writer.Write(File.ReadAllText(additionalSourceFile));
                        writer.WriteLine();
                    }
                }
            }
        }

        private void RenderToFile(string path, IEnumerable<LuaVariableListDeclarationSyntax> functions)
        {
            using (var fileStream = FileProvider.OpenNewWrite(path))
            {
                using (var writer = new StreamWriter(fileStream, new UTF8Encoding(false, true)))
                {
                    var renderer = new LuaRenderer(_rendererOptions, writer);

                    var compilationUnitSyntax = new LuaCompilationUnitSyntax();
                    foreach (var function in functions)
                    {
                        compilationUnitSyntax.AddStatement(function);
                    }

                    renderer.RenderCompilationUnit(compilationUnitSyntax);
                }
            }
        }
    }
}