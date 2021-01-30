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
        public ExpressionSyntax Transpile(IExpressionSyntax expression)
        {
            return expression switch
            {
                JassCharacterLiteralExpressionSyntax characterLiteralExpression => Transpile(characterLiteralExpression),
                JassFourCCLiteralExpressionSyntax fourCCLiteralExpression => Transpile(fourCCLiteralExpression),
                JassHexadecimalLiteralExpressionSyntax hexadecimalLiteralExpression => Transpile(hexadecimalLiteralExpression),
                JassRealLiteralExpressionSyntax realLiteralExpression => Transpile(realLiteralExpression),
                JassOctalLiteralExpressionSyntax octalLiteralExpression => Transpile(octalLiteralExpression),
                JassDecimalLiteralExpressionSyntax decimalLiteralExpression => Transpile(decimalLiteralExpression),
                JassBooleanLiteralExpressionSyntax booleanLiteralExpression => Transpile(booleanLiteralExpression),
                JassStringLiteralExpressionSyntax stringLiteralExpression => Transpile(stringLiteralExpression),
                JassNullLiteralExpressionSyntax nullLiteralExpression => Transpile(nullLiteralExpression),
                JassFunctionReferenceExpressionSyntax functionReferenceExpression => Transpile(functionReferenceExpression),
                JassInvocationExpressionSyntax invocationExpression => Transpile(invocationExpression),
                JassArrayReferenceExpressionSyntax arrayReferenceExpression => Transpile(arrayReferenceExpression),
                JassVariableReferenceExpressionSyntax variableReferenceExpression => Transpile(variableReferenceExpression),
                JassParenthesizedExpressionSyntax parenthesizedExpression => Transpile(parenthesizedExpression),
                JassUnaryExpressionSyntax unaryExpression => Transpile(unaryExpression),
                JassBinaryExpressionSyntax binaryExpression => Transpile(binaryExpression),
            };
        }
    }
}