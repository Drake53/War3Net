// ------------------------------------------------------------------------------
// <copyright file="LineDelimiterTranspiler.cs" company="Drake53">
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
        public IEnumerable<LuaStatementSyntax> Transpile(LineDelimiterSyntax lineDelimiter)
        {
            _ = lineDelimiter ?? throw new ArgumentNullException(nameof(lineDelimiter));

            return lineDelimiter.SkipWhile((eol, i) => i == 0 && eol.Comment is null).Select(eolNode => Transpile(eolNode));
        }
    }
}