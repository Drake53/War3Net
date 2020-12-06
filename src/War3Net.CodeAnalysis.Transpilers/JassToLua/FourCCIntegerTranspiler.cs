// ------------------------------------------------------------------------------
// <copyright file="FourCCIntegerTranspiler.cs" company="Drake53">
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
        public static void Transpile(this FourCCIntegerSyntax fourCCIntegerNode, ref StringBuilder sb)
        {
            _ = fourCCIntegerNode ?? throw new ArgumentNullException(nameof(fourCCIntegerNode));

            fourCCIntegerNode.FourCCNode.TranspileExpression(ref sb);
        }

        public static LuaExpressionSyntax TranspileToLua(this FourCCIntegerSyntax fourCCIntegerNode)
        {
            _ = fourCCIntegerNode ?? throw new ArgumentNullException(nameof(fourCCIntegerNode));

            return fourCCIntegerNode.FourCCNode.TranspileExpressionToLua();
        }
    }
}