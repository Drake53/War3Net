// ------------------------------------------------------------------------------
// <copyright file="ArrayDefinitionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void TranspileGlobal(this ArrayDefinitionSyntax arrayDefinitionNode, ref StringBuilder sb)
        {
            _ = arrayDefinitionNode ?? throw new ArgumentNullException(nameof(arrayDefinitionNode));

            arrayDefinitionNode.Transpile(ref sb);

            if (arrayDefinitionNode.TypeNameNode.TypeNameToken.TokenType == SyntaxTokenType.StringKeyword)
            {
                TranspileStringConcatenationHandler.RegisterGlobalStringVariable(arrayDefinitionNode.IdentifierNameNode.ValueText);
            }
        }

        [Obsolete]
        public static void TranspileLocal(this ArrayDefinitionSyntax arrayDefinitionNode, ref StringBuilder sb)
        {
            _ = arrayDefinitionNode ?? throw new ArgumentNullException(nameof(arrayDefinitionNode));

            sb.Append("local ");
            arrayDefinitionNode.Transpile(ref sb);

            if (arrayDefinitionNode.TypeNameNode.TypeNameToken.TokenType == SyntaxTokenType.StringKeyword)
            {
                TranspileStringConcatenationHandler.RegisterLocalStringVariable(arrayDefinitionNode.IdentifierNameNode.ValueText);
            }
        }

        [Obsolete]
        private static void Transpile(this ArrayDefinitionSyntax arrayDefinitionNode, ref StringBuilder sb)
        {
            arrayDefinitionNode.IdentifierNameNode.TranspileIdentifier(ref sb);
            sb.Append(" = ");

            var tokenType = arrayDefinitionNode.TypeNameNode.TypeNameToken.TokenType;
            switch (tokenType)
            {
                case SyntaxTokenType.HandleKeyword:
                case SyntaxTokenType.AlphanumericIdentifier:
                    sb.Append("{}");
                    break;

                case SyntaxTokenType.IntegerKeyword:
                    sb.Append("__jarray(0)");
                    break;

                case SyntaxTokenType.RealKeyword:
                    sb.Append("__jarray(0.0)");
                    break;

                case SyntaxTokenType.StringKeyword:
                    sb.Append("__jarray(\"\")");
                    break;

                case SyntaxTokenType.BooleanKeyword:
                    sb.Append("__jarray(false)");
                    break;

                case SyntaxTokenType.CodeKeyword: throw new NotSupportedException("Code arrays are not supported.");

                default: throw new InvalidEnumArgumentException(nameof(tokenType), (int)tokenType, typeof(SyntaxTokenType));
            }
        }

        public static LuaVariableDeclaratorSyntax TranspileToLua(this ArrayDefinitionSyntax arrayDefinitionNode)
        {
            _ = arrayDefinitionNode ?? throw new ArgumentNullException(nameof(arrayDefinitionNode));

            LuaExpressionSyntax equalsValueExpression = arrayDefinitionNode.TypeNameNode.TypeNameToken.TokenType switch
            {
                SyntaxTokenType.IntegerKeyword => new LuaInvocationExpressionSyntax("__jarray", default(int)),
                SyntaxTokenType.RealKeyword => new LuaInvocationExpressionSyntax("__jarray", default(float)),
                SyntaxTokenType.StringKeyword => new LuaInvocationExpressionSyntax("__jarray", string.Empty),
                SyntaxTokenType.BooleanKeyword => new LuaInvocationExpressionSyntax("__jarray", LuaIdentifierLiteralExpressionSyntax.False),
                _ => new LuaTableExpression(),
            };

            return new LuaVariableDeclaratorSyntax(arrayDefinitionNode.IdentifierNameNode.TranspileIdentifierToLua(), equalsValueExpression);
        }
    }
}