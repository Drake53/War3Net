// ------------------------------------------------------------------------------
// <copyright file="LoopStatementTranspiler.cs" company="Drake53">
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
        [return: NotNullIfNotNull("loopStatement")]
        public LuaStatementSyntax? Transpile(LoopStatementSyntax? loopStatement)
        {
            if (loopStatement is null)
            {
                return null;
            }

            var whileStatement = new LuaWhileStatementSyntax(LuaIdentifierLiteralExpressionSyntax.True);
            whileStatement.Body.Statements.AddRange(Transpile(loopStatement.LineDelimiterNode));
            whileStatement.Body.Statements.AddRange(Transpile(loopStatement.StatementListNode));

            return whileStatement;
        }
    }
}