// ------------------------------------------------------------------------------
// <copyright file="SyntaxTokenTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        private const string AntiReservedKeywordConflictPrefix = "@";

        private static readonly Lazy<HashSet<string>> _reservedKeywords = new Lazy<HashSet<string>>(() => GetReservedKeywords().ToHashSet(StringComparer.Ordinal));

        public string TranspileText(
            string tokenText)
        {
            return _reservedKeywords.Value.Contains(tokenText)
                ? $"{AntiReservedKeywordConflictPrefix}{tokenText}"
                : tokenText;
        }

        public string TranspileTextAligned(
            string tokenText,
            out bool prefixAdded)
        {
            prefixAdded = _reservedKeywords.Value.Contains(tokenText);
            return prefixAdded
                ? $"{AntiReservedKeywordConflictPrefix}{tokenText}"
                : tokenText;
        }

        public SyntaxToken Transpile(
            JassSyntaxToken token)
        {
            return Transpile(token.LeadingTrivia, token, token.TrailingTrivia);
        }

        public SyntaxToken Transpile(
            JassSyntaxToken token,
            JassSyntaxToken triviaFromToken)
        {
            return Transpile(triviaFromToken.LeadingTrivia, token, triviaFromToken.TrailingTrivia);
        }

        public SyntaxToken Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassSyntaxToken token)
        {
            return Transpile(leadingTrivia, token, token.TrailingTrivia);
        }

        public SyntaxToken Transpile(
            JassSyntaxToken token,
            JassSyntaxTriviaList trailingTrivia)
        {
            return Transpile(token.LeadingTrivia, token, trailingTrivia);
        }

        public SyntaxToken Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassSyntaxToken token,
            JassSyntaxTriviaList trailingTrivia)
        {
            return SyntaxFactory.Identifier(
                Transpile(leadingTrivia),
                SyntaxKind.IdentifierToken,
                TranspileText(token.Text),
                token.Text,
                Transpile(trailingTrivia));
        }

        public SyntaxToken TranspileAligned(
            JassSyntaxToken token,
            out bool prefixAdded)
        {
            return TranspileAligned(token.LeadingTrivia, token, token.TrailingTrivia, out prefixAdded);
        }

        public SyntaxToken TranspileAligned(
            JassSyntaxTriviaList leadingTrivia,
            JassSyntaxToken token,
            out bool prefixAdded)
        {
            return TranspileAligned(leadingTrivia, token, token.TrailingTrivia, out prefixAdded);
        }

        public SyntaxToken TranspileAligned(
            JassSyntaxToken token,
            JassSyntaxTriviaList trailingTrivia,
            out bool prefixAdded)
        {
            return TranspileAligned(token.LeadingTrivia, token, trailingTrivia, out prefixAdded);
        }

        public SyntaxToken TranspileAligned(
            JassSyntaxTriviaList leadingTrivia,
            JassSyntaxToken token,
            JassSyntaxTriviaList trailingTrivia,
            out bool prefixAdded)
        {
            return SyntaxFactory.Identifier(
                Transpile(leadingTrivia),
                SyntaxKind.IdentifierToken,
                TranspileTextAligned(token.Text, out prefixAdded),
                token.Text,
                Transpile(trailingTrivia));
        }

        public SyntaxToken Transpile(string text)
        {
            return SyntaxFactory.Identifier(
                SyntaxTriviaList.Empty,
                SyntaxKind.IdentifierToken,
                TranspileText(text),
                text,
                SyntaxTriviaList.Empty);
        }

        public SyntaxToken Transpile(
            string text,
            JassSyntaxToken triviaFromToken)
        {
            return SyntaxFactory.Identifier(
                Transpile(triviaFromToken.LeadingTrivia),
                SyntaxKind.IdentifierToken,
                TranspileText(text),
                text,
                Transpile(triviaFromToken.TrailingTrivia));
        }

        public SyntaxToken Transpile(
            JassSyntaxTriviaList leadingTrivia,
            string text,
            JassSyntaxTriviaList trailingTrivia)
        {
            return SyntaxFactory.Identifier(
                Transpile(leadingTrivia),
                SyntaxKind.IdentifierToken,
                TranspileText(text),
                text,
                Transpile(trailingTrivia));
        }

        public SyntaxToken Transpile(
            SyntaxKind syntaxKind,
            JassSyntaxToken triviaFromToken)
        {
            return Transpile(triviaFromToken.LeadingTrivia, syntaxKind, triviaFromToken.TrailingTrivia);
        }

        public SyntaxToken Transpile(
            JassSyntaxTriviaList leadingTrivia,
            SyntaxKind syntaxKind)
        {
            return SyntaxFactory.Token(
                Transpile(leadingTrivia),
                syntaxKind,
                SyntaxTriviaList.Empty);
        }

        public SyntaxToken Transpile(
            SyntaxKind syntaxKind,
            JassSyntaxTriviaList trailingTrivia)
        {
            return SyntaxFactory.Token(
                SyntaxTriviaList.Empty,
                syntaxKind,
                Transpile(trailingTrivia));
        }

        public SyntaxToken Transpile(
            JassSyntaxTriviaList leadingTrivia,
            SyntaxKind syntaxKind,
            JassSyntaxTriviaList trailingTrivia)
        {
            return SyntaxFactory.Token(
                Transpile(leadingTrivia),
                syntaxKind,
                Transpile(trailingTrivia));
        }

        private static IEnumerable<string> GetReservedKeywords()
        {
            foreach (SyntaxKind syntaxKind in Enum.GetValues(typeof(SyntaxKind)))
            {
                if (SyntaxFacts.IsReservedKeyword(syntaxKind))
                {
                    yield return SyntaxFactory.Token(syntaxKind).Text;
                }
            }
        }
    }
}