// ------------------------------------------------------------------------------
// <copyright file="LibraryDependencyParser.cs" company="Drake53">
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
        internal static Parser<char, VJassLibraryDependencySyntax> GetLibraryDependencyParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (optionalToken, identifierName, commaToken) => new VJassLibraryDependencySyntax(
                    optionalToken.GetValueOrDefault(),
                    identifierName,
                    commaToken.GetValueOrDefault()),
                Keyword.Optional.AsToken(triviaParser, VJassSyntaxKind.OptionalKeyword).Optional(),
                identifierNameParser.Labelled("library dependency identifier name"),
                Symbol.Comma.AsToken(triviaParser, VJassSyntaxKind.CommaToken, VJassSymbol.Comma).Optional());
        }
    }
}