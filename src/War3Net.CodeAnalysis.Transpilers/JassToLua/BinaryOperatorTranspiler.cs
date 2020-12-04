// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTranspiler.cs" company="Drake53">
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
        public static void Transpile(this BinaryOperatorSyntax binaryOperatorNode, bool isString, ref StringBuilder sb)
        {
            _ = binaryOperatorNode ?? throw new ArgumentNullException(nameof(binaryOperatorNode));

            sb.Append(' ');
            binaryOperatorNode.BinaryOperatorToken.TranspileBinaryOperator(isString, ref sb);
            sb.Append(' ');
        }
    }
}