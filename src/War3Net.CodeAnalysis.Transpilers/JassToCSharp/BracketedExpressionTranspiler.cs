// ------------------------------------------------------------------------------
// <copyright file="BracketedExpressionTranspiler.cs" company="Drake53">
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
        public static BracketedArgumentListSyntax Transpile(this Jass.Syntax.BracketedExpressionSyntax bracketedExpressionNode)
        {
            _ = bracketedExpressionNode ?? throw new ArgumentNullException(nameof(bracketedExpressionNode));

            return SyntaxFactory.BracketedArgumentList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Argument(
                        bracketedExpressionNode.ExpressionNode.Transpile())));
        }
    }
}