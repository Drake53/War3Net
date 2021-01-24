// ------------------------------------------------------------------------------
// <copyright file="ExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(IExpressionSyntax expression, out JassTypeSyntax type)
        {
            return expression switch
            {
                JassCharacterLiteralExpressionSyntax characterLiteralExpression => Transpile(characterLiteralExpression, out type),
                JassFourCCLiteralExpressionSyntax fourCCLiteralExpression => Transpile(fourCCLiteralExpression, out type),
                JassHexadecimalLiteralExpressionSyntax hexadecimalLiteralExpression => Transpile(hexadecimalLiteralExpression, out type),
                JassRealLiteralExpressionSyntax realLiteralExpression => Transpile(realLiteralExpression, out type),
                JassOctalLiteralExpressionSyntax octalLiteralExpression => Transpile(octalLiteralExpression, out type),
                JassDecimalLiteralExpressionSyntax decimalLiteralExpression => Transpile(decimalLiteralExpression, out type),
                JassBooleanLiteralExpressionSyntax booleanLiteralExpression => Transpile(booleanLiteralExpression, out type),
                JassStringLiteralExpressionSyntax stringLiteralExpression => Transpile(stringLiteralExpression, out type),
                JassNullLiteralExpressionSyntax nullLiteralExpression => Transpile(nullLiteralExpression, out type),
                JassFunctionReferenceExpressionSyntax functionReferenceExpression => Transpile(functionReferenceExpression, out type),
                JassInvocationExpressionSyntax invocationExpression => Transpile(invocationExpression, out type),
                JassArrayReferenceExpressionSyntax arrayReferenceExpression => Transpile(arrayReferenceExpression, out type),
                JassVariableReferenceExpressionSyntax variableReferenceExpression => Transpile(variableReferenceExpression, out type),
                JassParenthesizedExpressionSyntax parenthesizedExpression => Transpile(parenthesizedExpression, out type),
                JassUnaryExpressionSyntax unaryExpression => Transpile(unaryExpression, out type),
                JassBinaryExpressionSyntax binaryExpression => Transpile(binaryExpression, out type),
            };
        }
    }
}