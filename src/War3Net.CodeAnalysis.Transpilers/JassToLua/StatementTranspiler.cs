// ------------------------------------------------------------------------------
// <copyright file="StatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(StatementSyntax statement)
        {
            _ = statement ?? throw new ArgumentNullException(nameof(statement));

            return Transpile(statement.SetStatementNode)
                ?? Transpile(statement.CallStatementNode)
                ?? Transpile(statement.IfStatementNode)
                ?? Transpile(statement.LoopStatementNode)
                ?? Transpile(statement.ExitStatementNode)
                ?? Transpile(statement.ReturnStatementNode)
                ?? Transpile(statement.DebugStatementNode)
                ?? throw new ArgumentNullException(nameof(statement));
        }
    }
}