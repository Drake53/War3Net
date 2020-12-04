// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorTranspiler.cs" company="Drake53">
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
        public static void Transpile(this UnaryOperatorSyntax unaryOperatorNode, ref StringBuilder sb)
        {
            _ = unaryOperatorNode ?? throw new ArgumentNullException(nameof(unaryOperatorNode));

            unaryOperatorNode.UnaryOperatorToken.TranspileUnaryOperator(ref sb);
        }
    }
}