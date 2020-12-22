// ------------------------------------------------------------------------------
// <copyright file="VariableDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaLocalDeclarationStatementSyntax Transpile(VariableDeclarationSyntax variableDeclaration, bool isLocalDeclaration)
        {
            _ = variableDeclaration ?? throw new ArgumentNullException(nameof(variableDeclaration));

            var declaration = new LuaVariableListDeclarationSyntax();
            if (variableDeclaration.ArrayDefinitionNode is not null)
            {
                declaration.Variables.Add(Transpile(variableDeclaration.ArrayDefinitionNode));
                RegisterVariableType(variableDeclaration.ArrayDefinitionNode, isLocalDeclaration);
            }
            else if (variableDeclaration.VariableDefinitionNode is not null)
            {
                declaration.Variables.Add(Transpile(variableDeclaration.VariableDefinitionNode));
                RegisterVariableType(variableDeclaration.VariableDefinitionNode, isLocalDeclaration);
            }
            else
            {
                throw new ArgumentNullException(nameof(variableDeclaration));
            }

            declaration.Variables.Single().IsLocalDeclaration = isLocalDeclaration;

            return new LuaLocalDeclarationStatementSyntax(declaration);
        }
    }
}