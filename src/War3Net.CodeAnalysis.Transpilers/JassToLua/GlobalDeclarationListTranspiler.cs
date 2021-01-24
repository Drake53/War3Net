// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationListTranspiler.cs" company="Drake53">
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
        public IEnumerable<LuaStatementSyntax> Transpile(JassGlobalDeclarationListSyntax globalDeclarationList)
        {
            return globalDeclarationList.Globals.Select(declaration => declaration switch
            {
                JassEmptyDeclarationSyntax emptyDeclaration => Transpile(emptyDeclaration),
                JassCommentDeclarationSyntax commentDeclaration => Transpile(commentDeclaration),
                JassGlobalDeclarationSyntax globalDeclaration => Transpile(globalDeclaration),
            });
        }
    }
}