// ------------------------------------------------------------------------------
// <copyright file="ArrayDeclaratorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaVariableDeclaratorSyntax Transpile(JassArrayDeclaratorSyntax arrayDeclarator)
        {
            LuaExpressionSyntax expression = arrayDeclarator.Type.GetToken().SyntaxKind switch
            {
                JassSyntaxKind.IntegerKeyword => new LuaInvocationExpressionSyntax("__jarray", "0"),
                JassSyntaxKind.RealKeyword => new LuaInvocationExpressionSyntax("__jarray", "0.0"),
                JassSyntaxKind.StringKeyword => new LuaInvocationExpressionSyntax("__jarray", LuaStringLiteralExpressionSyntax.Empty),
                JassSyntaxKind.BooleanKeyword => new LuaInvocationExpressionSyntax("__jarray", LuaIdentifierLiteralExpressionSyntax.False),

                _ => new LuaTableExpression(),
            };

            return new LuaVariableDeclaratorSyntax(Transpile(arrayDeclarator.IdentifierName), expression);
        }
    }
}