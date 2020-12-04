// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static SyntaxKind Transpile(this Jass.Syntax.UnaryOperatorSyntax unaryOperatorNode)
        {
            _ = unaryOperatorNode ?? throw new ArgumentNullException(nameof(unaryOperatorNode));

            return unaryOperatorNode.UnaryOperatorToken.TranspileUnaryOperator();
        }
    }
}