// ------------------------------------------------------------------------------
// <copyright file="IfStatementTranspiler.cs" company="Drake53">
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
        public static void Transpile(this IfStatementSyntax ifStatementNode, ref StringBuilder sb)
        {
            _ = ifStatementNode ?? throw new ArgumentNullException(nameof(ifStatementNode));

            sb.Append("if ");
            ifStatementNode.ConditionExpressionNode.Transpile(ref sb);
            sb.Append(" then");
            sb.AppendLine();
            ifStatementNode.StatementListNode.Transpile(ref sb);
            if (ifStatementNode.EmptyElseClauseNode is null)
            {
                ifStatementNode.ElseClauseNode.Transpile(ref sb);
            }
            else
            {
                sb.Append("end");
            }
        }

        public static LuaIfStatementSyntax TranspileToLua(this IfStatementSyntax ifStatementNode)
        {
            _ = ifStatementNode ?? throw new ArgumentNullException(nameof(ifStatementNode));

            var ifStatement = new LuaIfStatementSyntax(ifStatementNode.ConditionExpressionNode.TranspileToLua());
            ifStatement.Body.Statements.AddRange(ifStatementNode.StatementListNode.TranspileToLua());

            if (ifStatementNode.ElseClauseNode is not null)
            {
                if (ifStatementNode.ElseClauseNode.ElseifNode is not null)
                {
                    ifStatement.ElseIfStatements.Add(ifStatementNode.ElseClauseNode.ElseifNode.TranspileToLua());
                }
                else
                {
                    ifStatement.Else = ifStatementNode.ElseClauseNode.ElseNode.TranspileToLua();
                }
            }

            return ifStatement;
        }
    }
}