// ------------------------------------------------------------------------------
// <copyright file="ArrayDefinitionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Text;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        public static void TranspileGlobal(this ArrayDefinitionSyntax arrayDefinitionNode, ref StringBuilder sb)
        {
            _ = arrayDefinitionNode ?? throw new ArgumentNullException(nameof(arrayDefinitionNode));

            arrayDefinitionNode.Transpile(ref sb);

            if (arrayDefinitionNode.TypeNameNode.TypeNameToken.TokenType == SyntaxTokenType.StringKeyword)
            {
                TranspileStringConcatenationHandler.RegisterGlobalStringVariable(arrayDefinitionNode.IdentifierNameNode.ValueText);
            }
        }

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
    }
}