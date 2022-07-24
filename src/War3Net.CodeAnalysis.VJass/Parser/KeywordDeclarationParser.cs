// ------------------------------------------------------------------------------
// <copyright file="KeywordDeclarationParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassKeywordDeclarationSyntax> GetKeywordDeclarationParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (keywordToken, identifierName) => new VJassKeywordDeclarationSyntax(
                    ImmutableArray<VJassModifierSyntax>.Empty,
                    keywordToken,
                    identifierName),
                Keyword.Keyword_.AsToken(triviaParser, VJassSyntaxKind.KeywordKeyword),
                identifierNameParser.Labelled("keyword identifier name"));
        }

        internal static Parser<char, Func<IEnumerable<VJassModifierSyntax>, VJassScopedDeclarationSyntax>> GetScopedKeywordDeclarationParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map<char, VJassSyntaxToken, VJassIdentifierNameSyntax, Func<IEnumerable<VJassModifierSyntax>, VJassScopedDeclarationSyntax>>(
                (keywordToken, identifierName) => modifiers => new VJassKeywordDeclarationSyntax(
                    modifiers.ToImmutableArray(),
                    keywordToken,
                    identifierName),
                Keyword.Keyword_.AsToken(triviaParser, VJassSyntaxKind.KeywordKeyword),
                identifierNameParser.Labelled("keyword identifier name"));
        }
    }
}