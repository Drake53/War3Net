using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers.Extensions;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassExitStatementSyntax exitStatement)
        {
            if (exitStatement.Condition is JassParenthesizedExpressionSyntax parenthesizedExpression)
            {
                return SyntaxFactory.IfStatement(
                    Transpile(SyntaxKind.IfKeyword, exitStatement.ExitWhenToken),
                    Transpile(SyntaxKind.OpenParenToken, parenthesizedExpression.OpenParenToken),
                    Transpile(parenthesizedExpression.Expression),
                    Transpile(
                        parenthesizedExpression.CloseParenToken.LeadingTrivia,
                        SyntaxKind.CloseParenToken,
                        JassSyntaxTriviaList.SingleSpace),
                    SyntaxFactory.BreakStatement(
                        SyntaxFactory.Token(SyntaxKind.BreakKeyword),
                        Transpile(SyntaxKind.SemicolonToken, parenthesizedExpression.CloseParenToken.TrailingTrivia)),
                    null);
            }
            else
            {
                var expression = Transpile(exitStatement.Condition);
                var trailingTrivia = expression.GetTrailingTrivia();

                return SyntaxFactory.IfStatement(
                    Transpile(SyntaxKind.IfKeyword, exitStatement.ExitWhenToken),
                    SyntaxFactory.Token(SyntaxKind.OpenParenToken),
                    expression.WithoutTrailingTrivia(),
                    Token(
                        SyntaxKind.CloseParenToken,
                        SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace)),
                    SyntaxFactory.BreakStatement(
                        SyntaxFactory.Token(SyntaxKind.BreakKeyword),
                        SyntaxFactory.Token(
                            SyntaxTriviaList.Empty,
                            SyntaxKind.SemicolonToken,
                            trailingTrivia)),
                    null);
            }
        }
    }
}