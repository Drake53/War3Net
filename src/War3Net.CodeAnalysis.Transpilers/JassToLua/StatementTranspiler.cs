// ------------------------------------------------------------------------------
// <copyright file="StatementTranspiler.cs" company="Drake53">
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
        public static void Transpile(this StatementSyntax statementNode, ref StringBuilder sb)
        {
            _ = statementNode ?? throw new ArgumentNullException(nameof(statementNode));

            statementNode.SetStatementNode?.Transpile(ref sb);
            statementNode.CallStatementNode?.Transpile(ref sb);
            statementNode.IfStatementNode?.Transpile(ref sb);
            statementNode.LoopStatementNode?.Transpile(ref sb);
            statementNode.ExitStatementNode?.Transpile(ref sb);
            statementNode.ReturnStatementNode?.Transpile(ref sb);
            statementNode.DebugStatementNode?.Transpile(ref sb);
        }

        public static LuaStatementSyntax TranspileToLua(this StatementSyntax statementNode)
        {
            _ = statementNode ?? throw new ArgumentNullException(nameof(statementNode));

            return statementNode.SetStatementNode?.TranspileToLua()
                ?? statementNode.CallStatementNode?.TranspileToLua()
                ?? statementNode.IfStatementNode?.TranspileToLua()
                ?? statementNode.LoopStatementNode?.TranspileToLua()
                ?? statementNode.ExitStatementNode?.TranspileToLua()
                ?? statementNode.ReturnStatementNode?.TranspileToLua()
                ?? statementNode.DebugStatementNode.TranspileToLua();
        }
    }
}