// ------------------------------------------------------------------------------
// <copyright file="ExpressionRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(IExpressionSyntax expression)
        {
            switch (expression)
            {
                case JassCharacterLiteralExpressionSyntax characterLiteralExpression: Render(characterLiteralExpression); break;
                case JassFourCCLiteralExpressionSyntax fourCCLiteralExpression: Render(fourCCLiteralExpression); break;
                case JassHexadecimalLiteralExpressionSyntax hexadecimalLiteralExpression: Render(hexadecimalLiteralExpression); break;
                case JassRealLiteralExpressionSyntax realLiteralExpression: Render(realLiteralExpression); break;
                case JassOctalLiteralExpressionSyntax octalLiteralExpression: Render(octalLiteralExpression); break;
                case JassDecimalLiteralExpressionSyntax decimalLiteralExpression: Render(decimalLiteralExpression); break;
                case JassBooleanLiteralExpressionSyntax booleanLiteralExpression: Render(booleanLiteralExpression); break;
                case JassStringLiteralExpressionSyntax stringLiteralExpression: Render(stringLiteralExpression); break;
                case JassNullLiteralExpressionSyntax nullLiteralExpression: Render(nullLiteralExpression); break;
                case JassFunctionReferenceExpressionSyntax functionReferenceExpression: Render(functionReferenceExpression); break;
                case JassInvocationExpressionSyntax invocationExpression: Render(invocationExpression); break;
                case JassArrayReferenceExpressionSyntax arrayReferenceExpression: Render(arrayReferenceExpression); break;
                case JassVariableReferenceExpressionSyntax variableReferenceExpression: Render(variableReferenceExpression); break;
                case JassParenthesizedExpressionSyntax parenthesizedExpression: Render(parenthesizedExpression); break;
                case JassUnaryExpressionSyntax unaryExpression: Render(unaryExpression); break;
                case JassBinaryExpressionSyntax binaryExpression: Render(binaryExpression); break;
            }
        }
    }
}