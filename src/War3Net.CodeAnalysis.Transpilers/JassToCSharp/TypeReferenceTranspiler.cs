// ------------------------------------------------------------------------------
// <copyright file="TypeReferenceTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static ParameterSyntax Transpile(this Jass.Syntax.TypeReferenceSyntax typeReferenceNode, TokenTranspileFlags flags)
        {
            _ = typeReferenceNode ?? throw new ArgumentNullException(nameof(typeReferenceNode));

            return SyntaxFactory.Parameter(
                SyntaxFactory.Identifier(
                    SyntaxTriviaList.Empty,
                    SyntaxKind.IdentifierToken,
                    typeReferenceNode.TypeReferenceNameToken.TranspileIdentifier(),
                    typeReferenceNode.TypeReferenceNameToken.ValueText,
                    SyntaxTriviaList.Empty))
            .WithType(
                typeReferenceNode.TypeNameNode.Transpile(flags));
        }
    }
}