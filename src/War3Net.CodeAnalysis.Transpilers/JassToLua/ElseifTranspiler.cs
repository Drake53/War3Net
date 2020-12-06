// ------------------------------------------------------------------------------
// <copyright file="ElseifTranspiler.cs" company="Drake53">
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
        public static void Transpile(this ElseifSyntax elseifNode, ref StringBuilder sb)
        {
            _ = elseifNode ?? throw new ArgumentNullException(nameof(elseifNode));

            sb.Append("elseif ");
            elseifNode.ConditionExpressionNode.Transpile(ref sb);
            sb.AppendLine(" then");
            elseifNode.StatementListNode.Transpile(ref sb);
            if (elseifNode.EmptyElseClauseNode is null)
            {
                elseifNode.ElseClauseNode.Transpile(ref sb);
            }
            else
            {
                sb.Append("end");
            }
        }

        public static LuaElseIfStatementSyntax TranspileToLua(this ElseifSyntax elseifNode)
        {
            _ = elseifNode ?? throw new ArgumentNullException(nameof(elseifNode));

            var elseif = new LuaElseIfStatementSyntax(elseifNode.ConditionExpressionNode.TranspileToLua());
            elseif.Body.Statements.AddRange(elseifNode.StatementListNode.TranspileToLua());

            return elseif;
        }
    }
}