using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassExpressionSyntax expression)
        {
            return expression switch
            {
                JassLiteralExpressionSyntax literalExpression => Transpile(literalExpression),
                JassFunctionReferenceExpressionSyntax functionReferenceExpression => Transpile(functionReferenceExpression),
                JassInvocationExpressionSyntax invocationExpression => Transpile(invocationExpression),
                JassElementAccessExpressionSyntax elementAccessExpression => Transpile(elementAccessExpression),
                JassIdentifierNameSyntax identifierName => Transpile(identifierName),
                JassParenthesizedExpressionSyntax parenthesizedExpression => Transpile(parenthesizedExpression),
                JassUnaryExpressionSyntax unaryExpression => Transpile(unaryExpression),
                JassBinaryExpressionSyntax binaryExpression => Transpile(binaryExpression),
            };
        }

        public ArgumentSyntax TranspileArgument(JassExpressionSyntax expression)
        {
            return SyntaxFactory.Argument(Transpile(expression));
        }
    }
}