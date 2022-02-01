﻿// ------------------------------------------------------------------------------
// <copyright file="StatementListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaStatementSyntax> Transpile(JassStatementListSyntax statementList)
        {
            return statementList.Statements
                .Where(statement => !(statement is JassCommentSyntax && IgnoreComments))
                .Where(statement => !(statement is JassEmptySyntax && IgnoreEmptyStatements))
                .Select(Transpile);
        }
    }
}