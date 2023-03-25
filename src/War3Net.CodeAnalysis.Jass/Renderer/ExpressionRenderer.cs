// ------------------------------------------------------------------------------
// <copyright file="ExpressionRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassExpressionSyntax expression)
        {
            switch (expression)
            {
                case JassLiteralExpressionSyntax literalExpression: Render(literalExpression); break;
                case JassFunctionReferenceExpressionSyntax functionReferenceExpression: Render(functionReferenceExpression); break;
                case JassInvocationExpressionSyntax invocationExpression: Render(invocationExpression); break;
                case JassArrayReferenceExpressionSyntax arrayReferenceExpression: Render(arrayReferenceExpression); break;
                case JassIdentifierNameSyntax identifierName: Render(identifierName); break;
                case JassParenthesizedExpressionSyntax parenthesizedExpression: Render(parenthesizedExpression); break;
                case JassUnaryExpressionSyntax unaryExpression: Render(unaryExpression); break;
                case JassBinaryExpressionSyntax binaryExpression: Render(binaryExpression); break;

                default: throw new NotSupportedException();
            }
        }
    }
}