// ------------------------------------------------------------------------------
// <copyright file="ArrayDeclaratorTranspiler.cs" company="Drake53">
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
        public LuaVariableDeclaratorSyntax Transpile(JassArrayDeclaratorSyntax arrayDeclarator)
        {
            LuaExpressionSyntax expression = arrayDeclarator.Type switch
            {
                JassTypeSyntax type when type.Equals(JassTypeSyntax.Integer) => new LuaInvocationExpressionSyntax("__jarray", "0"),
                JassTypeSyntax type when type.Equals(JassTypeSyntax.Real) => new LuaInvocationExpressionSyntax("__jarray", "0.0"),
                JassTypeSyntax type when type.Equals(JassTypeSyntax.String) => new LuaInvocationExpressionSyntax("__jarray", LuaStringLiteralExpressionSyntax.Empty),
                JassTypeSyntax type when type.Equals(JassTypeSyntax.Boolean) => new LuaInvocationExpressionSyntax("__jarray", LuaIdentifierLiteralExpressionSyntax.False),
                _ => new LuaTableExpression(),
            };

            return new LuaVariableDeclaratorSyntax(Transpile(arrayDeclarator.IdentifierName), expression);
        }
    }
}