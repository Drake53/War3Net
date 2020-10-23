// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Text;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static Microsoft.CodeAnalysis.CSharp.SyntaxKind Transpile(this Syntax.UnaryOperatorSyntax unaryOperatorNode)
        {
            _ = unaryOperatorNode ?? throw new ArgumentNullException(nameof(unaryOperatorNode));

            return unaryOperatorNode.UnaryOperatorToken.TranspileUnaryOperator();
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.UnaryOperatorSyntax unaryOperatorNode, ref StringBuilder sb)
        {
            _ = unaryOperatorNode ?? throw new ArgumentNullException(nameof(unaryOperatorNode));

            unaryOperatorNode.UnaryOperatorToken.TranspileUnaryOperator(ref sb);
        }
    }
}