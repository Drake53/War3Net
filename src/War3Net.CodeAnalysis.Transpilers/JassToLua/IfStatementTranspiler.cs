// ------------------------------------------------------------------------------
// <copyright file="IfStatementTranspiler.cs" company="Drake53">
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
    }
}