// ------------------------------------------------------------------------------
// <copyright file="NewStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaStatementSyntax> Transpile(NewStatementSyntax newStatement)
        {
            _ = newStatement ?? throw new ArgumentNullException(nameof(newStatement));

            return new[] { Transpile(newStatement.StatementNode) }.Concat(Transpile(newStatement.LineDelimiterNode));
        }
    }
}