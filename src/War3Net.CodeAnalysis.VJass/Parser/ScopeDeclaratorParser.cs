// ------------------------------------------------------------------------------
// <copyright file="ScopeDeclaratorParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassScopeDeclaratorSyntax> GetScopeDeclaratorParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassInitializerSyntax> initializerParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (scopeToken, identifierName, initializer, trailingTrivia) => new VJassScopeDeclaratorSyntax(
                    scopeToken,
                    identifierName,
                    initializer.GetValueOrDefault()).AppendTrivia(trailingTrivia),
                Keyword.Scope.AsToken(triviaParser, VJassSyntaxKind.ScopeKeyword),
                identifierNameParser.Labelled("scope identifier name"),
                initializerParser.Optional(),
                trailingTriviaParser);
        }
    }
}