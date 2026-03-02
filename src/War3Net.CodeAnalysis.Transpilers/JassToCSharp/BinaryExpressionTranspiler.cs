using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassBinaryExpressionSyntax binaryExpression)
        {
            return SyntaxFactory.BinaryExpression(
                TranspileBinaryExpressionKind(binaryExpression.SyntaxKind),
                Transpile(binaryExpression.Left),
                Transpile(
                    TranspileBinaryOperatorKind(binaryExpression.OperatorToken.SyntaxKind),
                    binaryExpression.OperatorToken),
                Transpile(binaryExpression.Right));
        }
    }
}