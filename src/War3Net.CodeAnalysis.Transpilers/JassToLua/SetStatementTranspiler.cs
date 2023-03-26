// ------------------------------------------------------------------------------
// <copyright file="SetStatementTranspiler.cs" company="Drake53">
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
        public LuaStatementSyntax Transpile(JassSetStatementSyntax setStatement)
        {
            return new LuaAssignmentExpressionSyntax(
                setStatement.ElementAccessClause is null
                    ? Transpile(setStatement.IdentifierName)
                    : new LuaTableIndexAccessExpressionSyntax(Transpile(setStatement.IdentifierName), Transpile(setStatement.ElementAccessClause.Expression, out _)),
                Transpile(setStatement.Value));
        }
    }
}