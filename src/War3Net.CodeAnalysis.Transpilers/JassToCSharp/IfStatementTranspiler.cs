// ------------------------------------------------------------------------------
// <copyright file="IfStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers.Extensions;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassIfStatementSyntax ifStatement)
        {
            ElseClauseSyntax? elseClause = null;
            JassSyntaxToken closingToken = ifStatement.EndIfToken;

            if (ifStatement.ElseClause is not null)
            {
                elseClause = Transpile(ifStatement.ElseClause, closingToken);
                closingToken = ifStatement.ElseClause.ElseToken;
            }

            foreach (var elseIfClause in ifStatement.ElseIfClauses.Reverse())
            {
                elseClause = SyntaxFactory.ElseClause(
                    Token(
                        SyntaxKind.ElseKeyword,
                        SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace)),
                    Transpile(elseIfClause, elseClause, closingToken));

                closingToken = elseIfClause.ElseIfClauseDeclarator.ElseIfToken;
            }

            var closeBraceToken = Transpile(SyntaxKind.CloseBraceToken, closingToken);
            if (elseClause is not null)
            {
                closeBraceToken = closeBraceToken.WithSpace();
            }

            var ifBlock = SyntaxFactory.Block(
                Transpile(SyntaxKind.OpenBraceToken, ifStatement.IfClause.IfClauseDeclarator.ThenToken),
                SyntaxFactory.List(ifStatement.IfClause.Statements.Select(Transpile)),
                closeBraceToken);

            if (ifStatement.IfClause.IfClauseDeclarator.Condition is JassParenthesizedExpressionSyntax parenthesizedExpression)
            {
                return SyntaxFactory.IfStatement(
                    SyntaxFactory.List<AttributeListSyntax>(),
                    Transpile(SyntaxKind.IfKeyword, ifStatement.IfClause.IfClauseDeclarator.IfToken),
                    Transpile(SyntaxKind.OpenParenToken, parenthesizedExpression.OpenParenToken),
                    Transpile(parenthesizedExpression.Expression),
                    Transpile(SyntaxKind.CloseParenToken, parenthesizedExpression.CloseParenToken),
                    ifBlock,
                    elseClause);
            }
            else
            {
                var trailingTrivia = MergeTrivia(
                    ifStatement.IfClause.IfClauseDeclarator.IfToken.TrailingTrivia,
                    ifStatement.IfClause.IfClauseDeclarator.Condition.GetLeadingTrivia());

                var leadingTrivia = MergeTrivia(
                    ifStatement.IfClause.IfClauseDeclarator.Condition.GetTrailingTrivia(),
                    ifStatement.IfClause.IfClauseDeclarator.ThenToken.LeadingTrivia);

                return SyntaxFactory.IfStatement(
                    SyntaxFactory.List<AttributeListSyntax>(),
                    SyntaxFactory.Token(
                        Transpile(ifStatement.IfClause.IfClauseDeclarator.IfToken.LeadingTrivia),
                        SyntaxKind.IfKeyword,
                        SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace)),
                    Transpile(SyntaxKind.OpenParenToken, trailingTrivia),
                    Transpile(ifStatement.IfClause.IfClauseDeclarator.Condition).WithoutTrivia(),
                    SyntaxFactory.Token(
                        Transpile(leadingTrivia),
                        SyntaxKind.CloseParenToken,
                        SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace)),
                    ifBlock,
                    elseClause);
            }
        }
    }
}