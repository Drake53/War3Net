// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassGlobalDeclarationSyntax globalDeclaration)
        {
            return globalDeclaration switch
            {
                JassGlobalConstantDeclarationSyntax globalConstantDeclaration => Transpile(globalConstantDeclaration),
                JassGlobalVariableDeclarationSyntax globalVariableDeclaration => Transpile(globalVariableDeclaration.Declarator, false),
            };
        }

        public LuaStatementSyntax Transpile(JassGlobalConstantDeclarationSyntax globalConstantDeclaration)
        {
            RegisterVariableType(globalConstantDeclaration);

            var expression = Transpile(globalConstantDeclaration.EqualsValueClause);

            var luaDeclarator = new LuaVariableDeclaratorSyntax(Transpile(globalConstantDeclaration.IdentifierName), expression);
            luaDeclarator.IsLocalDeclaration = false;

            var declaration = new LuaVariableListDeclarationSyntax();
            declaration.Variables.Add(luaDeclarator);

            return new LuaLocalDeclarationStatementSyntax(declaration);
        }
    }
}