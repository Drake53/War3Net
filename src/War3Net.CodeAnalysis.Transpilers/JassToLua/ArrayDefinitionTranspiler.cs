// ------------------------------------------------------------------------------
// <copyright file="ArrayDefinitionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaVariableDeclaratorSyntax Transpile(ArrayDefinitionSyntax arrayDefinition)
        {
            _ = arrayDefinition ?? throw new ArgumentNullException(nameof(arrayDefinition));

            LuaExpressionSyntax equalsValueExpression = arrayDefinition.TypeNameNode.TypeNameToken.TokenType switch
            {
                SyntaxTokenType.IntegerKeyword => new LuaInvocationExpressionSyntax("__jarray", "0"),
                SyntaxTokenType.RealKeyword => new LuaInvocationExpressionSyntax("__jarray", "0.0"),
                SyntaxTokenType.StringKeyword => new LuaInvocationExpressionSyntax("__jarray", LuaStringLiteralExpressionSyntax.Empty),
                SyntaxTokenType.BooleanKeyword => new LuaInvocationExpressionSyntax("__jarray", LuaIdentifierLiteralExpressionSyntax.False),
                _ => new LuaTableExpression(),
            };

            return new LuaVariableDeclaratorSyntax(TranspileIdentifier(arrayDefinition.IdentifierNameNode), equalsValueExpression);
        }
    }
}