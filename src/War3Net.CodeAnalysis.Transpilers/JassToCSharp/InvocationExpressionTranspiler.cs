using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassInvocationExpressionSyntax invocationExpression)
        {
            return SyntaxFactory.InvocationExpression(
                Transpile(invocationExpression.IdentifierName),
                Transpile(invocationExpression.ArgumentList));
        }
    }
}