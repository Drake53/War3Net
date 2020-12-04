// ------------------------------------------------------------------------------
// <copyright file="BracketedExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this BracketedExpressionSyntax bracketedExpressionNode, ref StringBuilder sb)
        {
            _ = bracketedExpressionNode ?? throw new ArgumentNullException(nameof(bracketedExpressionNode));

            sb.Append('[');
            bracketedExpressionNode.ExpressionNode.Transpile(ref sb);
            sb.Append(']');
        }
    }
}