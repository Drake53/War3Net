// ------------------------------------------------------------------------------
// <copyright file="TypeReferenceTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this TypeReferenceSyntax typeReferenceNode, ref StringBuilder sb)
        {
            _ = typeReferenceNode ?? throw new ArgumentNullException(nameof(typeReferenceNode));

            typeReferenceNode.TypeReferenceNameToken.TranspileIdentifier(ref sb);

            if (typeReferenceNode.TypeNameNode.TypeNameToken.TokenType == SyntaxTokenType.StringKeyword)
            {
                TranspileStringConcatenationHandler.RegisterLocalStringVariable(typeReferenceNode.TypeReferenceNameToken.ValueText);
            }
        }

        public static LuaIdentifierNameSyntax TranspileToLua(this TypeReferenceSyntax typeReferenceNode)
        {
            _ = typeReferenceNode ?? throw new ArgumentNullException(nameof(typeReferenceNode));

            return typeReferenceNode.TypeReferenceNameToken.TranspileIdentifierToLua();
        }
    }
}