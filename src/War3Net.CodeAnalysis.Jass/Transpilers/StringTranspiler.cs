// ------------------------------------------------------------------------------
// <copyright file="StringTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ExpressionSyntax Transpile(this Syntax.StringSyntax stringNode)
        {
            _ = stringNode ?? throw new ArgumentNullException(nameof(stringNode));

            return stringNode.StringNode?.TranspileExpression()
                ?? SyntaxFactory.ParseExpression("string.Empty");
        }
    }
}