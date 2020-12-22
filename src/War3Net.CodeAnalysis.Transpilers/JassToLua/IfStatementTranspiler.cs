// ------------------------------------------------------------------------------
// <copyright file="IfStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        [return: NotNullIfNotNull("ifStatement")]
        public LuaStatementSyntax? Transpile(IfStatementSyntax? ifStatement)
        {
            if (ifStatement is null)
            {
                return null;
            }

            var statement = new LuaIfStatementSyntax(Transpile(ifStatement.ConditionExpressionNode));
            statement.Body.Statements.AddRange(Transpile(ifStatement.LineDelimiterNode));
            statement.Body.Statements.AddRange(Transpile(ifStatement.StatementListNode));

            var elseClause = ifStatement.ElseClauseNode;
            while (elseClause is not null)
            {
                if (elseClause.ElseifNode is not null)
                {
                    statement.ElseIfStatements.Add(Transpile(elseClause.ElseifNode));
                    elseClause = elseClause.ElseifNode.ElseClauseNode;
                }
                else if (elseClause.ElseNode is not null)
                {
                    statement.Else = Transpile(elseClause.ElseNode);
                    break;
                }
                else
                {
                    throw new ArgumentNullException(nameof(ifStatement));
                }
            }

            return statement;
        }
    }
}