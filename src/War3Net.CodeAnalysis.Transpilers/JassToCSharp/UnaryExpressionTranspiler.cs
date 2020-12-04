// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionTranspiler.cs" company="Drake53">
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
        public static ExpressionSyntax Transpile(this Jass.Syntax.UnaryExpressionSyntax unaryExpressionNode)
        {
            _ = unaryExpressionNode ?? throw new ArgumentNullException(nameof(unaryExpressionNode));

            return SyntaxFactory.PrefixUnaryExpression(
                unaryExpressionNode.UnaryOperatorNode.Transpile(),
                unaryExpressionNode.ExpressionNode.Transpile());
        }

        public static ExpressionSyntax Transpile(this Jass.Syntax.UnaryExpressionSyntax unaryExpressionNode, out bool @const)
        {
            _ = unaryExpressionNode ?? throw new ArgumentNullException(nameof(unaryExpressionNode));

            return SyntaxFactory.PrefixUnaryExpression(
                unaryExpressionNode.UnaryOperatorNode.Transpile(),
                unaryExpressionNode.ExpressionNode.Transpile(out @const));
        }
    }
}