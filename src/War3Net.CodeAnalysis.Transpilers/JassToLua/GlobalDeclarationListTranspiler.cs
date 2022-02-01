// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationListTranspiler.cs" company="Drake53">
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
        public IEnumerable<LuaStatementSyntax> Transpile(JassGlobalDeclarationListSyntax globalDeclarationList)
        {
            return globalDeclarationList.Globals
                .Where(declaration => !(declaration is JassCommentSyntax && IgnoreComments))
                .Where(declaration => !(declaration is JassEmptySyntax && IgnoreEmptyDeclarations))
                .Select(declaration => declaration switch
            {
                JassEmptySyntax empty => Transpile(empty),
                JassCommentSyntax comment => Transpile(comment),
                JassGlobalDeclarationSyntax globalDeclaration => Transpile(globalDeclaration),
            });
        }
    }
}