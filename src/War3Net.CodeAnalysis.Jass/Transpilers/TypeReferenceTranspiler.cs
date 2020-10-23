// ------------------------------------------------------------------------------
// <copyright file="TypeReferenceTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ParameterSyntax Transpile(this Syntax.TypeReferenceSyntax typeReferenceNode, TokenTranspileFlags flags)
        {
            _ = typeReferenceNode ?? throw new ArgumentNullException(nameof(typeReferenceNode));

            return SyntaxFactory.Parameter(
                SyntaxFactory.Identifier(
                    SyntaxTriviaList.Empty,
                    Microsoft.CodeAnalysis.CSharp.SyntaxKind.IdentifierToken,
                    typeReferenceNode.TypeReferenceNameToken.TranspileIdentifier(),
                    typeReferenceNode.TypeReferenceNameToken.ValueText,
                    SyntaxTriviaList.Empty))
            .WithType(
                typeReferenceNode.TypeNameNode.Transpile(flags));
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.TypeReferenceSyntax typeReferenceNode, ref StringBuilder sb)
        {
            _ = typeReferenceNode ?? throw new ArgumentNullException(nameof(typeReferenceNode));

            typeReferenceNode.TypeReferenceNameToken.TranspileIdentifier(ref sb);
        }
    }
}