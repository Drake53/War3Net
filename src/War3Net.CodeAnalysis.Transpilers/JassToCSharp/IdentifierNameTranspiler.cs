// ------------------------------------------------------------------------------
// <copyright file="IdentifierNameTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        private const string AntiReservedKeywordConflictPrefix = "@";

        private static readonly Lazy<HashSet<string>> _reservedKeywords = new Lazy<HashSet<string>>(() => GetReservedKeywords().ToHashSet(StringComparer.Ordinal));

        public SyntaxToken Transpile(JassIdentifierNameSyntax identifierName)
        {
            var text = _reservedKeywords.Value.Contains(identifierName.Token.Text)
                ? $"{AntiReservedKeywordConflictPrefix}{identifierName.Token.Text}"
                : identifierName.Token.Text;

            return SyntaxFactory.Identifier(
                SyntaxTriviaList.Empty,
                SyntaxKind.IdentifierToken,
                text,
                identifierName.Token.Text,
                SyntaxTriviaList.Empty);
        }

        public ExpressionSyntax TranspileIdentifierName(JassIdentifierNameSyntax identifierName)
        {
            return SyntaxFactory.IdentifierName(Transpile(identifierName));
        }

        private static IEnumerable<string> GetReservedKeywords()
        {
            foreach (SyntaxKind syntaxKind in Enum.GetValues(typeof(SyntaxKind)))
            {
                if (SyntaxFacts.IsReservedKeyword(syntaxKind))
                {
                    yield return SyntaxFactory.Token(syntaxKind).ValueText;
                }
            }
        }
    }
}