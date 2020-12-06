// ------------------------------------------------------------------------------
// <copyright file="DeclarationListTranspiler.cs" company="Drake53">
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
        public static void Transpile(this DeclarationListSyntax declarationListNode, ref StringBuilder sb)
        {
            _ = declarationListNode ?? throw new ArgumentNullException(nameof(declarationListNode));

            foreach (var declaration in declarationListNode)
            {
                declaration.Transpile(ref sb);
            }
        }

        public static IEnumerable<LuaStatementSyntax> TranspileToLua(this DeclarationListSyntax declarationListNode)
        {
            _ = declarationListNode ?? throw new ArgumentNullException(nameof(declarationListNode));

            return declarationListNode.SelectMany(declaration => declaration.TranspileToLua());
        }
    }
}