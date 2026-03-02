using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassUnaryExpressionSyntax unaryExpression)
        {
            return SyntaxFactory.PrefixUnaryExpression(
                TranspileUnaryExpressionKind(unaryExpression.SyntaxKind),
                Transpile(
                    TranspileUnaryOperatorKind(unaryExpression.OperatorToken.SyntaxKind),
                    unaryExpression.OperatorToken),
                Transpile(unaryExpression.Expression));
        }
    }
}