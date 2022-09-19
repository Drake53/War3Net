// ------------------------------------------------------------------------------
// <copyright file="MapExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CSharpLua;

using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Transpilers;
using War3Net.Common.Providers;

namespace War3Net.Build.Extensions
{
    public static class MapExtensions
    {
        private static readonly Encoding _defaultEncoding = UTF8EncodingProvider.StrictUTF8;

        public static void CompileScript(this Map map)
        {
            var mapScriptBuilder = new MapScriptBuilder();
            mapScriptBuilder.SetDefaultOptionsForMap(map);

            map.CompileScript(mapScriptBuilder);
        }

        public static void CompileScript(this Map map, MapScriptBuilder mapScriptBuilder)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (mapScriptBuilder is null)
            {
                throw new ArgumentNullException(nameof(mapScriptBuilder));
            }

            if (map.Info is null)
            {
                throw new ArgumentException($"The map must contain the '{MapInfo.FileName}' file in order to use the jass compiler.", nameof(map));
            }

            if (map.Info.ScriptLanguage != ScriptLanguage.Jass)
            {
                throw new ArgumentException($"The map's script language must be set to jass in order to use the jass compiler.", nameof(map));
            }

            var compilationUnit = mapScriptBuilder.Build(map);

            using var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, _defaultEncoding, leaveOpen: true))
            {
                var renderer = new JassRenderer(writer);
                renderer.Render(compilationUnit);
            }

            stream.Position = 0;
            map.SetScriptFile(stream);
        }

        public static CompileResult CompileScript(this Map map, Compiler compiler, IEnumerable<string> luaSystemLibs)
        {
            var jassHelperFolder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Warcraft III", "JassHelper");
            return map.CompileScript(compiler, luaSystemLibs, Path.Combine(jassHelperFolder, "common.j"), Path.Combine(jassHelperFolder, "Blizzard.j"));
        }

        public static CompileResult CompileScript(this Map map, Compiler compiler, MapScriptBuilder mapScriptBuilder, IEnumerable<string> luaSystemLibs)
        {
            var jassHelperFolder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Warcraft III", "JassHelper");
            return map.CompileScript(compiler, mapScriptBuilder, luaSystemLibs, Path.Combine(jassHelperFolder, "common.j"), Path.Combine(jassHelperFolder, "Blizzard.j"));
        }

        public static CompileResult CompileScript(this Map map, Compiler compiler, IEnumerable<string> luaSystemLibs, string commonJPath, string blizzardJPath)
        {
            var mapScriptBuilder = new MapScriptBuilder();
            mapScriptBuilder.SetDefaultOptionsForCSharpLua();

            return map.CompileScript(compiler, mapScriptBuilder, luaSystemLibs, commonJPath, blizzardJPath);
        }

        public static CompileResult CompileScript(this Map map, Compiler compiler, MapScriptBuilder mapScriptBuilder, IEnumerable<string> luaSystemLibs, string commonJPath, string blizzardJPath)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (compiler is null)
            {
                throw new ArgumentNullException(nameof(compiler));
            }

            if (mapScriptBuilder is null)
            {
                throw new ArgumentNullException(nameof(mapScriptBuilder));
            }

            if (map.Info is null)
            {
                throw new ArgumentException($"The map must contain the '{MapInfo.FileName}' file in order to use the C# compiler.", nameof(map));
            }

            if (map.Info.ScriptLanguage != ScriptLanguage.Lua)
            {
                throw new ArgumentException($"The map's script language must be set to lua in order to use the C# compiler.", nameof(map));
            }

            using var stream = new MemoryStream();

            try
            {
                compiler.CompileSingleFile(stream, luaSystemLibs);
            }
            catch (CompilationErrorException e)
            {
                return new CompileResult(e.EmitResult);
            }

            var transpiler = new JassToLuaTranspiler();
            transpiler.IgnoreComments = true;
            transpiler.IgnoreEmptyDeclarations = true;
            transpiler.IgnoreEmptyStatements = true;
            transpiler.KeepFunctionsSeparated = true;

            transpiler.RegisterJassFile(JassSyntaxFactory.ParseCompilationUnit(File.ReadAllText(commonJPath)));
            transpiler.RegisterJassFile(JassSyntaxFactory.ParseCompilationUnit(File.ReadAllText(blizzardJPath)));

            var luaCompilationUnit = transpiler.Transpile(mapScriptBuilder.Build(map));
            using (var writer = new StreamWriter(stream, _defaultEncoding, leaveOpen: true))
            {
                var luaRenderOptions = new LuaSyntaxGenerator.SettingInfo
                {
                    Indent = 4,
                };

                var luaRenderer = new LuaRenderer(luaRenderOptions, writer);
                luaRenderer.RenderCompilationUnit(luaCompilationUnit);
            }

            stream.Position = 0;
            map.SetScriptFile(stream);

            return new CompileResult(true, null);
        }
    }
}