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
        public LuaExpressionSyntax Transpile(JassExpressionSyntax expression, out JassTypeSyntax type)
        {
            return expression switch
            {
                JassLiteralExpressionSyntax literalExpression => Transpile(literalExpression, out type),
                JassFunctionReferenceExpressionSyntax functionReferenceExpression => Transpile(functionReferenceExpression, out type),
                JassInvocationExpressionSyntax invocationExpression => Transpile(invocationExpression, out type),
                JassElementAccessExpressionSyntax elementAccessExpression => Transpile(elementAccessExpression, out type),
                JassIdentifierNameSyntax identifierName => Transpile(identifierName, out type),
                JassParenthesizedExpressionSyntax parenthesizedExpression => Transpile(parenthesizedExpression, out type),
                JassUnaryExpressionSyntax unaryExpression => Transpile(unaryExpression, out type),
                JassBinaryExpressionSyntax binaryExpression => Transpile(binaryExpression, out type),
            };
        }
    }
}