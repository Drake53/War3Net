// ------------------------------------------------------------------------------
// <copyright file="GlobalConstantDeclarationTranspiler.cs" company="Drake53">
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
        public IEnumerable<LuaStatementSyntax> Transpile(GlobalConstantDeclarationSyntax globalConstantDeclaration)
        {
            _ = globalConstantDeclaration ?? throw new ArgumentNullException(nameof(globalConstantDeclaration));

            var variableList = new LuaVariableListDeclarationSyntax();
            var declarator = new LuaVariableDeclaratorSyntax(
                TranspileIdentifier(globalConstantDeclaration.IdentifierNameNode),
                Transpile(globalConstantDeclaration.EqualsValueClause));

            declarator.IsLocalDeclaration = false;
            variableList.Variables.Add(declarator);
            RegisterGlobalVariableType(globalConstantDeclaration);

            return new LuaStatementSyntax[] { new LuaLocalDeclarationStatementSyntax(variableList) }.Concat(Transpile(globalConstantDeclaration.LineDelimiterNode));
        }
    }
}