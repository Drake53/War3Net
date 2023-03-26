// ------------------------------------------------------------------------------
// <copyright file="VariableDeclaratorTranspiler.cs" company="Drake53">
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
        public LuaVariableDeclaratorSyntax Transpile(JassVariableDeclaratorSyntax variableDeclarator)
        {
            var expression = variableDeclarator.Value is null
                ? LuaIdentifierLiteralExpressionSyntax.Nil
                : Transpile(variableDeclarator.Value);

            return new LuaVariableDeclaratorSyntax(Transpile(variableDeclarator.IdentifierName), expression);
        }
    }
}