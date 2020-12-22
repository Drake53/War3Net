// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaStatementSyntax> Transpile(GlobalDeclarationSyntax globalDeclaration)
        {
            _ = globalDeclaration ?? throw new ArgumentNullException(nameof(globalDeclaration));

            return globalDeclaration.VariableDeclarationNode is null
                ? Transpile(globalDeclaration.ConstantDeclarationNode)
                : Transpile(globalDeclaration.VariableDeclarationNode);
        }
    }
}