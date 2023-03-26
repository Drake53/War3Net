// ------------------------------------------------------------------------------
// <copyright file="ExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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
                JassArrayReferenceExpressionSyntax arrayReferenceExpression => Transpile(arrayReferenceExpression),
                JassIdentifierNameSyntax identifierName => TranspileIdentifierName(identifierName),
                JassParenthesizedExpressionSyntax parenthesizedExpression => Transpile(parenthesizedExpression),
                JassUnaryExpressionSyntax unaryExpression => Transpile(unaryExpression),
                JassBinaryExpressionSyntax binaryExpression => Transpile(binaryExpression),
            };
        }
    }
}