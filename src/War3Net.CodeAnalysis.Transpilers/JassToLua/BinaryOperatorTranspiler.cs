// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this BinaryOperatorSyntax binaryOperatorNode, bool isString, ref StringBuilder sb)
        {
            _ = binaryOperatorNode ?? throw new ArgumentNullException(nameof(binaryOperatorNode));

            sb.Append(' ');
            binaryOperatorNode.BinaryOperatorToken.TranspileBinaryOperator(isString, ref sb);
            sb.Append(' ');
        }

        public static string TranspileToLua(this BinaryOperatorSyntax binaryOperatorNode, bool isString)
        {
            _ = binaryOperatorNode ?? throw new ArgumentNullException(nameof(binaryOperatorNode));

            return binaryOperatorNode.BinaryOperatorToken.TranspileBinaryOperatorToLua(isString);
        }
    }
}