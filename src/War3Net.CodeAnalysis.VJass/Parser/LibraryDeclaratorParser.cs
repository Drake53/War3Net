// ------------------------------------------------------------------------------
// <copyright file="LibraryDeclaratorParser.cs" company="Drake53">
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
        internal static Parser<char, VJassLibraryDeclaratorSyntax> GetLibraryDeclaratorParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassInitializerSyntax> initializerParser,
            Parser<char, VJassLibraryDependencyListSyntax> libraryDependencyListParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (libraryToken, identifierName, initializer, dependencies, trailingTrivia) => new VJassLibraryDeclaratorSyntax(
                    libraryToken,
                    identifierName,
                    initializer.GetValueOrDefault(),
                    dependencies.GetValueOrDefault()).AppendTrivia(trailingTrivia),
                Keyword.Library.AsToken(triviaParser, VJassSyntaxKind.LibraryKeyword),
                identifierNameParser.Labelled("library identifier name"),
                initializerParser.Optional(),
                libraryDependencyListParser.Optional(),
                trailingTriviaParser);
        }
    }
}