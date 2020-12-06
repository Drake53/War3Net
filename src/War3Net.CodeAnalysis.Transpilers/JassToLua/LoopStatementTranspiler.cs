// ------------------------------------------------------------------------------
// <copyright file="LoopStatementTranspiler.cs" company="Drake53">
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
        public static void Transpile(this LoopStatementSyntax loopStatementNode, ref StringBuilder sb)
        {
            _ = loopStatementNode ?? throw new ArgumentNullException(nameof(loopStatementNode));

            sb.AppendLine("while (true)");
            sb.AppendLine("do");
            loopStatementNode.StatementListNode.Transpile(ref sb);
            sb.Append("end");
        }

        public static LuaWhileStatementSyntax TranspileToLua(this LoopStatementSyntax loopStatementNode)
        {
            _ = loopStatementNode ?? throw new ArgumentNullException(nameof(loopStatementNode));

            var whileStatement = new LuaWhileStatementSyntax(LuaIdentifierLiteralExpressionSyntax.True);
            whileStatement.Body.Statements.AddRange(loopStatementNode.StatementListNode.TranspileToLua());

            return whileStatement;
        }
    }
}