// ------------------------------------------------------------------------------
// <copyright file="JassRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        private readonly TextWriter _writer;
        private readonly JassRendererOptions _options;

        private int _currentIndentation;
        private bool _currentLineIndented;

        public JassRenderer(TextWriter writer)
        {
            _writer = writer;
            _options = JassRendererOptions.Default;
        }

        public JassRenderer(TextWriter writer, JassRendererOptions options)
        {
            _writer = writer;
            _options = options;
        }

        public void RenderLine(string value) => _writer.WriteLine(value);

        public void RenderNewLine() => WriteLine();

        private void Write(char c)
        {
            if (!_currentLineIndented)
            {
                WriteIndentation();
            }

            _writer.Write(c);
        }

        private void Write(string s)
        {
            if (!_currentLineIndented)
            {
                WriteIndentation();
            }

            _writer.Write(s);
        }

        private void WriteLine()
        {
            _writer.Write(_options.NewLineString);
            _currentLineIndented = false;
        }

        private void WriteLine(string s)
        {
            Write(s);
            WriteLine();
        }

        private void WriteIndentation()
        {
            _currentLineIndented = true;
            _writer.Write(string.Concat(Enumerable.Repeat(_options.IndentationString, _currentIndentation)));
        }

        private void Indent()
        {
            _currentIndentation++;
        }

        private void Outdent()
        {
            _currentIndentation--;
        }

        public void Render(IJassSyntaxToken syntaxToken)
        {
            switch (syntaxToken)
            {
                case JassArgumentListSyntax token:
                    Render(token);
                    return;
                case JassArrayDeclaratorSyntax token:
                    Render(token);
                    return;
                case JassArrayReferenceExpressionSyntax token:
                    Render(token);
                    return;
                case JassBinaryExpressionSyntax token:
                    Render(token);
                    return;
                case JassBooleanLiteralExpressionSyntax token:
                    Render(token);
                    return;
                case JassCallStatementSyntax token:
                    Render(token);
                    return;
                case JassCharacterLiteralExpressionSyntax token:
                    Render(token);
                    return;
                case JassCommentSyntax token:
                    Render(token);
                    return;
                case JassCompilationUnitSyntax token:
                    Render(token);
                    return;
                case JassDebugCustomScriptAction token:
                    Render(token);
                    return;
                case JassDebugStatementSyntax token:
                    Render(token);
                    return;
                case JassDecimalLiteralExpressionSyntax token:
                    Render(token);
                    return;
                case JassElseClauseSyntax token:
                    Render(token);
                    return;
                case JassElseCustomScriptAction token:
                    Render(token);
                    return;
                case JassElseIfClauseSyntax token:
                    Render(token);
                    return;
                case JassElseIfCustomScriptAction token:
                    Render(token);
                    return;
                case JassEmptySyntax token:
                    Render(token);
                    return;
                case JassEndFunctionCustomScriptAction token:
                    Render(token);
                    return;
                case JassEndGlobalsCustomScriptAction token:
                    Render(token);
                    return;
                case JassEndIfCustomScriptAction token:
                    Render(token);
                    return;
                case JassEndLoopCustomScriptAction token:
                    Render(token);
                    return;
                case JassEqualsValueClauseSyntax token:
                    Render(token);
                    return;
                case JassExitStatementSyntax token:
                    Render(token);
                    return;
                case JassFourCCLiteralExpressionSyntax token:
                    Render(token);
                    return;
                case JassFunctionCustomScriptAction token:
                    Render(token);
                    return;
                case JassFunctionDeclarationSyntax token:
                    Render(token);
                    return;
                case JassFunctionDeclaratorSyntax token:
                    Render(token);
                    return;
                case JassFunctionReferenceExpressionSyntax token:
                    Render(token);
                    return;
                case JassGlobalDeclarationListSyntax token:
                    Render(token);
                    return;
                case JassGlobalDeclarationSyntax token:
                    Render(token);
                    return;
                case JassGlobalsCustomScriptAction token:
                    Render(token);
                    return;
                case JassHexadecimalLiteralExpressionSyntax token:
                    Render(token);
                    return;
                case JassIdentifierNameSyntax token:
                    Render(token);
                    return;
                case JassIfCustomScriptAction token:
                    Render(token);
                    return;
                case JassIfStatementSyntax token:
                    Render(token);
                    return;
                case JassInvocationExpressionSyntax token:
                    Render(token);
                    return;
                case JassLocalVariableDeclarationStatementSyntax token:
                    Render(token);
                    return;
                case JassLoopCustomScriptAction token:
                    Render(token);
                    return;
                case JassLoopStatementSyntax token:
                    Render(token);
                    return;
                case JassNativeFunctionDeclarationSyntax token:
                    Render(token);
                    return;
                case JassNullLiteralExpressionSyntax token:
                    Render(token);
                    return;
                case JassOctalLiteralExpressionSyntax token:
                    Render(token);
                    return;
                case JassParameterListSyntax token:
                    Render(token);
                    return;
                case JassParameterSyntax token:
                    Render(token);
                    return;
                case JassParenthesizedExpressionSyntax token:
                    Render(token);
                    return;
                case JassRealLiteralExpressionSyntax token:
                    Render(token);
                    return;
                case JassReturnStatementSyntax token:
                    Render(token);
                    return;
                case JassSetStatementSyntax token:
                    Render(token);
                    return;
                case JassStatementListSyntax token:
                    Render(token);
                    return;
                case JassStringLiteralExpressionSyntax token:
                    Render(token);
                    return;
                case JassTypeDeclarationSyntax token:
                    Render(token);
                    return;
                case JassTypeSyntax token:
                    Render(token);
                    return;
                case JassUnaryExpressionSyntax token:
                    Render(token);
                    return;
                case JassVariableDeclaratorSyntax token:
                    Render(token);
                    return;
                case JassVariableReferenceExpressionSyntax token:
                    Render(token);
                    return;
            }
        }
    }
}