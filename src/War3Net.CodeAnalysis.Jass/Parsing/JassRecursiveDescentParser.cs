// ------------------------------------------------------------------------------
// <copyright file="JassRecursiveDescentParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Text;

using War3Net.CodeAnalysis.Diagnostics;
using War3Net.CodeAnalysis.Jass.Diagnostics;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Text;

namespace War3Net.CodeAnalysis.Jass.Parsing
{
    internal sealed partial class JassRecursiveDescentParser
    {
        private readonly ImmutableArray<JassSyntaxToken> _tokens;
        private readonly ImmutableArray<int> _tokenOffsets;
        private readonly DiagnosticBag _diagnostics;
        private readonly string? _filePath;
        private int _position;
        private JassSyntaxTrivia? _pendingSkippedTrivia;

        internal JassRecursiveDescentParser(
            ImmutableArray<JassSyntaxToken> tokens,
            ImmutableArray<int> tokenOffsets,
            DiagnosticBag diagnostics,
            string? filePath)
        {
            _tokens = tokens;
            _tokenOffsets = tokenOffsets;
            _diagnostics = diagnostics;
            _filePath = filePath;
        }

        private JassSyntaxToken Current => _tokens[Math.Min(_position, _tokens.Length - 1)];

        private JassSyntaxToken Peek(int offset) => _tokens[Math.Min(_position + offset, _tokens.Length - 1)];

        private bool AtEnd => Current.SyntaxKind == JassSyntaxKind.EndOfFileToken;

        private bool At(JassSyntaxKind kind) => Current.SyntaxKind == kind;

        private JassSyntaxToken EatToken()
        {
            var token = Current;
            _position++;

            if (_pendingSkippedTrivia is not null)
            {
                token = PrependSkippedTrivia(token, _pendingSkippedTrivia);
                _pendingSkippedTrivia = null;
            }

            return token;
        }

        private JassSyntaxToken EatToken(JassSyntaxKind expected)
        {
            if (Current.SyntaxKind == expected)
            {
                return EatToken();
            }

            return CreateMissingToken(expected);
        }

        private JassSyntaxToken CreateMissingToken(JassSyntaxKind expected)
        {
            var location = GetCurrentLocation();

            if (AtEnd)
            {
                _diagnostics.Report(
                    JassSyntaxDiagnostics.UnexpectedEndOfFile,
                    location,
                    JassSyntaxFacts.GetText(expected));
            }
            else
            {
                _diagnostics.Report(
                    JassSyntaxDiagnostics.UnexpectedToken,
                    location,
                    Current.Text,
                    $"'{JassSyntaxFacts.GetText(expected)}'");
            }

            var token = new JassSyntaxToken(
                JassSyntaxTriviaList.Empty,
                expected,
                string.Empty,
                JassSyntaxTriviaList.Empty,
                isMissing: true);

            if (_pendingSkippedTrivia is not null)
            {
                token = PrependSkippedTrivia(token, _pendingSkippedTrivia);
                _pendingSkippedTrivia = null;
            }

            return token;
        }

        private Location GetCurrentLocation()
        {
            var index = Math.Min(_position, _tokenOffsets.Length - 1);
            var offset = _tokenOffsets[index];
            var length = Current.Text.Length;
            return Location.Create(new TextSpan(offset, length), _filePath);
        }

        private static JassSyntaxToken PrependSkippedTrivia(JassSyntaxToken token, JassSyntaxTrivia skippedTrivia)
        {
            var existingTrivia = token.LeadingTrivia.Trivia;
            var newTrivia = ImmutableArray.CreateBuilder<JassSyntaxTrivia>(existingTrivia.Length + 1);
            newTrivia.Add(skippedTrivia);
            newTrivia.AddRange(existingTrivia);
            return token.WithLeadingTrivia(new JassSyntaxTriviaList(newTrivia.MoveToImmutable()));
        }

        private void SkipToKeywordSync()
        {
            var sb = new StringBuilder();

            while (!AtEnd && !IsStatementOrBlockKeyword(Current.SyntaxKind) && !IsTopLevelKeyword(Current.SyntaxKind))
            {
                var token = Current;
                _position++;
                sb.Append(token.ToFullString());
            }

            if (sb.Length > 0)
            {
                var skipped = new JassSyntaxTrivia(JassSyntaxKind.SkippedTokensTrivia, sb.ToString());
                if (_pendingSkippedTrivia is not null)
                {
                    _pendingSkippedTrivia = new JassSyntaxTrivia(
                        JassSyntaxKind.SkippedTokensTrivia,
                        _pendingSkippedTrivia.Text + skipped.Text);
                }
                else
                {
                    _pendingSkippedTrivia = skipped;
                }
            }
        }

        private void SkipToTopLevelKeywordSync()
        {
            var sb = new StringBuilder();

            while (!AtEnd && !IsTopLevelKeyword(Current.SyntaxKind))
            {
                var token = Current;
                _position++;
                sb.Append(token.ToFullString());
            }

            if (sb.Length > 0)
            {
                var skipped = new JassSyntaxTrivia(JassSyntaxKind.SkippedTokensTrivia, sb.ToString());
                if (_pendingSkippedTrivia is not null)
                {
                    _pendingSkippedTrivia = new JassSyntaxTrivia(
                        JassSyntaxKind.SkippedTokensTrivia,
                        _pendingSkippedTrivia.Text + skipped.Text);
                }
                else
                {
                    _pendingSkippedTrivia = skipped;
                }
            }
        }

        private static bool IsTopLevelKeyword(JassSyntaxKind kind)
        {
            return kind is JassSyntaxKind.FunctionKeyword
                or JassSyntaxKind.NativeKeyword
                or JassSyntaxKind.TypeKeyword
                or JassSyntaxKind.GlobalsKeyword
                or JassSyntaxKind.ConstantKeyword;
        }

        private bool IsTopLevelKeywordOutsideGlobals(JassSyntaxKind kind)
        {
            if (kind == JassSyntaxKind.ConstantKeyword)
            {
                // 'constant' inside globals is a global constant declaration,
                // only break out if followed by 'function' or 'native'.
                return Peek(1).SyntaxKind is JassSyntaxKind.FunctionKeyword or JassSyntaxKind.NativeKeyword;
            }

            return IsTopLevelKeyword(kind);
        }

        private static bool IsStatementKeyword(JassSyntaxKind kind)
        {
            return kind is JassSyntaxKind.SetKeyword
                or JassSyntaxKind.CallKeyword
                or JassSyntaxKind.LocalKeyword
                or JassSyntaxKind.ReturnKeyword
                or JassSyntaxKind.ExitWhenKeyword
                or JassSyntaxKind.IfKeyword
                or JassSyntaxKind.LoopKeyword
                or JassSyntaxKind.DebugKeyword;
        }

        private static bool IsBlockEndKeyword(JassSyntaxKind kind)
        {
            return kind is JassSyntaxKind.EndFunctionKeyword
                or JassSyntaxKind.EndIfKeyword
                or JassSyntaxKind.EndLoopKeyword
                or JassSyntaxKind.EndGlobalsKeyword
                or JassSyntaxKind.ElseIfKeyword
                or JassSyntaxKind.ElseKeyword;
        }

        private static bool IsStatementOrBlockKeyword(JassSyntaxKind kind)
        {
            return IsStatementKeyword(kind) || IsBlockEndKeyword(kind);
        }

        // === Top-level parsing ===

        internal JassCompilationUnitSyntax ParseCompilationUnit()
        {
            var declarations = ImmutableArray.CreateBuilder<JassTopLevelDeclarationSyntax>(64);

            while (!AtEnd)
            {
                var declaration = ParseTopLevelDeclaration();
                if (declaration is not null)
                {
                    declarations.Add(declaration);
                }
                else
                {
                    _diagnostics.Report(
                        JassSyntaxDiagnostics.InvalidStatementLocation,
                        GetCurrentLocation());
                    SkipToTopLevelKeywordSync();
                }
            }

            var endOfFileToken = EatToken(JassSyntaxKind.EndOfFileToken);
            declarations.Capacity = declarations.Count;
            return new JassCompilationUnitSyntax(declarations.MoveToImmutable(), endOfFileToken);
        }

        private JassTopLevelDeclarationSyntax? ParseTopLevelDeclaration()
        {
            return Current.SyntaxKind switch
            {
                JassSyntaxKind.TypeKeyword => ParseTypeDeclaration(),
                JassSyntaxKind.GlobalsKeyword => ParseGlobalsDeclaration(),
                JassSyntaxKind.NativeKeyword => ParseNativeFunctionDeclaration(constantToken: null),
                JassSyntaxKind.FunctionKeyword => ParseFunctionDeclaration(constantToken: null),
                JassSyntaxKind.ConstantKeyword => ParseConstantTopLevel(),
                _ => null,
            };
        }

        private JassTopLevelDeclarationSyntax ParseConstantTopLevel()
        {
            var constantToken = EatToken(JassSyntaxKind.ConstantKeyword);

            if (At(JassSyntaxKind.NativeKeyword))
            {
                return ParseNativeFunctionDeclaration(constantToken);
            }

            return ParseFunctionDeclaration(constantToken);
        }

        private JassTypeDeclarationSyntax ParseTypeDeclaration()
        {
            var typeToken = EatToken(JassSyntaxKind.TypeKeyword);
            var identifierName = ParseIdentifierName();
            var extendsToken = EatToken(JassSyntaxKind.ExtendsKeyword);
            var baseType = ParseTypeName();

            return new JassTypeDeclarationSyntax(typeToken, identifierName, extendsToken, baseType);
        }

        private JassGlobalsDeclarationSyntax ParseGlobalsDeclaration()
        {
            var globalsToken = EatToken(JassSyntaxKind.GlobalsKeyword);
            var declarations = ImmutableArray.CreateBuilder<JassGlobalDeclarationSyntax>(32);

            while (!AtEnd
                && !At(JassSyntaxKind.EndGlobalsKeyword)
                && !IsTopLevelKeywordOutsideGlobals(Current.SyntaxKind))
            {
                var declaration = ParseGlobalDeclaration();
                if (declaration is not null)
                {
                    declarations.Add(declaration);
                }
                else
                {
                    _diagnostics.Report(
                        JassSyntaxDiagnostics.SyntaxError,
                        GetCurrentLocation(),
                        $"Unexpected token '{Current.Text}' in globals block");

                    var positionBefore = _position;
                    SkipToKeywordSync();

                    if (_position == positionBefore)
                    {
                        _position++;
                    }
                }
            }

            JassSyntaxToken endGlobalsToken;
            if (At(JassSyntaxKind.EndGlobalsKeyword))
            {
                endGlobalsToken = EatToken();
            }
            else
            {
                _diagnostics.Report(
                    JassSyntaxDiagnostics.MissingEndGlobals,
                    GetCurrentLocation());
                endGlobalsToken = CreateMissingTokenSilent(JassSyntaxKind.EndGlobalsKeyword);
            }

            return new JassGlobalsDeclarationSyntax(globalsToken, declarations.ToImmutable(), endGlobalsToken);
        }

        private JassGlobalDeclarationSyntax? ParseGlobalDeclaration()
        {
            if (At(JassSyntaxKind.ConstantKeyword))
            {
                return ParseGlobalConstantDeclaration();
            }

            var type = TryParseTypeName();
            if (type is null)
            {
                return null;
            }

            if (At(JassSyntaxKind.ArrayKeyword))
            {
                var arrayToken = EatToken();
                var identifierName = ParseIdentifierName();
                var declarator = new JassArrayDeclaratorSyntax(type, arrayToken, identifierName);
                return new JassGlobalVariableDeclarationSyntax(declarator);
            }
            else
            {
                var identifierName = ParseIdentifierName();
                JassEqualsValueClauseSyntax? equalsValueClause = null;
                if (At(JassSyntaxKind.EqualsToken))
                {
                    equalsValueClause = ParseEqualsValueClause();
                }

                var declarator = new JassVariableDeclaratorSyntax(type, identifierName, equalsValueClause);
                return new JassGlobalVariableDeclarationSyntax(declarator);
            }
        }

        private JassGlobalConstantDeclarationSyntax ParseGlobalConstantDeclaration()
        {
            var constantToken = EatToken(JassSyntaxKind.ConstantKeyword);
            var type = ParseTypeName();
            var identifierName = ParseIdentifierName();
            var equalsValueClause = ParseEqualsValueClause();

            return new JassGlobalConstantDeclarationSyntax(constantToken, type, identifierName, equalsValueClause);
        }

        private JassNativeFunctionDeclarationSyntax ParseNativeFunctionDeclaration(JassSyntaxToken? constantToken)
        {
            var nativeToken = EatToken(JassSyntaxKind.NativeKeyword);
            var identifierName = ParseIdentifierName();
            var parameterList = ParseParameterListOrEmpty();
            var returnClause = ParseReturnClause();

            return new JassNativeFunctionDeclarationSyntax(constantToken, nativeToken, identifierName, parameterList, returnClause);
        }

        private JassFunctionDeclarationSyntax ParseFunctionDeclaration(JassSyntaxToken? constantToken)
        {
            var functionToken = EatToken(JassSyntaxKind.FunctionKeyword);
            var identifierName = ParseIdentifierName();
            var parameterList = ParseParameterListOrEmpty();
            var returnClause = ParseReturnClause();

            var declarator = new JassFunctionDeclaratorSyntax(constantToken, functionToken, identifierName, parameterList, returnClause);

            var statements = ParseFunctionBodyStatements();

            JassSyntaxToken endFunctionToken;
            if (At(JassSyntaxKind.EndFunctionKeyword))
            {
                endFunctionToken = EatToken();
            }
            else
            {
                _diagnostics.Report(
                    JassSyntaxDiagnostics.MissingEndFunction,
                    GetCurrentLocation(),
                    identifierName.Token.Text);
                endFunctionToken = CreateMissingTokenSilent(JassSyntaxKind.EndFunctionKeyword);
            }

            return new JassFunctionDeclarationSyntax(declarator, statements, endFunctionToken);
        }

        private ImmutableArray<JassStatementSyntax> ParseFunctionBodyStatements()
        {
            var statements = ImmutableArray.CreateBuilder<JassStatementSyntax>(8);

            while (!AtEnd
                && !At(JassSyntaxKind.EndFunctionKeyword)
                && !IsTopLevelKeyword(Current.SyntaxKind))
            {
                var statement = ParseStatement();
                if (statement is not null)
                {
                    statements.Add(statement);
                }
                else
                {
                    _diagnostics.Report(
                        JassSyntaxDiagnostics.SyntaxError,
                        GetCurrentLocation(),
                        $"Unexpected token '{Current.Text}'");

                    var positionBefore = _position;
                    SkipToKeywordSync();

                    if (_position == positionBefore)
                    {
                        _position++;
                    }
                }
            }

            return statements.ToImmutable();
        }

        // === Shared helpers ===

        private JassIdentifierNameSyntax ParseIdentifierName()
        {
            var token = EatToken(JassSyntaxKind.IdentifierToken);
            return new JassIdentifierNameSyntax(token);
        }

        private JassTypeSyntax ParseTypeName()
        {
            if (JassSyntaxFacts.IsPredefinedTypeKeyword(Current.SyntaxKind))
            {
                return new JassPredefinedTypeSyntax(EatToken());
            }

            if (At(JassSyntaxKind.IdentifierToken))
            {
                return new JassIdentifierNameSyntax(EatToken());
            }

            return new JassIdentifierNameSyntax(CreateMissingToken(JassSyntaxKind.IdentifierToken));
        }

        private JassTypeSyntax? TryParseTypeName()
        {
            if (JassSyntaxFacts.IsPredefinedTypeKeyword(Current.SyntaxKind))
            {
                return new JassPredefinedTypeSyntax(EatToken());
            }

            if (At(JassSyntaxKind.IdentifierToken))
            {
                return new JassIdentifierNameSyntax(EatToken());
            }

            return null;
        }

        private JassParameterListOrEmptyParameterListSyntax ParseParameterListOrEmpty()
        {
            var takesToken = EatToken(JassSyntaxKind.TakesKeyword);

            if (At(JassSyntaxKind.NothingKeyword))
            {
                var nothingToken = EatToken();
                return new JassEmptyParameterListSyntax(takesToken, nothingToken);
            }

            return ParseParameterList(takesToken);
        }

        private JassParameterListSyntax ParseParameterList(JassSyntaxToken takesToken)
        {
            var firstParam = ParseParameter();

            if (!At(JassSyntaxKind.CommaToken))
            {
                return new JassParameterListSyntax(
                    takesToken,
                    SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken>.Create(firstParam));
            }

            var builder = SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken>.CreateBuilder(firstParam);

            while (At(JassSyntaxKind.CommaToken))
            {
                var commaToken = EatToken();
                var param = ParseParameter();
                builder.Add(commaToken, param);
            }

            return new JassParameterListSyntax(takesToken, builder.ToSeparatedSyntaxList());
        }

        private JassParameterSyntax ParseParameter()
        {
            var type = ParseTypeName();
            var identifierName = ParseIdentifierName();
            return new JassParameterSyntax(type, identifierName);
        }

        private JassReturnClauseSyntax ParseReturnClause()
        {
            var returnsToken = EatToken(JassSyntaxKind.ReturnsKeyword);
            var returnType = ParseTypeName();
            return new JassReturnClauseSyntax(returnsToken, returnType);
        }

        private JassEqualsValueClauseSyntax ParseEqualsValueClause()
        {
            var equalsToken = EatToken(JassSyntaxKind.EqualsToken);
            var expression = ParseExpression();
            return new JassEqualsValueClauseSyntax(equalsToken, expression);
        }

        /// <summary>
        /// Creates a missing token without reporting a diagnostic (the caller reports its own).
        /// </summary>
        private JassSyntaxToken CreateMissingTokenSilent(JassSyntaxKind expected)
        {
            var token = new JassSyntaxToken(
                JassSyntaxTriviaList.Empty,
                expected,
                string.Empty,
                JassSyntaxTriviaList.Empty,
                isMissing: true);

            if (_pendingSkippedTrivia is not null)
            {
                token = PrependSkippedTrivia(token, _pendingSkippedTrivia);
                _pendingSkippedTrivia = null;
            }

            return token;
        }
    }
}