// ------------------------------------------------------------------------------
// <copyright file="GlobalVariableDeclarationTranspiler.cs" company="Drake53">
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
        public IEnumerable<LuaStatementSyntax> Transpile(GlobalVariableDeclarationSyntax globalVariableDeclaration)
        {
            _ = globalVariableDeclaration ?? throw new ArgumentNullException(nameof(globalVariableDeclaration));

            return new LuaStatementSyntax[] { Transpile(globalVariableDeclaration.DeclarationNode, false) }.Concat(Transpile(globalVariableDeclaration.LineDelimiterNode));
        }
    }
}