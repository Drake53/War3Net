using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassElementAccessExpressionSyntax elementAccessExpression)
        {
            return SyntaxFactory.ElementAccessExpression(
                Transpile(elementAccessExpression.IdentifierName),
                Transpile(elementAccessExpression.ElementAccessClause));
        }
    }
}