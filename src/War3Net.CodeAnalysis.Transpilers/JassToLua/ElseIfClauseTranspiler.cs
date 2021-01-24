// ------------------------------------------------------------------------------
// <copyright file="ElseIfClauseTranspiler.cs" company="Drake53">
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
        public LuaElseIfStatementSyntax Transpile(JassElseIfClauseSyntax elseIfClause)
        {
            var elseifStatement = new LuaElseIfStatementSyntax(Transpile(elseIfClause.Condition, out _));

            elseifStatement.Body.Statements.AddRange(Transpile(elseIfClause.Body));

            return elseifStatement;
        }
    }
}