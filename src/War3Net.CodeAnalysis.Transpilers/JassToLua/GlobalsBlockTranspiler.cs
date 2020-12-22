// ------------------------------------------------------------------------------
// <copyright file="GlobalsBlockTranspiler.cs" company="Drake53">
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
        public IEnumerable<LuaStatementSyntax> Transpile(GlobalsBlockSyntax globalsBlock)
        {
            _ = globalsBlock ?? throw new ArgumentNullException(nameof(globalsBlock));

            return Transpile(globalsBlock.LineDelimiterNode).Concat(globalsBlock.GlobalDeclarationListNode.SelectMany(declaration => Transpile(declaration)));
        }
    }
}