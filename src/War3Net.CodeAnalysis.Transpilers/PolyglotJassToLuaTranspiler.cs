// ------------------------------------------------------------------------------
// <copyright file="PolyglotJassToLuaTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using CSharpLua;
using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    /// <summary>
    /// Special <see cref="JassToLuaTranspiler"/> that can handle lua code embedded in the jass code using //! beginusercode and //! endusercode.
    /// </summary>
    public class PolyglotJassToLuaTranspiler
    {
        private readonly JassToLuaTranspiler _transpiler;
        private readonly LuaRenderer _renderer;
        private readonly TextWriter _writer;

        private bool _isUserCode;
        private JassScriptContext _scriptContext;

        public PolyglotJassToLuaTranspiler(
            JassToLuaTranspiler transpiler,
            LuaSyntaxGenerator.SettingInfo rendererSettings,
            TextWriter writer)
        {
            _transpiler = transpiler;
            _renderer = new LuaRenderer(rendererSettings, writer);
            _writer = writer;

            _isUserCode = false;
            _scriptContext = JassScriptContext.Declarations;
        }

        private enum JassScriptContext
        {
            Declarations,
            Globals,
            Statements,
        }

        public void Transpile(string input)
        {
            using var reader = new StringReader(input);

            while (true)
            {
                var line = reader.ReadLine();
                if (line is null)
                {
                    break;
                }

                if (JassSyntaxFactory.TryParseComment(line, out var comment))
                {
                    if (string.Equals(comment.Comment, "! beginusercode", StringComparison.Ordinal))
                    {
                        if (_isUserCode)
                        {
                            throw new ArgumentException("Invalid: beginusercode", nameof(input));
                        }

                        _isUserCode = true;
                        continue;
                    }
                    else if (string.Equals(comment.Comment, "! endusercode", StringComparison.Ordinal))
                    {
                        if (!_isUserCode)
                        {
                            throw new ArgumentException("Invalid: endusercode", nameof(input));
                        }

                        _isUserCode = false;
                        continue;
                    }
                }

                if (_isUserCode)
                {
                    _writer.WriteLine(line);
                }
                else
                {
                    switch (_scriptContext)
                    {
                        case JassScriptContext.Declarations:
                            Transpile(JassSyntaxFactory.ParseDeclarationLine(line));
                            break;

                        case JassScriptContext.Globals:
                            Transpile(JassSyntaxFactory.ParseGlobalLine(line));
                            break;

                        case JassScriptContext.Statements:
                            Transpile(JassSyntaxFactory.ParseStatementLine(line));
                            break;
                    }
                }
            }
        }

        private void Transpile(IDeclarationLineSyntax declarationLine)
        {
            switch (declarationLine)
            {
                case JassCommentSyntax comment:
                    _renderer.Render((LuaShortCommentStatement)_transpiler.Transpile(comment));
                    break;

                case JassEmptySyntax:
                    if (!_transpiler.IgnoreEmptyDeclarations)
                    {
                        _writer.WriteLine();
                    }

                    break;

                case JassTypeDeclarationSyntax:
                    break;

                case JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration:
                    _transpiler.RegisterFunctionReturnType(nativeFunctionDeclaration.FunctionDeclarator);
                    break;

                case JassGlobalsCustomScriptAction:
                    _scriptContext = JassScriptContext.Globals;
                    break;

                case JassFunctionCustomScriptAction functionCustomScriptAction:
                    _renderer.RenderFunctionDeclarator(_transpiler.Transpile(functionCustomScriptAction.FunctionDeclarator));
                    _scriptContext = JassScriptContext.Statements;
                    break;
            }
        }

        private void Transpile(IGlobalLineSyntax globalLine)
        {
            switch (globalLine)
            {
                case JassCommentSyntax comment:
                    _renderer.Render((LuaShortCommentStatement)_transpiler.Transpile(comment));
                    break;

                case JassEmptySyntax:
                    if (!_transpiler.IgnoreEmptyDeclarations)
                    {
                        _writer.WriteLine();
                    }

                    break;

                case JassGlobalDeclarationSyntax globalDeclaration:
                    _renderer.Render((LuaLocalDeclarationStatementSyntax)_transpiler.Transpile(globalDeclaration));
                    break;

                case JassEndGlobalsCustomScriptAction:
                    _scriptContext = JassScriptContext.Declarations;
                    break;
            }
        }

        private void Transpile(IStatementLineSyntax statementLine)
        {
            switch (statementLine)
            {
                case JassCallStatementSyntax callStatement:
                    _renderer.Render((LuaExpressionStatementSyntax)_transpiler.Transpile(callStatement));
                    break;

                case JassCommentSyntax comment:
                    _renderer.Render((LuaShortCommentStatement)_transpiler.Transpile(comment));
                    break;

                case JassDebugCustomScriptAction:
                    throw new NotSupportedException();

                case JassElseCustomScriptAction:
                    _renderer.RenderElse();
                    break;

                case JassElseIfCustomScriptAction elseIfCustomScriptAction:
                    _renderer.RenderElseIf(_transpiler.Transpile(elseIfCustomScriptAction.Condition, out _));
                    break;

                case JassEmptySyntax:
                    if (!_transpiler.IgnoreEmptyStatements)
                    {
                        _writer.WriteLine();
                    }

                    break;

                case JassEndFunctionCustomScriptAction:
                    _renderer.RenderEnd();
                    _transpiler.ClearLocalTypes();
                    _scriptContext = JassScriptContext.Declarations;
                    break;

                case JassEndIfCustomScriptAction:
                    _renderer.RenderEnd();
                    break;

                case JassEndLoopCustomScriptAction:
                    _renderer.RenderEnd();
                    break;

                case JassExitStatementSyntax exitStatement:
                    _renderer.Render((LuaIfStatementSyntax)_transpiler.Transpile(exitStatement));
                    break;

                case JassIfCustomScriptAction ifCustomScriptAction:
                    _renderer.RenderIf(_transpiler.Transpile(ifCustomScriptAction.Condition, out _));
                    break;

                case JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement:
                    _renderer.Render((LuaLocalDeclarationStatementSyntax)_transpiler.Transpile(localVariableDeclarationStatement));
                    break;

                case JassLoopCustomScriptAction:
                    _renderer.RenderLoop();
                    break;

                case JassReturnStatementSyntax returnStatement:
                    _renderer.Render((LuaReturnStatementSyntax)_transpiler.Transpile(returnStatement));
                    break;

                case JassSetStatementSyntax setStatement:
                    _renderer.Render((LuaExpressionStatementSyntax)_transpiler.Transpile(setStatement));
                    break;
            }
        }
    }
}