// ------------------------------------------------------------------------------
// <copyright file="SetStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        [return: NotNullIfNotNull("setStatement")]
        public LuaStatementSyntax? Transpile(SetStatementSyntax? setStatement)
        {
            if (setStatement is null)
            {
                return null;
            }

            var left = setStatement.ArrayIndexerNode is null
                ? TranspileExpression(setStatement.IdentifierNameNode)
                : new LuaTableIndexAccessExpressionSyntax(TranspileExpression(setStatement.IdentifierNameNode), Transpile(setStatement.ArrayIndexerNode));

            return new LuaExpressionStatementSyntax(new LuaAssignmentExpressionSyntax(left, Transpile(setStatement.EqualsValueClauseNode)));
        }
    }
}