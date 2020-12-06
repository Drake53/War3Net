// ------------------------------------------------------------------------------
// <copyright file="GlobalsBlockTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this GlobalsBlockSyntax globalsBlockNode, ref StringBuilder sb)
        {
            _ = globalsBlockNode ?? throw new ArgumentNullException(nameof(globalsBlockNode));

            foreach (var declaration in globalsBlockNode.GlobalDeclarationListNode)
            {
                declaration.Transpile(ref sb);
            }
        }

        public static IEnumerable<LuaStatementSyntax> TranspileToLua(this GlobalsBlockSyntax globalsBlockNode)
        {
            _ = globalsBlockNode ?? throw new ArgumentNullException(nameof(globalsBlockNode));

            return globalsBlockNode.GlobalDeclarationListNode.Select(declaration => new LuaLocalDeclarationStatementSyntax(declaration.TranspileToLua()));
        }
    }
}