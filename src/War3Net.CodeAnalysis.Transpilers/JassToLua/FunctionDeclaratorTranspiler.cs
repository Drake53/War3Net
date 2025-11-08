// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclaratorTranspiler.cs" company="Drake53">
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
        public LuaVariableDeclaratorSyntax Transpile(JassFunctionDeclaratorSyntax functionDeclarator)
        {
            RegisterFunctionReturnType(functionDeclarator);

            var functionExpression = new LuaFunctionExpressionSyntax();
            functionExpression.AddParameters(Transpile(functionDeclarator.ParameterList));

            return new LuaVariableDeclaratorSyntax(Transpile(functionDeclarator.IdentifierName), functionExpression);
        }
    }
}