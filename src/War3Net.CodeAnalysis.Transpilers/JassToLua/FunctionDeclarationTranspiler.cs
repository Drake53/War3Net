// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this FunctionDeclarationSyntax functionDeclarationNode, ref StringBuilder sb)
        {
            _ = functionDeclarationNode ?? throw new ArgumentNullException(nameof(functionDeclarationNode));

            sb.Append("function ");
            functionDeclarationNode.IdentifierNode.TranspileIdentifier(ref sb);
            sb.Append('(');
            functionDeclarationNode.ParameterListReferenceNode.Transpile(ref sb);
            sb.Append(')');

            if (functionDeclarationNode.ReturnTypeNode.TypeNameNode?.TypeNameToken.TokenType == SyntaxTokenType.StringKeyword)
            {
                TranspileStringConcatenationHandler.RegisterFunctionWithStringReturnType(functionDeclarationNode.IdentifierNode.ValueText);
            }
        }
    }
}