// ------------------------------------------------------------------------------
// <copyright file="IdentifierNameTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers.Extensions;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public IdentifierNameSyntax Transpile(
            JassIdentifierNameSyntax identifierName)
        {
            return SyntaxFactory.IdentifierName(Transpile(identifierName.Token));
        }

        public IdentifierNameSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassIdentifierNameSyntax identifierName)
        {
            return SyntaxFactory.IdentifierName(Transpile(leadingTrivia, identifierName.Token));
        }

        public IdentifierNameSyntax Transpile(
            JassIdentifierNameSyntax identifierName,
            JassSyntaxTriviaList trailingTrivia)
        {
            return SyntaxFactory.IdentifierName(Transpile(identifierName.Token, trailingTrivia));
        }

        public IdentifierNameSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassIdentifierNameSyntax identifierName,
            JassSyntaxTriviaList trailingTrivia)
        {
            return SyntaxFactory.IdentifierName(Transpile(leadingTrivia, identifierName.Token, trailingTrivia));
        }

        public IdentifierNameSyntax TranspileAligned(
            JassIdentifierNameSyntax identifierName,
            bool isArray)
        {
            return SyntaxFactory.IdentifierName(TranspileAligned(identifierName.Token, out var prefixAdded))
                .WithAlignedWhitespace(GetWhitespaceDiff(prefixAdded, isArray));
        }

        public IdentifierNameSyntax TranspileAligned(
            JassSyntaxTriviaList leadingTrivia,
            JassIdentifierNameSyntax identifierName,
            bool isArray)
        {
            return SyntaxFactory.IdentifierName(TranspileAligned(leadingTrivia, identifierName.Token, out var prefixAdded))
                .WithAlignedWhitespace(GetWhitespaceDiff(prefixAdded, isArray));
        }

        public IdentifierNameSyntax TranspileAligned(
            JassIdentifierNameSyntax identifierName,
            JassSyntaxTriviaList trailingTrivia,
            bool isArray)
        {
            return SyntaxFactory.IdentifierName(TranspileAligned(identifierName.Token, trailingTrivia, out var prefixAdded))
                .WithAlignedWhitespace(GetWhitespaceDiff(prefixAdded, isArray));
        }

        public IdentifierNameSyntax TranspileAligned(
            JassSyntaxTriviaList leadingTrivia,
            JassIdentifierNameSyntax identifierName,
            JassSyntaxTriviaList trailingTrivia,
            bool isArray)
        {
            return SyntaxFactory.IdentifierName(TranspileAligned(leadingTrivia, identifierName.Token, trailingTrivia, out var prefixAdded))
                .WithAlignedWhitespace(GetWhitespaceDiff(prefixAdded, isArray));
        }

        private int GetWhitespaceDiff(bool prefixAdded, bool isArray)
        {
            return prefixAdded
                ? isArray
                    ? ArrayWhitespaceDiff + PrefixWhitespaceDiff
                    : PrefixWhitespaceDiff
                : isArray
                    ? ArrayWhitespaceDiff
                    : 0;
        }
    }
}