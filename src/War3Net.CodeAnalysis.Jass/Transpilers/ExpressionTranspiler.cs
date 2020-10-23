// ------------------------------------------------------------------------------
// <copyright file="ExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ExpressionSyntax Transpile(this Syntax.ExpressionSyntax expressionNode)
        {
            _ = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));

            return expressionNode.UnaryExpression?.Transpile()
                ?? expressionNode.FunctionCall?.Transpile()
                ?? expressionNode.ArrayReference?.Transpile()
                ?? expressionNode.FunctionReference?.Transpile()
                ?? expressionNode.ConstantExpression?.Transpile()
                ?? expressionNode.ParenthesizedExpressionSyntax?.Transpile()
                ?? expressionNode.Identifier.TranspileExpression();
        }

        public static ExpressionSyntax Transpile(this Syntax.ExpressionSyntax expressionNode, out bool @const)
        {
            _ = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));

            // Assign here so it doesn't complain about out parameter not being assigned, even though it gets assigned in all code paths.
            @const = false;

            return expressionNode.UnaryExpression?.Transpile(out @const)
                ?? expressionNode.FunctionCall?.Transpile(out @const)
                ?? expressionNode.ArrayReference?.Transpile(out @const)
                ?? expressionNode.FunctionReference?.Transpile(out @const)
                ?? expressionNode.ConstantExpression?.Transpile(out @const)
                ?? expressionNode.ParenthesizedExpressionSyntax?.Transpile(out @const)
                ?? expressionNode.Identifier.TranspileExpression(out @const);
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.ExpressionSyntax expressionNode, ref StringBuilder sb)
        {
            _ = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));

            expressionNode.UnaryExpression?.Transpile(ref sb);
            expressionNode.FunctionCall?.Transpile(ref sb);
            expressionNode.ArrayReference?.Transpile(ref sb);
            expressionNode.FunctionReference?.Transpile(ref sb);
            expressionNode.ConstantExpression?.Transpile(ref sb);
            expressionNode.ParenthesizedExpressionSyntax?.Transpile(ref sb);
            expressionNode.Identifier?.TranspileExpression(ref sb);
        }
    }
}