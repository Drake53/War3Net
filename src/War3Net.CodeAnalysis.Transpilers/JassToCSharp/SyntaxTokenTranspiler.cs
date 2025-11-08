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

        public SyntaxToken Transpile(JassSyntaxToken token)
        {
            var text = _reservedKeywords.Value.Contains(token.Text)
                ? $"{AntiReservedKeywordConflictPrefix}{token.Text}"
                : token.Text;

            return SyntaxFactory.Identifier(
                Transpile(token.LeadingTrivia),
                SyntaxKind.IdentifierToken,
                text,
                token.Text,
                Transpile(token.TrailingTrivia));
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