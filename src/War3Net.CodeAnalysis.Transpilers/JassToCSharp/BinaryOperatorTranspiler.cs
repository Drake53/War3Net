// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTranspiler.cs" company="Drake53">
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
        public static SyntaxKind Transpile(this Jass.Syntax.BinaryOperatorSyntax binaryOperatorNode)
        {
            _ = binaryOperatorNode ?? throw new ArgumentNullException(nameof(binaryOperatorNode));

            return binaryOperatorNode.BinaryOperatorToken.TranspileBinaryOperator();
        }
    }
}