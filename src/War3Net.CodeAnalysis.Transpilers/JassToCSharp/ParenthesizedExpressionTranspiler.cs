// ------------------------------------------------------------------------------
// <copyright file="ParenthesizedExpressionTranspiler.cs" company="Drake53">
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
        public static ExpressionSyntax Transpile(this Jass.Syntax.ParenthesizedExpressionSyntax parenthesizedExpressionNode)
        {
            _ = parenthesizedExpressionNode ?? throw new ArgumentNullException(nameof(parenthesizedExpressionNode));

            return SyntaxFactory.ParenthesizedExpression(parenthesizedExpressionNode.ExpressionNode.Transpile());
        }

        public static ExpressionSyntax Transpile(this Jass.Syntax.ParenthesizedExpressionSyntax parenthesizedExpressionNode, out bool @const)
        {
            _ = parenthesizedExpressionNode ?? throw new ArgumentNullException(nameof(parenthesizedExpressionNode));

            return SyntaxFactory.ParenthesizedExpression(parenthesizedExpressionNode.ExpressionNode.Transpile(out @const));
        }
    }
}