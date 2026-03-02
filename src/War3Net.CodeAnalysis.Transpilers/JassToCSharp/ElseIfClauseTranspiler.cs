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
        public StatementSyntax Transpile(
            JassElseIfClauseSyntax elseIfClause,
            ElseClauseSyntax? elseClause,
            JassSyntaxToken closingToken)
        {
            var closeBraceToken = Transpile(SyntaxKind.CloseBraceToken, closingToken);
            if (elseClause is not null)
            {
                closeBraceToken = closeBraceToken.WithSpace();
            }

            var elseIfBlock = SyntaxFactory.Block(
                Transpile(SyntaxKind.OpenBraceToken, elseIfClause.ElseIfClauseDeclarator.ThenToken),
                SyntaxFactory.List(elseIfClause.Statements.Select(Transpile)),
                closeBraceToken);

            if (elseIfClause.ElseIfClauseDeclarator.Condition is JassParenthesizedExpressionSyntax parenthesizedExpression)
            {
                return SyntaxFactory.IfStatement(
                    SyntaxFactory.List<AttributeListSyntax>(),
                    Transpile(SyntaxKind.IfKeyword, elseIfClause.ElseIfClauseDeclarator.ElseIfToken.TrailingTrivia),
                    Transpile(SyntaxKind.OpenParenToken, parenthesizedExpression.OpenParenToken),
                    Transpile(parenthesizedExpression.Expression),
                    Transpile(SyntaxKind.CloseParenToken, parenthesizedExpression.CloseParenToken),
                    elseIfBlock,
                    elseClause);
            }
            else
            {
                var trailingTrivia = MergeTrivia(
                    elseIfClause.ElseIfClauseDeclarator.ElseIfToken.TrailingTrivia,
                    elseIfClause.ElseIfClauseDeclarator.Condition.GetLeadingTrivia());

                var leadingTrivia = MergeTrivia(
                    elseIfClause.ElseIfClauseDeclarator.Condition.GetTrailingTrivia(),
                    elseIfClause.ElseIfClauseDeclarator.ThenToken.LeadingTrivia);

                return SyntaxFactory.IfStatement(
                    SyntaxFactory.List<AttributeListSyntax>(),
                    SyntaxFactory.Token(
                        SyntaxTriviaList.Empty,
                        SyntaxKind.IfKeyword,
                        SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace)),
                    Transpile(SyntaxKind.OpenParenToken, trailingTrivia),
                    Transpile(elseIfClause.ElseIfClauseDeclarator.Condition).WithoutTrivia(),
                    SyntaxFactory.Token(
                        Transpile(leadingTrivia),
                        SyntaxKind.CloseParenToken,
                        SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace)),
                    elseIfBlock,
                    elseClause);
            }
        }
    }
}