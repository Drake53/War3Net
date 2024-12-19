// ------------------------------------------------------------------------------
// 
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

using War3Net.Build.Environment;
using War3Net.Build;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.Build.Audio;
using War3Net.Build.Widget;
using War3Net.Build.Info;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        [AttributeUsage(AttributeTargets.Method)]
        private class RegisterStatementParserAttribute : Attribute
        {
        }

        internal class StatementParserInput
        {
            public IStatementLineSyntax Statement;
            public List<IJassSyntaxToken> StatementChildren;
        }

        private static List<MethodInfo> _statementParsers;
        static JassScriptDecompiler()
        {
            _statementParsers = typeof(JassScriptDecompiler)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.GetCustomAttributes(typeof(RegisterStatementParserAttribute), false).Any()).ToList();
        }

        public DecompilationContext Context { get; }

        public JassScriptDecompiler(JassCompilationUnitSyntax compilationUnit, DecompileOptions options = null, MapInfo mapInfo = null)
        {
            Context = new DecompilationContext(compilationUnit, options, mapInfo);
        }

        private void ProcessStatementParsers(IStatementLineSyntax statement, IEnumerable<Action<StatementParserInput>> statementParsers)
        {
            var input = new StatementParserInput() { Statement = statement, StatementChildren = statement.GetChildren_RecursiveDepthFirst().ToList() };
            foreach (var parser in statementParsers)
            {
                try
                {
                    parser(input);
                    if (Context.HandledStatements.Contains(statement))
                    {
                        break;
                    }
                }
                catch
                {
                    //swallow exceptions
                }
            }
        }

        private FunctionDeclarationContext? GetFunction(string functionName)
        {
            if (Context.FunctionDeclarations.TryGetValue(functionName, out var functionDeclaration))
            {
                return functionDeclaration;
            }

            return null;
        }

        public List<IStatementLineSyntax> GetFunctionStatements_EnteringCalls(string startingFunctionName)
        {
            var functionDeclaration = Context.FunctionDeclarations.GetValueOrDefault(startingFunctionName);
            if (functionDeclaration == null || functionDeclaration.Handled)
            {
                return new List<IStatementLineSyntax>();
            }

            functionDeclaration.Handled = true;
            var result = new List<IStatementLineSyntax>();
            foreach (var child in functionDeclaration.FunctionDeclaration.GetChildren_RecursiveDepthFirst())
            {
                if (child is IStatementLineSyntax statement)
                {
                    result.Add(statement);
                }

                if (child is JassFunctionReferenceExpressionSyntax functionReference)
                {
                    result.AddRange(GetFunctionStatements_EnteringCalls(functionReference.IdentifierName.Name));
                }
                else if (child is JassCallStatementSyntax callStatement)
                {
                    if (string.Equals(callStatement.IdentifierName.Name, "ExecuteFunc", StringComparison.InvariantCultureIgnoreCase) && callStatement.Arguments.Arguments.FirstOrDefault() is JassStringLiteralExpressionSyntax execFunctionName)
                    {
                        result.AddRange(GetFunctionStatements_EnteringCalls(execFunctionName.Value));
                    }
                    else
                    {
                        result.AddRange(GetFunctionStatements_EnteringCalls(callStatement.IdentifierName.Name));
                    }
                }
            }

            return result.ToList();
        }

        public Map DecompileObjectManagerData()
        {
            if (Context.MapInfo?.ScriptLanguage == ScriptLanguage.Lua)
            {
                throw new Exception("Lua decompilation is not supported yet");
            }

            //todo: Run through Jass=>Lua transpiler, then run with NLua with native polyfills & method execution logging to better handle obfuscated code & to re-use the same logic for both Jass & Lua Decompilation (still need parser to direct interpreter what functions to execute for non-iterative patterns like TriggerRegisterUnitEvent for unit death item drop tables).

            var actions = _statementParsers.Select(parser => (Action<StatementParserInput>)((StatementParserInput input) => parser.Invoke(this, new[] { input }))).ToList();

            foreach (var function in Context.FunctionDeclarations)
            {
                function.Value.Handled = false;
            }
            var statements = GetFunctionStatements_EnteringCalls("config").Concat(GetFunctionStatements_EnteringCalls("main")).ToList();
            foreach (var statement in statements)
            {
                ProcessStatementParsers(statement, actions);
            }

            var map = new Map() { Info = Context.MapInfo };
            map.Cameras = new MapCameras(Context.Options.mapCamerasFormatVersion, Context.Options.mapCamerasUseNewFormat) { Cameras = Context.GetAll<Camera>().ToList() };
            map.Regions = new MapRegions(Context.Options.mapRegionsFormatVersion) { Regions = Context.GetAll<Region>().ToList() };
            map.Sounds = new MapSounds(Context.Options.mapSoundsFormatVersion) { Sounds = Context.GetAll<Sound>().ToList() };
            map.Units = new MapUnits(Context.Options.mapWidgetsFormatVersion, Context.Options.mapWidgetsSubVersion, Context.Options.mapWidgetsUseNewFormat) { Units = Context.GetAll<UnitData>().ToList() };
            map.Doodads = new MapDoodads(Context.Options.mapWidgetsFormatVersion, Context.Options.mapWidgetsSubVersion, Context.Options.mapWidgetsUseNewFormat) { Doodads = Context.GetAll<DoodadData>().ToList(), SpecialDoodads = Context.GetAll<SpecialDoodadData>().ToList(), SpecialDoodadVersion = Context.Options.specialDoodadVersion };
            return map;
        }
    }
}