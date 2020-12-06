// ------------------------------------------------------------------------------
// <copyright file="NewDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this NewDeclarationSyntax newDeclarationNode, ref StringBuilder sb)
        {
            _ = newDeclarationNode ?? throw new ArgumentNullException(nameof(newDeclarationNode));

            newDeclarationNode.Declaration.Transpile(ref sb);
        }

        public static IEnumerable<LuaStatementSyntax> TranspileToLua(this NewDeclarationSyntax newDeclarationNode)
        {
            _ = newDeclarationNode ?? throw new ArgumentNullException(nameof(newDeclarationNode));

            return newDeclarationNode.Declaration.TranspileToLua();
        }
    }
}