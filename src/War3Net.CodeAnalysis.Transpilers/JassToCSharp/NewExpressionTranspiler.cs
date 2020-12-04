// ------------------------------------------------------------------------------
// <copyright file="NewExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ExpressionSyntax Transpile(this Jass.Syntax.NewExpressionSyntax newExpressionNode)
        {
            _ = newExpressionNode ?? throw new ArgumentNullException(nameof(newExpressionNode));

            if (newExpressionNode.EmptyExpressionTail is null)
            {
                return SyntaxFactory.BinaryExpression(
                    newExpressionNode.ExpressionTail.BinaryOperatorNode.Transpile(),
                    newExpressionNode.Expression.Transpile(),
                    newExpressionNode.ExpressionTail.ExpressionNode.Transpile());
            }
            else
            {
                return newExpressionNode.Expression.Transpile();
            }
        }

        public static ExpressionSyntax Transpile(this Jass.Syntax.NewExpressionSyntax newExpressionNode, out bool @const)
        {
            _ = newExpressionNode ?? throw new ArgumentNullException(nameof(newExpressionNode));

            if (newExpressionNode.EmptyExpressionTail is null)
            {
                var binaryExpression = SyntaxFactory.BinaryExpression(
                    newExpressionNode.ExpressionTail.BinaryOperatorNode.Transpile(),
                    newExpressionNode.Expression.Transpile(out var leftConst),
                    newExpressionNode.ExpressionTail.ExpressionNode.Transpile(out var rightConst));

                @const = leftConst && rightConst;
                return binaryExpression;
            }
            else
            {
                return newExpressionNode.Expression.Transpile(out @const);
            }
        }
    }
}