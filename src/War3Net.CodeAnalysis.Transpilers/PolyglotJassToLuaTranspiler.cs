using System;
using System.IO;
using CSharpLua;
using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    /// <summary>
    /// Special <see cref="JassToLuaTranspiler"/> that can handle lua code embedded in the jass code using <c>//! beginusercode</c> and <c>//! endusercode</c>.
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
            _scriptContext = JassScriptContext.TopLevelDeclarations;
        }

        private enum JassScriptContext
        {
            TopLevelDeclarations,
            GlobalsBlock,
            FunctionBody,
        }

        public void Transpile(string input)
        {
            using var reader = new StringReader(input);

            var lineNumber = 0;
            while (true)
            {
                lineNumber++;
                var line = reader.ReadLine();
                if (line is null)
                {
                    break;
                }

                var trimmed = line.AsSpan().TrimStart();
                if (trimmed.StartsWith("//"))
                {
                    var comment = trimmed.TrimEnd();

                    if (comment.Equals("//! beginusercode", StringComparison.Ordinal))
                    {
                        if (_isUserCode)
                        {
                            throw new ArgumentException("Unexpected //! beginusercode", nameof(input));
                        }

                        _isUserCode = true;
                        continue;
                    }
                    else if (comment.Equals("//! endusercode", StringComparison.Ordinal))
                    {
                        if (!_isUserCode)
                        {
                            throw new ArgumentException("Unexpected //! endusercode", nameof(input));
                        }

                        _isUserCode = false;
                        continue;
                    }
                    else if (_isUserCode)
                    {
                        _writer.WriteLine(line);
                    }
                    else if (!_transpiler.IgnoreComments)
                    {
                        _renderer.Render(new LuaShortCommentStatement(trimmed[2..].ToString()));
                    }
                }
                else if (_isUserCode)
                {
                    _writer.WriteLine(line);
                }
                else if (JassSyntaxFactory.TryParseScriptLine(line, out var scriptLine))
                {
                    try
                    {
                        Transpile(scriptLine.Value);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException($"Failed to transpile JASS on line {lineNumber}: {line}", nameof(input), ex);
                    }
                }
                else
                {
                    throw new ArgumentException($"Invalid JASS on line {lineNumber}: {line}", nameof(input));
                }
            }
        }

        private void Transpile(JassSyntaxNodeOrToken scriptLine)
        {
            if (scriptLine.TryPickToken(out var token, out var node))
            {
                Transpile(token);
            }
            else
            {
                Transpile(node);
            }
        }

        private void Transpile(JassSyntaxToken token)
        {
            switch (token.SyntaxKind)
            {
                case JassSyntaxKind.ElseKeyword:
                    _renderer.RenderElse();
                    break;

                case JassSyntaxKind.EndFunctionKeyword:
                    _renderer.RenderEnd();
                    _transpiler.ClearLocalTypes();
                    _scriptContext = JassScriptContext.TopLevelDeclarations;
                    break;

                case JassSyntaxKind.EndGlobalsKeyword:
                    _scriptContext = JassScriptContext.TopLevelDeclarations;
                    break;

                case JassSyntaxKind.EndIfKeyword:
                    _renderer.RenderEnd();
                    break;

                case JassSyntaxKind.EndLoopKeyword:
                    _renderer.RenderEnd();
                    break;

                case JassSyntaxKind.GlobalsKeyword:
                    _scriptContext = JassScriptContext.GlobalsBlock;
                    break;

                case JassSyntaxKind.LoopKeyword:
                    _renderer.RenderLoop();
                    break;
            }
        }

        private void Transpile(JassSyntaxNode node)
        {
            switch (node)
            {
                case JassGlobalDeclarationSyntax globalDeclaration:
                    _renderer.Render((LuaLocalDeclarationStatementSyntax)_transpiler.Transpile(globalDeclaration));
                    break;

                case JassCallStatementSyntax callStatement:
                    _renderer.Render((LuaExpressionStatementSyntax)_transpiler.Transpile(callStatement));
                    break;

                case JassDebugStatementSyntax:
                    throw new NotSupportedException();

                case JassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator:
                    _renderer.RenderElseIf(_transpiler.Transpile(elseIfClauseDeclarator.Condition, out _));
                    break;

                case JassExitStatementSyntax exitStatement:
                    _renderer.Render((LuaIfStatementSyntax)_transpiler.Transpile(exitStatement));
                    break;

                case JassFunctionDeclaratorSyntax functionDeclarator:
                    _renderer.RenderFunctionDeclarator(_transpiler.Transpile(functionDeclarator));
                    _scriptContext = JassScriptContext.FunctionBody;
                    break;

                case JassIfClauseDeclaratorSyntax ifClauseDeclarator:
                    _renderer.RenderIf(_transpiler.Transpile(ifClauseDeclarator.Condition, out _));
                    break;

                case JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement:
                    _renderer.Render((LuaLocalDeclarationStatementSyntax)_transpiler.Transpile(localVariableDeclarationStatement));
                    break;

                case JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration:
                    _transpiler.RegisterFunctionReturnType(nativeFunctionDeclaration);
                    break;

                case JassReturnStatementSyntax returnStatement:
                    _renderer.Render((LuaReturnStatementSyntax)_transpiler.Transpile(returnStatement));
                    break;

                case JassSetStatementSyntax setStatement:
                    _renderer.Render((LuaExpressionStatementSyntax)_transpiler.Transpile(setStatement));
                    break;

                case JassTypeDeclarationSyntax:
                    break;
            }
        }
    }
}