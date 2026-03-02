using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassParenthesizedExpressionSyntax parenthesizedExpression)
        {
            return SyntaxFactory.ParenthesizedExpression(
                Transpile(SyntaxKind.OpenParenToken, parenthesizedExpression.OpenParenToken),
                Transpile(parenthesizedExpression.Expression),
                Transpile(SyntaxKind.CloseParenToken, parenthesizedExpression.CloseParenToken));
        }
    }
}