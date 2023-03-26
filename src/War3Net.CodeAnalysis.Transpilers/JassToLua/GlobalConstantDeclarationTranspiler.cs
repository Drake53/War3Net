// ------------------------------------------------------------------------------
// <copyright file="GlobalConstantDeclarationTranspiler.cs" company="Drake53">
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
        public LuaStatementSyntax Transpile(JassGlobalConstantDeclarationSyntax globalDeclaration)
        {
            RegisterGlobalVariableType(globalDeclaration);

            var expression = Transpile(globalDeclaration.Value);

            var luaDeclarator = new LuaVariableDeclaratorSyntax(Transpile(globalDeclaration.IdentifierName), expression);

            var declaration = new LuaVariableListDeclarationSyntax();

            luaDeclarator.IsLocalDeclaration = false;
            declaration.Variables.Add(luaDeclarator);

            return new LuaLocalDeclarationStatementSyntax(declaration);
        }
    }
}