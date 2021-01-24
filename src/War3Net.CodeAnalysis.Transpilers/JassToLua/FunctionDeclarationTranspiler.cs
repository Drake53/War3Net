// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationTranspiler.cs" company="Drake53">
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
        public LuaStatementSyntax Transpile(JassFunctionDeclarationSyntax functionDeclaration)
        {
            RegisterFunctionReturnType(functionDeclaration.FunctionDeclarator);

            var functionExpression = new LuaFunctionExpressionSyntax();
            functionExpression.AddParameters(Transpile(functionDeclaration.FunctionDeclarator.ParameterList));
            functionExpression.Body.Statements.AddRange(Transpile(functionDeclaration.Body));
            functionExpression.RenderAsFunctionDefinition = true;

            _localTypes.Clear();

            var luaFunctionDeclaration = new LuaVariableDeclaratorSyntax(Transpile(functionDeclaration.FunctionDeclarator.IdentifierName), functionExpression);
            luaFunctionDeclaration.IsLocalDeclaration = false;

            var declaration = new LuaVariableListDeclarationSyntax();
            declaration.Variables.Add(luaFunctionDeclaration);

            return new LuaLocalDeclarationStatementSyntax(declaration);
        }
    }
}