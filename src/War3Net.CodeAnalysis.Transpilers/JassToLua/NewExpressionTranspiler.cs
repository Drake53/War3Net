// ------------------------------------------------------------------------------
// <copyright file="NewExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(NewExpressionSyntax newExpression)
        {
            _ = newExpression ?? throw new ArgumentNullException(nameof(newExpression));

            var left = Transpile(newExpression.Expression, out var leftExpressionType);
            if (newExpression.ExpressionTail is null)
            {
                return left;
            }

            var right = Transpile(newExpression.ExpressionTail.ExpressionNode, out var rightExpressionType);

            return new LuaBinaryExpressionSyntax(left, Transpile(newExpression.ExpressionTail.BinaryOperatorNode, leftExpressionType, rightExpressionType), right);
        }

        public LuaExpressionSyntax Transpile(NewExpressionSyntax newExpression, out SyntaxTokenType expressionType)
        {
            _ = newExpression ?? throw new ArgumentNullException(nameof(newExpression));

            var left = Transpile(newExpression.Expression, out expressionType);
            if (newExpression.ExpressionTail is null)
            {
                return left;
            }

            var right = Transpile(newExpression.ExpressionTail.ExpressionNode, out var rightExpressionType);
            if (rightExpressionType != expressionType)
            {
                if (rightExpressionType == SyntaxTokenType.StringKeyword)
                {
                    expressionType = SyntaxTokenType.StringKeyword;
                }
                else if (expressionType != SyntaxTokenType.StringKeyword && rightExpressionType == SyntaxTokenType.RealKeyword)
                {
                    expressionType = SyntaxTokenType.RealKeyword;
                }
            }

            return new LuaBinaryExpressionSyntax(left, Transpile(newExpression.ExpressionTail.BinaryOperatorNode, expressionType, rightExpressionType), right);
        }
    }
}