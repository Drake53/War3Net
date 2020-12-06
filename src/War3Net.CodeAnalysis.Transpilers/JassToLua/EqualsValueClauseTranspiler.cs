// ------------------------------------------------------------------------------
// <copyright file="EqualsValueClauseTranspiler.cs" company="Drake53">
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
        public static void Transpile(this EqualsValueClauseSyntax equalsValueClauseNode, ref StringBuilder sb)
        {
            _ = equalsValueClauseNode ?? throw new ArgumentNullException(nameof(equalsValueClauseNode));

            sb.Append(" = ");
            equalsValueClauseNode.ValueNode.Transpile(ref sb);
        }

        public static LuaExpressionSyntax TranspileToLua(this EqualsValueClauseSyntax equalsValueClauseNode)
        {
            _ = equalsValueClauseNode ?? throw new ArgumentNullException(nameof(equalsValueClauseNode));

            return equalsValueClauseNode.ValueNode.TranspileToLua();
        }
    }
}