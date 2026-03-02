using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassFunctionReferenceExpressionSyntax functionReferenceExpression)
        {
            var leadingTrivia = MergeTrivia(
                functionReferenceExpression.FunctionToken,
                functionReferenceExpression.IdentifierName.Token.LeadingTrivia);

            return Transpile(leadingTrivia, functionReferenceExpression.IdentifierName);
        }
    }
}