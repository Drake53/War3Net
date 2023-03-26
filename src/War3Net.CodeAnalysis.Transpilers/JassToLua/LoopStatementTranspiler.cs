﻿// ------------------------------------------------------------------------------
// <copyright file="LoopStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassLoopStatementSyntax loopStatement)
        {
            var whileStatement = new LuaWhileStatementSyntax(LuaIdentifierLiteralExpressionSyntax.True);

            whileStatement.Body.Statements.AddRange(Transpile(loopStatement.Statements));

            return whileStatement;
        }
    }
}