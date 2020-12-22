// ------------------------------------------------------------------------------
// <copyright file="ElseifTranspiler.cs" company="Drake53">
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
        public LuaElseIfStatementSyntax Transpile(ElseifSyntax elseif)
        {
            _ = elseif ?? throw new ArgumentNullException(nameof(elseif));

            var elseifStatement = new LuaElseIfStatementSyntax(Transpile(elseif.ConditionExpressionNode));
            elseifStatement.Body.Statements.AddRange(Transpile(elseif.LineDelimiterNode));
            elseifStatement.Body.Statements.AddRange(Transpile(elseif.StatementListNode));

            return elseifStatement;
        }
    }
}