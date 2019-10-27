// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static Microsoft.CodeAnalysis.CSharp.SyntaxKind Transpile(this Syntax.BinaryOperatorSyntax binaryOperatorNode)
        {
            _ = binaryOperatorNode ?? throw new ArgumentNullException(nameof(binaryOperatorNode));

            return binaryOperatorNode.BinaryOperatorToken.TranspileBinaryOperator();
        }
    }
}