// ------------------------------------------------------------------------------
// <copyright file="JassRecursiveDescentParser.Statements.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Diagnostics;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Parsing
{
    internal sealed partial class JassRecursiveDescentParser
    {
        private JassStatementSyntax? ParseStatement()
        {
            return Current.SyntaxKind switch
            {
                JassSyntaxKind.SetKeyword => ParseSetStatement(),
                JassSyntaxKind.CallKeyword => ParseCallStatement(),
                JassSyntaxKind.ReturnKeyword => ParseReturnStatement(),
                JassSyntaxKind.ExitWhenKeyword => ParseExitStatement(),
                JassSyntaxKind.IfKeyword => ParseIfStatement(),
                JassSyntaxKind.LoopKeyword => ParseLoopStatement(),
                JassSyntaxKind.LocalKeyword => ParseLocalVariableDeclarationStatement(),
                JassSyntaxKind.DebugKeyword => ParseDebugStatement(),
                _ => null,
            };
        }

        private JassSetStatementSyntax ParseSetStatement()
        {
            var setToken = EatToken(JassSyntaxKind.SetKeyword);
            var identifierName = ParseIdentifierName();

            JassElementAccessClauseSyntax? elementAccessClause = null;
            if (At(JassSyntaxKind.OpenBracketToken))
            {
                elementAccessClause = ParseElementAccessClause();
            }

            var equalsValueClause = ParseEqualsValueClause();

            return new JassSetStatementSyntax(setToken, identifierName, elementAccessClause, equalsValueClause);
        }

        private JassCallStatementSyntax ParseCallStatement()
        {
            var callToken = EatToken(JassSyntaxKind.CallKeyword);
            var identifierName = ParseIdentifierName();
            var argumentList = ParseArgumentList();

            return new JassCallStatementSyntax(callToken, identifierName, argumentList);
        }

        private JassReturnStatementSyntax ParseReturnStatement()
        {
            var returnToken = EatToken(JassSyntaxKind.ReturnKeyword);

            JassExpressionSyntax? expression = null;
            if (!AtEnd && !IsStatementOrBlockKeyword(Current.SyntaxKind) && !IsTopLevelKeyword(Current.SyntaxKind))
            {
                expression = ParseExpression();
            }

            return new JassReturnStatementSyntax(returnToken, expression);
        }

        private JassExitStatementSyntax ParseExitStatement()
        {
            var exitWhenToken = EatToken(JassSyntaxKind.ExitWhenKeyword);
            var condition = ParseExpression();

            return new JassExitStatementSyntax(exitWhenToken, condition);
        }

        private JassIfStatementSyntax ParseIfStatement()
        {
            var ifClause = ParseIfClause();

            var elseIfClauses = ImmutableArray.CreateBuilder<JassElseIfClauseSyntax>(2);
            JassElseClauseSyntax? elseClause = null;

            while (!AtEnd
                && !At(JassSyntaxKind.EndIfKeyword)
                && !At(JassSyntaxKind.EndFunctionKeyword)
                && !IsTopLevelKeyword(Current.SyntaxKind))
            {
                if (At(JassSyntaxKind.ElseIfKeyword))
                {
                    if (elseClause is not null)
                    {
                        _diagnostics.Report(
                            JassSyntaxDiagnostics.ElseAfterElse,
                            GetCurrentLocation());
                    }

                    elseIfClauses.Add(ParseElseIfClause());
                }
                else if (At(JassSyntaxKind.ElseKeyword))
                {
                    if (elseClause is not null)
                    {
                        _diagnostics.Report(
                            JassSyntaxDiagnostics.DuplicateElse,
                            GetCurrentLocation());
                    }

                    elseClause = ParseElseClause();
                }
                else
                {
                    break;
                }
            }

            JassSyntaxToken endIfToken;
            if (At(JassSyntaxKind.EndIfKeyword))
            {
                endIfToken = EatToken();
            }
            else
            {
                _diagnostics.Report(
                    JassSyntaxDiagnostics.MissingEndIf,
                    GetCurrentLocation());
                endIfToken = CreateMissingTokenSilent(JassSyntaxKind.EndIfKeyword);
            }

            return new JassIfStatementSyntax(ifClause, elseIfClauses.ToImmutable(), elseClause, endIfToken);
        }

        private JassIfClauseSyntax ParseIfClause()
        {
            var ifToken = EatToken(JassSyntaxKind.IfKeyword);
            var condition = ParseExpression();
            var thenToken = EatToken(JassSyntaxKind.ThenKeyword);

            var declarator = new JassIfClauseDeclaratorSyntax(ifToken, condition, thenToken);
            var statements = ParseStatementBlock(
                JassSyntaxKind.EndIfKeyword,
                JassSyntaxKind.ElseIfKeyword,
                JassSyntaxKind.ElseKeyword);

            return new JassIfClauseSyntax(declarator, statements);
        }

        private JassElseIfClauseSyntax ParseElseIfClause()
        {
            var elseIfToken = EatToken(JassSyntaxKind.ElseIfKeyword);
            var condition = ParseExpression();
            var thenToken = EatToken(JassSyntaxKind.ThenKeyword);

            var declarator = new JassElseIfClauseDeclaratorSyntax(elseIfToken, condition, thenToken);
            var statements = ParseStatementBlock(
                JassSyntaxKind.EndIfKeyword,
                JassSyntaxKind.ElseIfKeyword,
                JassSyntaxKind.ElseKeyword);

            return new JassElseIfClauseSyntax(declarator, statements);
        }

        private JassElseClauseSyntax ParseElseClause()
        {
            var elseToken = EatToken(JassSyntaxKind.ElseKeyword);
            var statements = ParseStatementBlock(
                JassSyntaxKind.EndIfKeyword,
                JassSyntaxKind.ElseIfKeyword,
                JassSyntaxKind.ElseKeyword);

            return new JassElseClauseSyntax(elseToken, statements);
        }

        private JassLoopStatementSyntax ParseLoopStatement()
        {
            var loopToken = EatToken(JassSyntaxKind.LoopKeyword);
            var statements = ParseStatementBlock(JassSyntaxKind.EndLoopKeyword);

            JassSyntaxToken endLoopToken;
            if (At(JassSyntaxKind.EndLoopKeyword))
            {
                endLoopToken = EatToken();
            }
            else
            {
                _diagnostics.Report(
                    JassSyntaxDiagnostics.MissingEndLoop,
                    GetCurrentLocation());
                endLoopToken = CreateMissingTokenSilent(JassSyntaxKind.EndLoopKeyword);
            }

            return new JassLoopStatementSyntax(loopToken, statements, endLoopToken);
        }

        private JassLocalVariableDeclarationStatementSyntax ParseLocalVariableDeclarationStatement()
        {
            var localToken = EatToken(JassSyntaxKind.LocalKeyword);
            var type = ParseTypeName();

            if (At(JassSyntaxKind.ArrayKeyword))
            {
                var arrayToken = EatToken();
                var identifierName = ParseIdentifierName();
                var declarator = new JassArrayDeclaratorSyntax(type, arrayToken, identifierName);
                return new JassLocalVariableDeclarationStatementSyntax(localToken, declarator);
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
                return new JassLocalVariableDeclarationStatementSyntax(localToken, declarator);
            }
        }

        private JassDebugStatementSyntax? ParseDebugStatement()
        {
            var debugToken = EatToken(JassSyntaxKind.DebugKeyword);

            JassStatementSyntax innerStatement = Current.SyntaxKind switch
            {
                JassSyntaxKind.SetKeyword => ParseSetStatement(),
                JassSyntaxKind.CallKeyword => ParseCallStatement(),
                JassSyntaxKind.IfKeyword => ParseIfStatement(),
                JassSyntaxKind.LoopKeyword => ParseLoopStatement(),
                _ => ParseCallStatement(),
            };

            return new JassDebugStatementSyntax(debugToken, innerStatement);
        }

        /// <summary>
        /// Parses a block of statements terminated by one of the specified closing keywords,
        /// <c>endfunction</c>, or a top-level keyword.
        /// </summary>
        private ImmutableArray<JassStatementSyntax> ParseStatementBlock(
            JassSyntaxKind closingKeyword1,
            JassSyntaxKind closingKeyword2 = JassSyntaxKind.None,
            JassSyntaxKind closingKeyword3 = JassSyntaxKind.None)
        {
            var statements = ImmutableArray.CreateBuilder<JassStatementSyntax>(8);

            while (!AtEnd
                && !At(closingKeyword1)
                && (closingKeyword2 == JassSyntaxKind.None || !At(closingKeyword2))
                && (closingKeyword3 == JassSyntaxKind.None || !At(closingKeyword3))
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

        private JassElementAccessClauseSyntax ParseElementAccessClause()
        {
            var openBracketToken = EatToken(JassSyntaxKind.OpenBracketToken);
            var expression = ParseExpression();
            var closeBracketToken = EatToken(JassSyntaxKind.CloseBracketToken);

            return new JassElementAccessClauseSyntax(openBracketToken, expression, closeBracketToken);
        }
    }
}